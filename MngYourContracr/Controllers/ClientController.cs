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
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private CompanyContext context = new CompanyContext();
        private UserService UserService;
        private ClientService clientService;

        public ClientController()
        {
            UserService = new UserService(context);
            clientService = new ClientService(context);
        }

        // GET: /Client/
        public ActionResult Index()
        {
            Client client = clientService.GetByID(User.Identity.GetUserId());
            ApplicationUser user = UserService.FindUserById(User.Identity.GetUserId());
            client.User = user;
            return View(client);
        }

        public ActionResult CurrentProjects()
        {
            return Projects(false);
        }

        public ActionResult CompletedProjects()
        {
            return Projects(true);
        }

        public ActionResult Completed(string id)
        {
            var client = clientService.GetByID(User.Identity.GetUserId());
            using (var context = new CompanyContext())
            {
                var projects = from project in context.Projects
                               where project.ClientId == client.ClientId 
                               select project;
                foreach (var t in projects)
                {
                    t.EndDate = DateTime.Today;
                    t.Status = "COMPLETED";
                }
                context.SaveChanges();
            }
            return View();
        }

        private ActionResult Projects(bool completed)
        {
            var client = clientService.GetByID(User.Identity.GetUserId());
            IEnumerable<Project> projects;

            using (var context = new CompanyContext())
            {
                projects = from project in context.Projects
                           where project.ClientId == client.ClientId //add filtering
                           orderby project.Deadline
                           select project;
                projects = projects.ToList();
            }

            return View(projects);
        }
        public ActionResult CreateProject()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items = new List<SelectListItem>();

            var managers = (from m in context.Managers select m).ToList();
            managers.ForEach(m => m.User = UserService.FindUserById(m.ManagerId));

            managers.ForEach(m => items.Add(new SelectListItem { Text = m.User.FirstName + " " + m.User.LastName, Value = m.ManagerId }));
            items.First().Selected = true;

            ViewBag.ManagerId = items;

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
                project.StartDate = DateTime.Today;
                project.income = project.budget;
                project.outgoings = 0;
                project.ClientId = User.Identity.GetUserId();
                context.Projects.Add(project);
                context.SaveChanges();
                return RedirectToAction("CurrentProjects");
            }
            return View(project);
        }
    }
}
