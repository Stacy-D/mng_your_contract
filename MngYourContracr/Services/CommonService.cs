using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MngYourContracr.Service
{
    public class CommonService
    {
        ClientService clientService;
        EmployeeService employeeService;
        ManagerService managerService;

        public void connectUserAndPosition(string role, string userId) {
            switch (role) {
                case "Manager":
                    managerService.createManager(userId);
                    break;
                case "Employee":
                    employeeService.createEmployee(userId);
                    break;
                default:
                    clientService.createClient(userId);
                    break;
            }
        }

    }
}