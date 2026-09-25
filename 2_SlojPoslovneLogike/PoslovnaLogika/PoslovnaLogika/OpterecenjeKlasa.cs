using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using KlasePodataka;

namespace PoslovnaLogika
{
    public class OpterecenjeKlasa
    {

        private string _stringKonekcije;

        public OpterecenjeKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public int DajTrenutnoOpterecenje(string IdVozilaZaProveru)
        // izracunava iz baze podataka koliko trenutno ima paketa dodeljenih datom tipu vozila
        {
            int pomUkupnoPaketa = 0;
            SPPaketDBKlasa SPPaketDBObjekat = new SPPaketDBKlasa(_stringKonekcije);

            // u slucaju da ima praznina u ID vrednosti
            string parametarZaProveru = IdVozilaZaProveru.Replace(" ", "");

            pomUkupnoPaketa = SPPaketDBObjekat.DajUkupnoPaketaZaVozilo(parametarZaProveru);
            return pomUkupnoPaketa;
        }


    }
}
