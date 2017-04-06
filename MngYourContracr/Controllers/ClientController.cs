using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MngYourContracr.Models;
using MngYourContracr.MngYourContractDatabase;

namespace MngYourContracr.Controllers
{
    //TODO uncomment it
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        //
        // GET: /Client/
        public ActionResult Index()
        {
            ViewBag.User = LoggedInUser();
            ViewBag.Client = GetClient(ViewBag.User);
            return View();
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
            var user = LoggedInUser();
            using (var context = new CompanyContext())
            {
                var projects = from project in context.Projects
                            where project.ClientId == user.Id && project.ProjectId == id
                            select project;
                foreach (var t in projects)
                {
                    t.EndDate = DateTime.Now;
                    t.Status = "COMPLETED";
                }
                context.SaveChanges();
            }
            return View();
        }

        private ActionResult Projects(bool completed)
        {
            var client = LoggedInUser();
            IEnumerable<Project> projects;

            using (var context = new CompanyContext())
            {
                projects = from project in context.Projects
                        where project.ClientId == client.Id && (completed ? project.Status == "COMPLETED" : project.Status == "OPENED")
                        orderby project.Deadline
                        select project;
                projects = projects.ToList();
            }

            return View(projects);
        }


        private ApplicationUser LoggedInUser()
        {
            var userId = User.Identity.GetUserId();
            using (var context = new CompanyContext())
            {
                var users = from u in context.Users
                            where u.Id == userId
                            select u;

                foreach (var u in users)
                {
                    return u;
                }
            }

            return null;
        }
        

        private Client GetClient(ApplicationUser user)
        {
            using (var context = new CompanyContext())
            {
                var clients = from client in context.Clients
                                where client.ClientId == user.Id
                                select client;

                foreach (var e in clients)
                {
                    return e;
                }
            }

            return null;
        }

    }

}