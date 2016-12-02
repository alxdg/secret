using System.Web.Mvc;

namespace SecretSanta.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string Next()
        {
            return "Carlos";
        }

        [HttpPost]
        public string Received(string person)
        {

            return "true";
        }
    }
}