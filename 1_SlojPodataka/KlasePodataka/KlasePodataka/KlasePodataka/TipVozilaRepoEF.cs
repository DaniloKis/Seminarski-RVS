using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace KlasePodataka
{
    // EF6 REPOZITORIJUM za sifarnik TipVozila (CRUD preko Entity Framework-a).
    // Analogno klasi TehnologijaRepo (EF) iz reference.
    public class TipVozilaRepoEF
    {
        private readonly KurirskaKontekstEF _kontekst;

        // DI konstruktor (moze se proslediti kontekst)
        public TipVozilaRepoEF(KurirskaKontekstEF kontekst)
        {
            _kontekst = kontekst;
        }

        // podrazumevani konstruktor - sam kreira kontekst
        public TipVozilaRepoEF()
        {
            _kontekst = new KurirskaKontekstEF();
        }

        public List<TipVozilaEntity> DajSve()
        {
            return _kontekst.TipoviVozila
                .OrderBy(tipVozilaObjekat => tipVozilaObjekat.Naziv)
                .ToList();
        }

        public TipVozilaEntity DajPoSifri(string sifra)
        {
            return _kontekst.TipoviVozila.Find(sifra);
        }

        public void Dodaj(TipVozilaEntity tipVozilaObjekat)
        {
            if (tipVozilaObjekat == null) return;

            _kontekst.TipoviVozila.Add(tipVozilaObjekat);
            _kontekst.SaveChanges();
        }

        public void Izmeni(TipVozilaEntity tipVozilaObjekat)
        {
            if (tipVozilaObjekat == null) return;

            // posto je Sifra primarni kljuc, menja se samo Naziv
            TipVozilaEntity postojeci = _kontekst.TipoviVozila.Find(tipVozilaObjekat.Sifra);
            if (postojeci == null) return;

            postojeci.Naziv = tipVozilaObjekat.Naziv;
            _kontekst.SaveChanges();
        }

        public void Obrisi(string sifra)
        {
            TipVozilaEntity tipVozilaObjekat = _kontekst.TipoviVozila.Find(sifra);
            if (tipVozilaObjekat == null) return;

            _kontekst.TipoviVozila.Remove(tipVozilaObjekat);
            _kontekst.SaveChanges();
        }
    }
}
