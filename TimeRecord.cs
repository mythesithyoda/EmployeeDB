namespace EmployeeDB;

public record struct TimeRecord//(DateTime Date, float Hours, Project Project);
{
    public DateTime Date { get; set; }
    public float Hours { get; set; }
    public Project Project { get; set; }

    public TimeRecord(DateTime date, float hours, Project project)
    {
        Date = date;
        Hours = hours;
        Project = project;
    }
}
