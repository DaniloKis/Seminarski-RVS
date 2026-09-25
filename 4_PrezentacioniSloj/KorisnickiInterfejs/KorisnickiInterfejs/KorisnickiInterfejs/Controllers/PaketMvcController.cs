using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
//
using KlasePodataka;
using PoslovnaLogika;
using KorisnickiInterfejs.Filteri;
using KorisnickiInterfejs.ViewModels;

namespace KorisnickiInterfejs.Controllers
{
    // MVC KONTROLER (ASP.NET MVC 5) ZA PAKETE.
    // Pun CRUD + master-detail + stampa. Uloge: kurir sme samo Unos,
    // administrator ima pun pristup (filteri ZahtevaPrijavu / SamoAdmin).
    public class PaketMvcController : Controller
    {
        // ZAVISNOST PREKO INTERFEJSA (Dependency Injection).
        // Konkretnu implementaciju ubacuje DI resolver (KurirskiResolver).
        private readonly IPaketRepo _paketRepo;

        private readonly string _stringKonekcije;
        private readonly string _restBaseUrl;
        private readonly string _restToken;

        public PaketMvcController(IPaketRepo paketRepo)
        {
            _paketRepo = paketRepo;
            _stringKonekcije = ConfigurationManager.ConnectionStrings["NasaKonekcija"].ConnectionString;
            _restBaseUrl = ConfigurationManager.AppSettings["RestOgranicenjaUrl"];
            _restToken = ConfigurationManager.AppSettings["RestApiToken"];
        }

        // pomocna: primena poslovnog pravila (tezina -> tip vozila) preko REST servisa
        private string DodeliVozilo(decimal tezina)
        {
            DodeljivanjeVozilaRestKlasa dodeljivanjeObjekat =
                new DodeljivanjeVozilaRestKlasa(_stringKonekcije, _restBaseUrl, _restToken);
            return dodeljivanjeObjekat.DajTipVozilaZaTezinu(tezina);
        }

        // GET: /PaketMvc  -> tabelarni prikaz svih paketa (master), sa filterom po kodu
        [SamoAdmin]
        public ActionResult Index(string filter)
        {
            List<PaketKlasa> listaPaketa = _paketRepo.DajSvePaketeLista();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                listaPaketa = listaPaketa
                    .Where(pk => pk.KodPaketa != null &&
                                 pk.KodPaketa.IndexOf(filter, System.StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
            ViewBag.Filter = filter;
            return View(listaPaketa);
        }

        // GET: /PaketMvc/Unos  -> master-detail forma (kurir i admin)
        [ZahtevaPrijavu]
        [HttpGet]
        public ActionResult Unos()
        {
            PaketUnosViewModel model = new PaketUnosViewModel();
            return View(model);
        }

        // POST: /PaketMvc/Unos  -> snimanje master + detalji (transakcija)
        [ZahtevaPrijavu]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unos(PaketUnosViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. POSLOVNO PRAVILO - dodela tipa vozila prema tezini (preko REST servisa)
            string nazivVozila = DodeliVozilo(model.Tezina);

            // 2. mapiranje naziva vozila -> sifra iz sifarnika
            SPTipVozilaDBKlasa tipVozilaDBObjekat = new SPTipVozilaDBKlasa(_stringKonekcije);
            string sifraVozila = tipVozilaDBObjekat.DajSifruVozilaPoNazivu(nazivVozila);

            // 3. priprema master objekta (Paket)
            TipVozilaKlasa tipVozilaObjekat = new TipVozilaKlasa();
            tipVozilaObjekat.Naziv = nazivVozila;
            tipVozilaObjekat.Sifra = sifraVozila;

            PaketKlasa paketObjekat = new PaketKlasa();
            paketObjekat.KodPaketa = model.KodPaketa;
            paketObjekat.PosiljalacIme = model.PosiljalacIme;
            paketObjekat.PosiljalacAdresa = model.PosiljalacAdresa;
            paketObjekat.PosiljalacGrad = model.PosiljalacGrad;
            paketObjekat.PrimalacIme = model.PrimalacIme;
            paketObjekat.PrimalacAdresa = model.PrimalacAdresa;
            paketObjekat.PrimalacPostanskiBroj = model.PrimalacPostanskiBroj;
            paketObjekat.PrimalacGrad = model.PrimalacGrad;
            paketObjekat.Tezina = model.Tezina;
            paketObjekat.OznakaUpozorenja = model.OznakaUpozorenja;
            paketObjekat.Cena = model.Cena;
            paketObjekat.TipVozila = tipVozilaObjekat;

            // 4. priprema liste detalja (stavke)
            List<StavkaPaketaKlasa> listaStavki = new List<StavkaPaketaKlasa>();
            if (model.Stavke != null)
            {
                foreach (StavkaUnosViewModel stavkaVM in model.Stavke)
                {
                    if (stavkaVM == null || string.IsNullOrWhiteSpace(stavkaVM.NazivArtikla))
                    {
                        continue;
                    }
                    StavkaPaketaKlasa stavkaObjekat = new StavkaPaketaKlasa();
                    stavkaObjekat.NazivArtikla = stavkaVM.NazivArtikla;
                    stavkaObjekat.Kolicina = stavkaVM.Kolicina;
                    stavkaObjekat.TezinaStavke = stavkaVM.TezinaStavke;
                    listaStavki.Add(stavkaObjekat);
                }
            }

            // 5. TRANSAKCIONI UPIS master + svi detalji
            StavkaPaketaDBKlasa masterDetailDBObjekat = new StavkaPaketaDBKlasa(_stringKonekcije);
            bool uspeh = masterDetailDBObjekat.SnimiPaketSaStavkama(paketObjekat, listaStavki);

            if (uspeh)
            {
                TempData["Poruka"] = "Paket je uspesno snimljen (dodeljeno vozilo: " + nazivVozila + ").";
                TempData["PorukaTip"] = "uspeh";
                // kurir nema pristup listi -> vraca se na unos; admin ide na listu
                string uloga = Session["Uloga"] as string;
                if (uloga == "Admin")
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Unos");
            }

            TempData["Poruka"] = "Greska pri snimanju paketa - transakcija je ponistena.";
            TempData["PorukaTip"] = "greska";
            model.DodeljeniTipVozila = nazivVozila;
            return View(model);
        }

        // GET: /PaketMvc/MasterDetail?kod=...  -> prikaz paketa + stavki
        [SamoAdmin]
        [HttpGet]
        public ActionResult MasterDetail(string kod)
        {
            if (string.IsNullOrWhiteSpace(kod))
            {
                return RedirectToAction("Index");
            }

            // citanje ide kroz ubacenu zavisnost (interfejs IPaketRepo), bez SQL-a u kontroleru
            PaketKlasa paketObjekat = _paketRepo.DajPaketPoKoduObjekat(kod);
            if (paketObjekat == null)
            {
                return HttpNotFound();
            }

            StavkaPaketaDBKlasa masterDetailDBObjekat = new StavkaPaketaDBKlasa(_stringKonekcije);

            PaketMasterDetailViewModel model = new PaketMasterDetailViewModel();
            model.KodPaketa = paketObjekat.KodPaketa;
            model.PosiljalacIme = paketObjekat.PosiljalacIme;
            model.PrimalacIme = paketObjekat.PrimalacIme;
            model.PrimalacGrad = paketObjekat.PrimalacGrad;
            model.Tezina = paketObjekat.Tezina;
            model.Cena = paketObjekat.Cena;
            model.NazivVozila = paketObjekat.TipVozila != null ? paketObjekat.TipVozila.Naziv : "";
            model.Stavke = masterDetailDBObjekat.DajStavkePaketa(kod);

            return View(model);
        }

        // GET: /PaketMvc/Izmeni?kod=...  -> forma za izmenu paketa
        [SamoAdmin]
        [HttpGet]
        public ActionResult Izmeni(string kod)
        {
            if (string.IsNullOrWhiteSpace(kod))
            {
                return RedirectToAction("Index");
            }

            PaketKlasa paketObjekat = _paketRepo.DajPaketPoKoduObjekat(kod);
            if (paketObjekat == null)
            {
                return HttpNotFound();
            }

            PaketIzmenaViewModel model = new PaketIzmenaViewModel();
            model.KodPaketa = paketObjekat.KodPaketa;
            model.PosiljalacIme = paketObjekat.PosiljalacIme;
            model.PosiljalacAdresa = paketObjekat.PosiljalacAdresa;
            model.PosiljalacGrad = paketObjekat.PosiljalacGrad;
            model.PrimalacIme = paketObjekat.PrimalacIme;
            model.PrimalacAdresa = paketObjekat.PrimalacAdresa;
            model.PrimalacPostanskiBroj = paketObjekat.PrimalacPostanskiBroj;
            model.PrimalacGrad = paketObjekat.PrimalacGrad;
            model.Tezina = paketObjekat.Tezina;
            model.OznakaUpozorenja = paketObjekat.OznakaUpozorenja;
            model.Cena = paketObjekat.Cena;
            model.DodeljeniTipVozila = paketObjekat.TipVozila != null ? paketObjekat.TipVozila.Naziv : "";
            return View(model);
        }

        // POST: /PaketMvc/Izmeni  -> cuvanje izmena (ponovo se primenjuje poslovno pravilo)
        [SamoAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Izmeni(PaketIzmenaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // POSLOVNO PRAVILO se ponovo primenjuje na (moguce izmenjenu) tezinu
            string nazivVozila = DodeliVozilo(model.Tezina);
            SPTipVozilaDBKlasa tipVozilaDBObjekat = new SPTipVozilaDBKlasa(_stringKonekcije);
            string sifraVozila = tipVozilaDBObjekat.DajSifruVozilaPoNazivu(nazivVozila);

            TipVozilaKlasa tipVozilaObjekat = new TipVozilaKlasa();
            tipVozilaObjekat.Naziv = nazivVozila;
            tipVozilaObjekat.Sifra = sifraVozila;

            // stari (kljuc) i novi paket
            PaketKlasa stariPaketObjekat = new PaketKlasa();
            stariPaketObjekat.KodPaketa = model.KodPaketa;

            PaketKlasa noviPaketObjekat = new PaketKlasa();
            noviPaketObjekat.KodPaketa = model.KodPaketa;
            noviPaketObjekat.PosiljalacIme = model.PosiljalacIme;
            noviPaketObjekat.PosiljalacAdresa = model.PosiljalacAdresa;
            noviPaketObjekat.PosiljalacGrad = model.PosiljalacGrad;
            noviPaketObjekat.PrimalacIme = model.PrimalacIme;
            noviPaketObjekat.PrimalacAdresa = model.PrimalacAdresa;
            noviPaketObjekat.PrimalacPostanskiBroj = model.PrimalacPostanskiBroj;
            noviPaketObjekat.PrimalacGrad = model.PrimalacGrad;
            noviPaketObjekat.Tezina = model.Tezina;
            noviPaketObjekat.OznakaUpozorenja = model.OznakaUpozorenja;
            noviPaketObjekat.Cena = model.Cena;
            noviPaketObjekat.TipVozila = tipVozilaObjekat;

            SPPaketDBKlasa paketDBObjekat = new SPPaketDBKlasa(_stringKonekcije);
            bool uspeh = paketDBObjekat.IzmeniPaket(stariPaketObjekat, noviPaketObjekat);

            if (uspeh)
            {
                TempData["Poruka"] = "Paket je uspesno izmenjen (dodeljeno vozilo: " + nazivVozila + ").";
                TempData["PorukaTip"] = "uspeh";
                return RedirectToAction("Index");
            }

            TempData["Poruka"] = "Greska pri izmeni paketa.";
            TempData["PorukaTip"] = "greska";
            model.DodeljeniTipVozila = nazivVozila;
            return View(model);
        }

        // POST: /PaketMvc/Obrisi  -> brisanje paketa
        [SamoAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Obrisi(string kod)
        {
            SPPaketDBKlasa paketDBObjekat = new SPPaketDBKlasa(_stringKonekcije);
            bool uspeh = paketDBObjekat.ObrisiPaket(kod);

            TempData["Poruka"] = uspeh ? "Paket je obrisan." : "Greska pri brisanju paketa.";
            TempData["PorukaTip"] = uspeh ? "uspeh" : "greska";
            return RedirectToAction("Index");
        }

        // GET: /PaketMvc/Stampa?filter=...  -> prikaz za stampu (bez glavnog menija)
        [SamoAdmin]
        public ActionResult Stampa(string filter)
        {
            List<PaketKlasa> listaPaketa = _paketRepo.DajSvePaketeLista();
            if (!string.IsNullOrWhiteSpace(filter) && filter != "0")
            {
                listaPaketa = listaPaketa
                    .Where(pk => pk.KodPaketa != null &&
                                 pk.KodPaketa.IndexOf(filter, System.StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
                ViewBag.Naslov = "FILTRIRANI SPISAK PAKETA, kod=" + filter;
            }
            else
            {
                ViewBag.Naslov = "SPISAK SVIH PAKETA";
            }
            return View(listaPaketa);
        }
    }
}
