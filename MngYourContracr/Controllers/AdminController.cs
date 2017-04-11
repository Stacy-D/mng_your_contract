using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MngYourContracr.Service;
using MngYourContracr.MngYourContractDatabase;

namespace MngYourContracr.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private CompanyContext context = new CompanyContext();
        private UserService UserService;
        private ManagerService managerService;
        private TeamService teamService;
        private TaskService taskService;
        private ProjectService projectService;
        private ClientService clientService;
        private EmployeeService employeeService;

        public AdminController()
        {
            UserService = new UserService(context);
            managerService = new ManagerService(context);
            teamService = new TeamService(context);
            taskService = new TaskService(context);
            projectService = new ProjectService(context);
            clientService = new ClientService(context);
            employeeService = new EmployeeService(context);
             
    }
    // GET: Admin
    public ActionResult ShowAllProjects()
        {
            var projects = projectService.Get();
            return View(projects);
        }
        public ActionResult ShowAllClients()
        {
            var clients = clientService.Get();
            return View(clients);
        }
        public ActionResult ShowAllManagers()
        {
            var managers = managerService.Get();
            return View(managers);
        }
        public ActionResult ShowAllEmployees()
        {
            var clients = employeeService.Get();
            return View(clients);
        }
    }
}