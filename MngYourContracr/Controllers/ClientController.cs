using System.Web.Mvc;

namespace MngYourContracr.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        //
        // GET: /Client/
        public ActionResult Index()
        {
            return View();
        }
    }
}