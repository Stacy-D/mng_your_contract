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
        
    }
}