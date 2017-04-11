using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MngYourContracr.Service;
using MngYourContracr.MngYourContractDatabase;

namespace MngYourContract.Controllers
{
    public class TeamController : Controller
    {
        private CompanyContext db = new CompanyContext();
        private UserService UserService;
        private TeamService teamService;

        public TeamController()
        {
            UserService = new UserService(db);
            teamService = new TeamService(db);
        }

        // GET: /Team/
        public ActionResult Index()
        {
            return View(db.Teams.ToList());
        }

        // GET: /Team/Details/5
        public ActionResult Details(int id)
        {
            Team team = teamService.GetByID(id);

            if (team == null)
            {
                return HttpNotFound();
            }
            team.Employees.ForEach(e => e.User = UserService.FindUserById(e.EmployeeId));
            team.Manager.User = UserService.FindUserById(team.Manager.ManagerId);
            return View(team);
        }

        // GET: /Team/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Team/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeamId,ManagerId")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(team);
        }

        // GET: /Team/Edit/5
        public ActionResult Edit(int id)
        {
            Team team = teamService.GetByID(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: /Team/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamId,ManagerId")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // GET: /Team/Delete/5
        public ActionResult Delete(int id)
        {
            Team team = teamService.GetByID(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: /Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = teamService.GetByID(id);
            db.Teams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}