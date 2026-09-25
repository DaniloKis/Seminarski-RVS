using System.Web.Mvc;

namespace KorisnickiInterfejs.Controllers
{
    // MVC KONTROLER - pocetna strana i strana o autoru (javno dostupno).
    public class PocetnaController : Controller
    {
        // GET: /  ili  /Pocetna
        public ActionResult Index()
        {
            // status konekcije ka bazi (otvorena u Global.asax na startu aplikacije)
            ViewBag.StatusKonekcije = Global.uspehKonekcije
                ? "USPESNO REALIZOVANA KONEKCIJA!"
                : "NEUSPESNA KONEKCIJA!";
            return View();
        }

        // GET: /Pocetna/Autor
        public ActionResult Autor()
        {
            return View();
        }
    }
}
