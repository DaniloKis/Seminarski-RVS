using System.Configuration;
using System.Data;
using System.Web.Mvc;
//
using KlasePodataka;
using KorisnickiInterfejs.Filteri;
using KorisnickiInterfejs.ViewModels;

namespace KorisnickiInterfejs.Controllers
{
    // MVC KONTROLER - prijava, odjava i pocetni ekran administratora.
    // Dve uloge: Admin (pun pristup) i Kurir (samo unos paketa).
    public class NalogController : Controller
    {
        private readonly string _stringKonekcije;

        public NalogController()
        {
            _stringKonekcije = ConfigurationManager.ConnectionStrings["NasaKonekcija"].ConnectionString;
        }

        // GET: /Nalog/Prijava
        [HttpGet]
        public ActionResult Prijava()
        {
            return View(new PrijavaViewModel());
        }

        // POST: /Nalog/Prijava
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Prijava(PrijavaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SPKorisnikDBKlasa korisnikDBObjekat = new SPKorisnikDBKlasa(_stringKonekcije);
            DataSet ds = korisnikDBObjekat.DajKorisnikaPoKorisnickomImenuISifri(model.KorisnickoIme, model.Sifra);

            if (ds.Tables[0].Rows.Count == 0)
            {
                ModelState.AddModelError("", "Korisnik nije pronadjen - pogresno korisnicko ime ili sifra.");
                return View(model);
            }

            DataRow red = ds.Tables[0].Rows[0];
            // kolone: ID(0), Prezime(1), Ime(2), KorisnickoIme(3), Sifra(4), Status/uloga(5)
            string imePrezime = red.ItemArray[2].ToString() + " " + red.ItemArray[1].ToString();
            string uloga = red.ItemArray[5].ToString();

            Session["KorisnikImePrezime"] = imePrezime;
            Session["Uloga"] = uloga;

            // Admin -> pocetni ekran administracije; Kurir -> unos paketa
            if (uloga == "Admin")
            {
                return RedirectToAction("DobrodosliAdmin");
            }
            return RedirectToAction("Unos", "PaketMvc");
        }

        // GET: /Nalog/Odjava
        public ActionResult Odjava()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Pocetna");
        }

        // GET: /Nalog/DobrodosliAdmin  (samo administrator)
        [SamoAdmin]
        public ActionResult DobrodosliAdmin()
        {
            ViewBag.ImePrezime = Session["KorisnikImePrezime"] as string;
            return View();
        }
    }
}
