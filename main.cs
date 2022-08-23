using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDB
{
    public class Program
    {
        public static async Task<List<Employee>> GetEmployees()
        {
            List<Employee> es = new List<Employee>();
            for (int i = 0; i < 5; ++i)
            {
                //Pretend this is a network call, or DB, or something long running
                es.Add(new Employee("i12345", "", "David", "Farrington", i.ToString()));
                await Task.Delay(10);
            }

            return es;
        }

        public static async Task<Dictionary<string, List<TimeRecord>>> GetRecords(string inumber, int pastDays, Func<Project, float> getProjectCodeSplit)
        {
            List<Project> codes = new List<Project>() {
              new Project() { ProjectCode = "LTACAPEX" },
                new Project() { ProjectCode = "GLTOPEX" },
                  new Project() { ProjectCode = "ASDFEX" }
            };

            Dictionary<string, List<TimeRecord>> records = new Dictionary<string, List<TimeRecord>>();
            for (int i = 0; i < pastDays; ++i)
            {
                var date = DateTime.Now.Subtract(TimeSpan.FromDays(i));

                foreach (var code in codes)
                {
                    float percent = getProjectCodeSplit(code);
                    if (!records.ContainsKey(code.ProjectCode))
                    {
                        records[code.ProjectCode] = new List<TimeRecord>();
                    }

                    //Simulate some unentered time
                    if (i > 10)
                    {
                        records[code.ProjectCode].Add(null);
                        continue;
                    }
                    //Pretend this is a network call, or DB, or something long running
                    records[code.ProjectCode].Add(new TimeRecord() { Date = date, Hours = 8 * percent, Project = code });
                    await Task.Delay(10);
                }
            }

            return records;
        }

        public static async Task Main(string[] args)
        {
            //Get All Employees
            var employees = await GetEmployees();
            //Get Time Records for each employee
            foreach (var employee in employees)
            {
                Func<Project, float> myLambda = (Project project) =>
                {
                    switch (project.ProjectCode)
                    {
                        case "LTACAPEX":
                            return 0.6f;
                        case "GLTOPEX":
                            return 0.4f;
                        case "ASDFEX":
                            return 0.0f;
                    }
                    return 0.0f;
                };
                var dictionary = await GetRecords(employee.INumber, 14, myLambda);
                Console.WriteLine($"{employee.FirstName} {employee.MiddleName} {employee.LastName} {employee.Suffix}");
                //Display the records for each employee
                foreach (var codegroup in dictionary)
                {
                    Console.Write($"{codegroup.Key} ");
                    foreach (var record in codegroup.Value)
                    {
                        if (record == null)
                        {
                            Console.Write("{} ");
                        }
                        else
                        {
                            Console.Write($"{record.Hours} ");
                        }

                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            //TODO: Summarize statistics
            Console.WriteLine($"Is equal when null? {new Employee("", "", "", "", "").Equals(null)}");
        }
    }
}

