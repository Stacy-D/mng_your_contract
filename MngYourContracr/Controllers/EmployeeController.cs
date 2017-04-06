using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MngYourContracr.Models;
using MngYourContracr.MngYourContractDatabase;
using MngYourContracr.Service;

namespace MngYourContracr.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private CompanyContext context = new CompanyContext();
        private UserService UserService;
        private EmployeeService employeeService;
        public EmployeeController()
        {
            UserService = new UserService(context);
            employeeService = new EmployeeService(context);
        }
        //
        // GET: /Employee/
        public ActionResult Index()
        {
            Employee employee = employeeService.GetByID(User.Identity.GetUserId());
            ApplicationUser user = UserService.FindUserById(User.Identity.GetUserId());
            employee.User = user;
            return View(employee);
        }

        public ActionResult CurrentTasks()
        {
            return Tasks(false);
        }

        public ActionResult CompletedTasks()
        {
            return Tasks(true);
        }
        public ActionResult Completed(string id)
        {
            var employee = employeeService.GetByID(User.Identity.GetUserId());
            using (var context = new CompanyContext())
            {
                var tasks = from task in context.Tasks
                            where task.EmployeeId == employee.EmployeeId && task.TaskId == id
                            select task;
                foreach (var t in tasks)
                {
                    t.EndDate = DateTime.Now;
                    t.Status = "COMPLETED";
                }
                context.SaveChanges();
            }
            return View();
        }

        private ActionResult Tasks(bool completed)
        {
            var employee = employeeService.GetByID(User.Identity.GetUserId());
            IEnumerable<Task> tasks;
            using (var context = new CompanyContext())
            {
                tasks = from task in context.Tasks
                        where task.EmployeeId == employee.EmployeeId && (completed ? task.Status == "COMPLETED" : task.Status == "OPENED")
                        orderby task.Deadline
                        select task;
                tasks = tasks.ToList();
            }

            return View(tasks);
        }
    }
}