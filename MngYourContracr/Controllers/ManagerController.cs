using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MngYourContracr.Service;
using MngYourContracr.MngYourContractDatabase;
using Microsoft.AspNet.Identity;
using MngYourContracr.Models;
namespace MngYourContracr.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private CompanyContext context = new CompanyContext();
        private UserService UserService;
        private ManagerService managerService;
        private TeamService teamService;
        private TaskService taskService;
        private ProjectService projectService;

        public ManagerController()
        {
            UserService = new UserService(context);
            managerService = new ManagerService(context);
            teamService = new TeamService(context);
            taskService = new TaskService(context);
            projectService = new ProjectService(context);
        }

        //
        // GET: /Manager/
        public ActionResult Index()
        { 
            Manager manager = managerService.GetByID(User.Identity.GetUserId());
            ApplicationUser user = UserService.FindUserById(User.Identity.GetUserId());
            manager.User = user;
            return View(manager);
        }

        //
        // GET: /Manager/Projects
        public ActionResult Projects()
        {
            Manager manager = managerService.GetByID(User.Identity.GetUserId());
            manager.User = UserService.FindUserById(manager.ManagerId);
            ViewBag.Manager = manager;
            var projects = context.Projects.ToList();
            projects.ForEach(c => c.Client.User = UserService.FindUserById(c.ClientId));
            return View(projects);
        }

        //
        // GET: /Manager/CurrentProjects
        public ActionResult CurrentProjects()
        {
            Manager manager = managerService.GetByID(User.Identity.GetUserId());
            manager.User = UserService.FindUserById(manager.ManagerId);
            ViewBag.Manager = manager;
            //TODO: change this! Current projects are not working correctly
            var currProj = (from cp in this.context.Projects where cp.ManagerId == manager.ManagerId && Nullable.Compare(cp.Deadline, cp.EndDate) <= 0 select cp).ToList();
            currProj.ForEach(c => c.Client.User = UserService.FindUserById(c.ClientId));
            return View(currProj);
        }

        //
        // GET: /Manager/CompletedProjects
        public ActionResult CompletedProjects()
        {
            Manager manager = managerService.GetByID(User.Identity.GetUserId());
            manager.User = UserService.FindUserById(manager.ManagerId);
            ViewBag.Manager = manager;
            // todo CHANGE THIS TOO. Show Completed projects, which have status Completed/resolved
            var currProj = (from cp in this.context.Projects where cp.ManagerId == manager.ManagerId && Nullable.Compare(cp.Deadline, cp.EndDate) > 0 select cp).ToList();
            currProj.ForEach(c => c.Client.User = UserService.FindUserById(c.ClientId));
            return View(currProj);
        }

        //
        // GET: /Manager/ProjectTasks
        public ActionResult ProjectTasks([Bind(Include = "projectId")] int projectId)
        {
            var project = projectService.GetByID(projectId);
            ViewBag.ProjectId = projectId;
            return View(project.Tasks);
        }

        // GET: /Task/Create
        public ActionResult CreateTask([Bind(Include = "projectId")] int projectId)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var employees = (from e in context.Employees select e).ToList();
            employees.ForEach(e => e.User = UserService.FindUserById(e.EmployeeId));

            employees.ForEach(m => items.Add(new SelectListItem { Text = m.User.FirstName + " " + m.User.LastName, Value = m.EmployeeId }));
            items.First().Selected = true;

            ViewBag.EmployeeId = items;
            ViewBag.ProjectId = projectId;

            return View();
        }

        // POST: /Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTask([Bind(Include = "TaskId,Name,Description,Status,Deadline,StartDate,EndDate,EmployeeId")] Task task,
            [Bind(Include = "projectId")] int projectId)
        {
            if (ModelState.IsValid)
            {
                // DO NOT TOUCH THIS!
                var project = projectService.GetByID(projectId);
                task.Status = "OPENED";
                task.StartDate = DateTime.Today;
                project.Tasks.Add(task);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(task);
        }

        //
        // GET: /Manager/Teams
        public ActionResult Teams()
        {
            Manager manager = managerService.GetByID(User.Identity.GetUserId());
            var tq = from team in this.context.Teams where team.ManagerId == manager.ManagerId select team;

            manager.User = UserService.FindUserById(manager.ManagerId);
            ViewBag.Manager = manager;
            return View(tq.ToList());
        }

        // create team
        // GET: /Manager/CreateTeam
        public ActionResult CreateTeam()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var managers = (from m in context.Managers select m).ToList();
            managers.ForEach(m => m.User = UserService.FindUserById(m.ManagerId));

            managers.ForEach(m => items.Add(new SelectListItem { Text = m.User.FirstName + " " + m.User.LastName, Value = m.ManagerId }));
            items.First().Selected = true;

            var employee = (from e in context.Employees select e).ToList();
            employee.ForEach(e => e.User = UserService.FindUserById(e.EmployeeId));
            MultiSelectList multieEmployeeList = new MultiSelectList(employee);

            ViewBag.ManagerId = items;
            ViewBag.Employees = employee;

            return View();
        }

        // POST: /Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTeam([Bind(Include = "TeamId,ManagerId")] Team team, [Bind(Include = "employeeIdList")] List<string> employeeIdList)
        {
            if (ModelState.IsValid)
            {
                team.Employees = new List<Employee>();
                employeeIdList.ForEach(e => team.Employees.Add(context.Employees.Find(e)));
                context.Teams.Add(team);
                context.SaveChanges();
                return RedirectToAction("Teams");
            }

            return View(team);
        }

        // delete team

        // GET: /Team/Delete/5
        public ActionResult DeleteTeam(int id)
        {
            Team team = teamService.GetByID(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: /Team/Delete/5
        [HttpPost, ActionName("DeleteTeam")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = teamService.GetByID(id);
            try
            {
                team.Employees = null;
                context.Entry(team).State = EntityState.Modified;
                context.SaveChanges();
                context.Teams.Remove(team);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Teams");
            }
        }

        // delete team

        //
        // GET: /Manager/CreateProject
        public ActionResult CreateProject()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var clients = (from e in context.Clients select e).ToList();
            clients.ForEach(e => e.User = UserService.FindUserById(e.ClientId));
            clients.ForEach(m => items.Add(new SelectListItem { Text = m.User.FirstName + " " + m.User.LastName, Value = m.ClientId }));
            items.First().Selected = true;
            ViewBag.ClientId = items;
            items = new List<SelectListItem>();
            var managers = (from m in context.Managers select m).ToList();
            managers.ForEach(m => m.User = UserService.FindUserById(m.ManagerId));
            managers.ForEach(m => items.Add(new SelectListItem { Text = m.User.FirstName + " " + m.User.LastName, Value = m.ManagerId }));
            items.First().Selected = true;
            ViewBag.ManagerId = items;
            items = new List<SelectListItem>();
            var teams = (from m in context.Teams select m).ToList();
            teams.ForEach(m => items.Add(new SelectListItem { Text = m.TeamId.ToString(), Value = m.TeamId.ToString() }));
            items.First().Selected = true;
            ViewBag.TeamId = items;
            return View();
        }

        //
        // POST: /Manager/CreateProject
        [HttpPost]
        public ActionResult CreateProject(Project project)
        {
            if (ModelState.IsValid)
            {
                project.Status = "OPENED";
                project.StartDate =  DateTime.Today;
                projectService.Insert(project);
                return RedirectToAction("Projects");
            }
            return View(project);
        }

        //Close Project
        public ActionResult CloseProject([Bind(Include = "projectId")]int projectId)
        {
            if (ModelState.IsValid)
            {
                var project = projectService.GetByID(projectId);
                project.Status = "RESOLVED";
                project.EndDate = DateTime.Today;
                context.Entry(project).State = EntityState.Modified;
                context.SaveChanges();
            }
            return RedirectToAction("Projects");
        }

        public ActionResult CloseProjectTask([Bind(Include = "taskId")] int taskId)
        {
            if (ModelState.IsValid)
            {
                var task = taskService.GetByID(taskId);
                var projectId = task.Project.ProjectId;
                task.Status = "Closed";
                task.EndDate = DateTime.Today;
                context.Entry(task).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("ProjectTasks?projectId=" + projectId);
            }
            return RedirectToAction("Projects");
           
        }
}
}