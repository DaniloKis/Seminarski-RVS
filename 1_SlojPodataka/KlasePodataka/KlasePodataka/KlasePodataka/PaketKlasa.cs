using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class PaketKlasa
    {
        // atributi
        private string _kodPaketa;          // string zbog moguce vodece nule u kodu
        private string _posiljalacIme;
        private string _posiljalacAdresa;
        private string _posiljalacGrad;
        private string _primalacIme;
        private string _primalacAdresa;
        private string _primalacPostanskiBroj;
        private string _primalacGrad;
        private decimal _tezina;
        private string _oznakaUpozorenja;
        private decimal _cena;
        private TipVozilaKlasa _tipVozilaObjekat;

        // property
        public string KodPaketa
        {
            get { return _kodPaketa; }
            set { _kodPaketa = value; }
        }

        public string PosiljalacIme
        {
            get { return _posiljalacIme; }
            set { _posiljalacIme = value; }
        }

        public string PosiljalacAdresa
        {
            get { return _posiljalacAdresa; }
            set { _posiljalacAdresa = value; }
        }

        public string PosiljalacGrad
        {
            get { return _posiljalacGrad; }
            set { _posiljalacGrad = value; }
        }

        public string PrimalacIme
        {
            get { return _primalacIme; }
            set { _primalacIme = value; }
        }

        public string PrimalacAdresa
        {
            get { return _primalacAdresa; }
            set { _primalacAdresa = value; }
        }

        public string PrimalacPostanskiBroj
        {
            get { return _primalacPostanskiBroj; }
            set { _primalacPostanskiBroj = value; }
        }

        public string PrimalacGrad
        {
            get { return _primalacGrad; }
            set { _primalacGrad = value; }
        }

        public decimal Tezina
        {
            get { return _tezina; }
            set { _tezina = value; }
        }

        public string OznakaUpozorenja
        {
            get { return _oznakaUpozorenja; }
            set { _oznakaUpozorenja = value; }
        }

        public decimal Cena
        {
            get { return _cena; }
            set { _cena = value; }
        }

        public TipVozilaKlasa TipVozila
        {
            get { return _tipVozilaObjekat; }
            set { _tipVozilaObjekat = value; }
        }
    }
}
