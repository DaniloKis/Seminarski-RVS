using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaTipVozilaUnosKlasa
    {
        // atributi
        private string _stringKonekcije;
        private string _sifra;
        private string _naziv;

        // property
        public string Sifra
        {
            get { return _sifra; }
            set { _sifra = value; }
        }

        public string Naziv
        {
            get { return _naziv; }
            set { _naziv = value; }
        }

        public FormaTipVozilaUnosKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public bool SnimiPodatke()
        {
            bool uspehSnimanja = false;

            SPTipVozilaDBKlasa TipVozilaDBObjekat = new SPTipVozilaDBKlasa(this._stringKonekcije);
            TipVozilaKlasa TipVozilaObjekat = new TipVozilaKlasa();
            TipVozilaObjekat.Sifra = this._sifra;
            TipVozilaObjekat.Naziv = this._naziv;
            uspehSnimanja = TipVozilaDBObjekat.SnimiNovoVozilo(TipVozilaObjekat);

            return uspehSnimanja;

        }




    }
}
