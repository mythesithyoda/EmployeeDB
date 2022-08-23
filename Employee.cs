namespace EmployeeDB
{
    public class Employee
    {
        public string INumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }


        public Employee(string inumber, string firstName, string middleName, string lastName, string suffix)
        {
            INumber = inumber;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Suffix = suffix;
        }

        //TODO: override is equal
        public override bool Equals(object obj)
        {
            if (obj is Employee employee) return Equals(employee);
            else return false;
        }

        protected bool Equals(Employee other)
        {
            return INumber == other.INumber && FirstName == other.FirstName && MiddleName == other.MiddleName && LastName == other.LastName;
        }

        public override int GetHashCode()
        {
            return INumber.GetHashCode() ^ FirstName.GetHashCode() ^ MiddleName.GetHashCode() ^ LastName.GetHashCode();
        }
    }
}