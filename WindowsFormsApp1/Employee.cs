using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class Employee
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public List<Payment> Payments { get; set; }

        public Employee()
        {
            Payments = new List<Payment>();
        }

        public Employee(string code, string firstName, string lastName, int age)
        {
            Code = code;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Payments = new List<Payment>();
        }
    }
}