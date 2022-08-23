namespace EmployeeDB
{
  public class Employee
  {
    public string INumber {get; set;}
    public string FirstName {get; set;}
    public string MiddleName {get; set;}
    public string LastName {get; set;}
    public string Suffix {get; set;}

    
    public Employee(string inumber, string firstName, string middleName, string lastName, string suffix)
    {
      INumber = inumber;
      FirstName = firstName;
      MiddleName = middleName;
      LastName = lastName;
      Suffix = suffix;
    }

    //TODO: override is equal
    //TODO: override gethash
    
    
  }
}