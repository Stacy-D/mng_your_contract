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
    public class ProjectController : Controller
    {
        private CompanyContext context = new CompanyContext();
        private UserService UserService;
        private ClientService clientService;
        public ProjectController()
        {
            UserService = new UserService(context);
            clientService = new ClientService(context);
        }
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create() {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Project project) {
            return RedirectToAction("");
        }
        /*
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

            teams.ForEach(m => items.Add(new SelectListItem { Text = m.TeamId, Value = m.TeamId }));
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
                context.Projects.Add(project);
                context.SaveChanges();
                return RedirectToAction("Projects");
            }
            return View(project);
        }*/
    }
}