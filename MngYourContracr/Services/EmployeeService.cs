using System;
using System.Collections.Generic;
using MngYourContracr.MngYourContractDatabase;

namespace MngYourContracr.Service
{
    public class EmployeeService : Service<Employee>
    {
        public EmployeeService(CompanyContext context)
            : base(context)
        {
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