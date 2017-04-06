using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MngYourContracr.MngYourContractDatabase;
namespace MngYourContracr.Service
{
    public class CommonService
    {
        public CommonService(CompanyContext context) {
            this.context = context;
            clientService = new ClientService(context);
            employeeService = new EmployeeService(context);
            managerService = new ManagerService(context);
        }
        private CompanyContext context;
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