/*
null safety - Art
x 4. Null safety every where(NotNullAttribute, null coalesce/assignment, object notation)

extra stuff - Krista
x 7. file declared namespace
x 8. global using
x 9. top level expressions

records - Dan Burton
x 5. Record types
x 10. init only properties
x 12. with statements

async - Ben
x 1. Async Enumerable for data get methods

lambda - David F.
x 2. Switch Expression in lambda
x 3. Inferred type for lambda
x 6. Pattern matching/deconstruction when possible
x 11. lambda attributes 
*/

//Get All Employees
await foreach (var employee in GetEmployees())
{
    if (employee is null)
    {
        continue;
    }
    var myLambda =[EmployeeDB.My] (EmployeeDB.Project project) =>
       project switch
       {
           (ProjectCode: "LTACAPEX") => 0.6f,
           (ProjectCode: "GLTOPEX") => 0.4f,
           (ProjectCode: "ASDFEX") => 0.0f,
           (_) => 0.0f,
       };

    Console.WriteLine($"{employee.FirstName} {employee.MiddleName ?? "<none>"} {employee.LastName} {employee.Suffix ?? "<none>"}");
    //Display the records for each employee
    await foreach (var (code, recordList) in GetRecords(employee.INumber, 14, myLambda))
    {
        Console.Write($"{code.ProjectCode} ");
        await foreach (var record in recordList)
        {
            if (record is { Hours: var h })
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

static async IAsyncEnumerable<(Project, IAsyncEnumerable<TimeRecord?>)> GetRecords(string inumber, int pastDays, Func<Project, float> getProjectCodeSplit)
{
    var projects = new List<Project> {
                new (ProjectCode: "LTACAPEX"),
                new (ProjectCode: "GLTOPEX"),
                new (ProjectCode: "ASDFEX"),
            };
    var startingRecord = new TimeRecord
        {
            Date = DateTime.Now,
            Hours = 8,
        };
    foreach (var project in projects)
    {
        //Pretend this is a network call, or DB, or something long running
        await Task.Delay(10);
        var records = GetProjectRecords(startingRecord with { Project = project }, inumber, pastDays, getProjectCodeSplit);
        yield return (project, records);
    }
}

static async IAsyncEnumerable<TimeRecord?> GetProjectRecords(EmployeeDB.TimeRecord record, string inumber, int pastDays, Func<Project, float> getProjectCodeSplit)
{
    for (int i = 0; i < pastDays; ++i)
    {
        var date = record.Date.Subtract(TimeSpan.FromDays(i));

        float percent = getProjectCodeSplit(record.Project);

        //Simulate some unentered time
        if (i > 10)
        {
            yield return null;
            continue;
        }
        //Pretend this is a network call, or DB, or something long running
        await Task.Delay(10);
        yield return record with { Date = date, Hours = record.Hours * percent };
    }
}
