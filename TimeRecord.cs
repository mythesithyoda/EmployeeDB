using System;

namespace EmployeeDB
{
    public class TimeRecord
    {
        public DateTime Date { get; set; }
        public float Hours { get; set; }
        public Project Project { get; set; }
    }
}