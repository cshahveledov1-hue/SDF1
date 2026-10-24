using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp1
{
    public class EmployeeService : EmployeeServiceBase
    {
        public static List<Employee> Employees = new List<Employee>();

        public override bool AddEmployee(Employee employee)
        {
            if (Employees.Any(e => e.Code == employee.Code))
            {
                return false;
            }

            Employees.Add(employee);
            return true;
        }

        public override Employee FindEmployee(string code)
        {
            return Employees.FirstOrDefault(e => e.Code == code);
        }

        public override bool AddPayment(string employeeCode, string title, double amount)
        {
            Employee emp = FindEmployee(employeeCode);

            if (emp == null || amount <= 0)
            {
                return false;
            }

            emp.Payments.Add(new Payment(title, amount));
            return true;
        }

        public override List<Payment> GetEmployeePayments(string employeeCode)
        {
            Employee emp = FindEmployee(employeeCode);
            if (emp == null)
            {
                return null;
            }

            return emp.Payments;
        }

        public override List<Employee> GetAllEmployees()
        {
            return Employees;
        }
    }
}