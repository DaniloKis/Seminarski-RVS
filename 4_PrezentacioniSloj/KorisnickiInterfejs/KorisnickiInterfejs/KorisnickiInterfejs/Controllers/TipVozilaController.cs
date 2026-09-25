using System.Collections.Generic;
using System.Web.Mvc;
//
using KlasePodataka;
using KorisnickiInterfejs.Filteri;
using KorisnickiInterfejs.ViewModels;

namespace KorisnickiInterfejs.Controllers
{
    // MVC KONTROLER (ASP.NET MVC 5) ZA SIFARNIK TIP VOZILA - preko ENTITY FRAMEWORK-a (EF6).
    // CRUD operacije idu kroz TipVozilaRepoEF (DbContext). Sifarnik menja samo administrator.
    [SamoAdmin]
    public class TipVozilaController : Controller
    {
        private readonly TipVozilaRepoEF _repoEF;

        public TipVozilaController()
        {
            _repoEF = new TipVozilaRepoEF();
        }

        // GET: /TipVozila
        public ActionResult Index()
        {
            List<TipVozilaEntity> lista = _repoEF.DajSve();
            return View(lista);
        }

        // GET: /TipVozila/Dodaj
        [HttpGet]
        public ActionResult Dodaj()
        {
            return View(new TipVozilaViewModel());
        }

        // POST: /TipVozila/Dodaj
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodaj(TipVozilaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // mapiranje VM -> EF entitet
            TipVozilaEntity entitet = new TipVozilaEntity();
            entitet.Sifra = model.Sifra;
            entitet.Naziv = model.Naziv;

            _repoEF.Dodaj(entitet);

            TempData["Poruka"] = "Tip vozila je uspesno dodat.";
            TempData["PorukaTip"] = "uspeh";
            return RedirectToAction("Index");
        }

        // GET: /TipVozila/Izmeni?sifra=...
        [HttpGet]
        public ActionResult Izmeni(string sifra)
        {
            TipVozilaEntity entitet = _repoEF.DajPoSifri(sifra);
            if (entitet == null)
            {
                return HttpNotFound();
            }

            TipVozilaViewModel model = new TipVozilaViewModel();
            model.Sifra = entitet.Sifra;
            model.Naziv = entitet.Naziv;
            return View(model);
        }

        // POST: /TipVozila/Izmeni
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Izmeni(TipVozilaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TipVozilaEntity entitet = new TipVozilaEntity();
            entitet.Sifra = model.Sifra;
            entitet.Naziv = model.Naziv;

            _repoEF.Izmeni(entitet);

            TempData["Poruka"] = "Tip vozila je uspesno izmenjen.";
            TempData["PorukaTip"] = "uspeh";
            return RedirectToAction("Index");
        }

        // POST: /TipVozila/Obrisi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Obrisi(string sifra)
        {
            _repoEF.Obrisi(sifra);

            TempData["Poruka"] = "Tip vozila je uspesno obrisan.";
            TempData["PorukaTip"] = "uspeh";
            return RedirectToAction("Index");
        }
    }
}
