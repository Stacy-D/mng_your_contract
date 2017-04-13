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
            ViewBag.CountProjects = manager.Projects.Count;
            ViewBag.CountOpened = manager.Projects.Where(p => p.Status == "OPENED").Count();
            ViewBag.CountTeams = manager.Teams.Count;
            return View(manager);
        }

        //
        // GET: /Manager/Projects
        public ActionResult Projects()
        {
            Manager manager = managerService.GetByID(User.Identity.GetUserId());
            manager.User = UserService.FindUserById(manager.ManagerId);
            ViewBag.Manager = manager;
            var projects = manager.Projects;
            projects.ForEach(c => c.Client.User = UserService.FindUserById(c.ClientId));
            List<SelectListItem> items = new List<SelectListItem>();
            var teams = (from m in context.Teams select m).ToList();
            teams.ForEach(m => items.Add(new SelectListItem { Text = m.TeamId.ToString(), Value = m.TeamId.ToString() }));
            items.First().Selected = true;
            ViewBag.TeamId = items;
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
            var currProj = manager.Projects.Where(p => p.Status == "OPENED").ToList();
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
            var currProj = manager.Projects.Where(p => p.Status == "RESOLVED").ToList();
            currProj.ForEach(c => c.Client.User = UserService.FindUserById(c.ClientId));
            return View(currProj);
        }

        //
        // GET: /Manager/ProjectTasks
        public ActionResult ProjectTasks([Bind(Include = "projectId")] int projectId)
        {
            var project = projectService.GetByID(projectId);
            ViewBag.ProjectId = projectId;
            ViewBag.status = project.Status;
            var tasks = project.Tasks.ToList();
            tasks.ForEach(c => c.Employee.User = UserService.FindUserById(c.EmployeeId));
            return View(tasks);
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

            var employee = (from e in context.Employees select e).ToList();
            employee.ForEach(e => e.User = UserService.FindUserById(e.EmployeeId));
            MultiSelectList multieEmployeeList = new MultiSelectList(employee);

            ViewBag.Employees = employee;

            return View();
        }

        // POST: /Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTeam([Bind(Include = "TeamId")] Team team, [Bind(Include = "employeeIdList")] List<string> employeeIdList)
        {
            if (ModelState.IsValid)
            {
                team.ManagerId = User.Identity.GetUserId();
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
            return View();
        }

        //
        // POST: /Manager/CreateProject
        [HttpPost]
        public ActionResult CreateProject(Project project)
        {
            if (ModelState.IsValid)
            {
                project.ManagerId = User.Identity.GetUserId();
                project.Status = "OPENED";
                project.StartDate = DateTime.Today;
                project.income = project.budget;
                project.outgoings = 0;
                context.Projects.Add(project);
                context.SaveChanges();
                return RedirectToAction("CurrentProjects");
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
        public ActionResult AcceptProject(int id)
        {
            var project = projectService.GetByID(id);
            List<SelectListItem> items = new List<SelectListItem>();
            var teams = (from m in context.Teams select m).ToList();
            teams.ForEach(m => items.Add(new SelectListItem { Text = m.TeamId.ToString(), Value = m.TeamId.ToString() }));
            items.First().Selected = true;
            ViewBag.TeamId = items;
            ViewBag.Name = project.Name;
            return View(project);
        }
        public ActionResult PayEmployees(int id) {
            var project = projectService.GetByID(id);
            ViewBag.Name = project.Name;
            ViewBag.outgoings = project.outgoings;
            return View(project);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptProject(
    [Bind(Include = "TeamId, ProjectId")]
         Project project)
        {
            this.projectService.assignTeam(project.TeamId.Value, project.ProjectId);
            return RedirectToAction("Projects");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayEmployees(
[Bind(Include = "ProjectId, outgoings")]
         Project project)
        {
            this.projectService.updateBudget(project.outgoings, project.ProjectId);
            return RedirectToAction("Projects");
        }
    }
}