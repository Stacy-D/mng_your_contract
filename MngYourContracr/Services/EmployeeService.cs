using System;
using System.Collections.Generic;
using MngYourContracr.MngYourContractDatabase;

namespace MngYourContracr.Service
{
    public class EmployeeService : Service<Employee>
    {
        public UserService userService;
        public EmployeeService(CompanyContext context)
            : base(context)
        {
        }
        public void createEmployee(string user_id)
        {
            var employee = new Employee
            {
                EmployeeId = user_id
            };
            this.Insert(employee);
            this.Save();

        }

        private List<Task> getTasks(String employeeId)
        {
            return GetByID(employeeId).Tasks;
        }

        private List<Task> getTasks(Employee employee)
        {
            return employee.Tasks;
        }
    }
}