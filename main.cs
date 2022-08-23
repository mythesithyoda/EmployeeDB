/*
x 1. Async Enumerable for data get methods
x 2. Switch Expression in lambda
x 3. Inferred type for lambda
x 4. Null safety every where(NotNullAttribute, ull coalesce/assignment, object notation)
x 5. Record types
x 6. Pattern matching/deconstruction when possible
x 7. file declared namespace
x 8. global using
x 9. top level expressions
10. init only properties
x 11. lambda attributes
12. 
*/


//Get All Employees
await foreach (var employee in GetEmployees())
{
    var myLambda = [EmployeeDB.My] (EmployeeDB.Project project) =>
        project switch
        {
            (ProjectCode: "LTACAPEX") => 0.6f,
            (ProjectCode: "GLTOPEX") => 0.4f,
            (ProjectCode: "ASDFEX") => 0.0f,
            (_) => 0.0f,
        };
    
    Console.WriteLine($"{employee.FirstName} {employee.MiddleName} {employee.LastName} {employee.Suffix}");
    //Display the records for each employee
    await foreach (var (code, recordList) in GetRecords(employee.INumber, 14, myLambda))
    {
        Console.Write($"{code.ProjectCode} ");
        foreach (var record in recordList)
        {
            if (record is {Hours: var h})
            {
              Console.Write($"{h} ");
            }
            else
            {
                Console.Write("{} ");
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

static async IAsyncEnumerable<Employee> GetEmployees()
{
    for (int i = 0; i < 5; ++i)
    {
        //Pretend this is a network call, or DB, or something long running
        await Task.Delay(10);
        yield return new Employee("i12345", "", "David", "Farrington", i.ToString());
    }
}

static async IAsyncEnumerable<(Project, List<TimeRecord?>)> GetRecords(string inumber, int pastDays, Func<Project, float> getProjectCodeSplit)
{
    var codes = new List<Project> {
                new (ProjectCode: "LTACAPEX"),
                new (ProjectCode: "GLTOPEX"),
                new (ProjectCode: "ASDFEX"),
            };

    foreach (var code in codes)
    {
        List <TimeRecord?> records = new();
        for (int i = 0; i < pastDays; ++i)
        {
            var date = DateTime.Now.Subtract(TimeSpan.FromDays(i));

            float percent = getProjectCodeSplit(code);
            
            //Simulate some unentered time
            if (i > 10)
            {
                records.Add(null);
                continue;
            }
            //Pretend this is a network call, or DB, or something long running
            records.Add(new TimeRecord(date, 8 * percent, code));
            await Task.Delay(10);
        }     
      yield return (code, records);
    }
}
