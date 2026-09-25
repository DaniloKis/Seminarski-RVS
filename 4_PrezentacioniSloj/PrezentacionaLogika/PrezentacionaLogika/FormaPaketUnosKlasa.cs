using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
//
using System.Data;
using KlasePodataka;
using PoslovnaLogika;

namespace PrezentacionaLogika
{
    public class FormaPaketUnosKlasa
    {
        // atributi
        private string _stringKonekcije;
        private string _kodPaketa;          // string zbog moguce vodece nule
        private string _posiljalacIme;
        private string _posiljalacAdresa;
        private string _posiljalacGrad;
        private string _primalacIme;
        private string _primalacAdresa;
        private string _primalacPostanskiBroj;
        private string _primalacGrad;
        private string _tezina;             // string iz textbox-a, parsira se u decimal
        private string _oznakaUpozorenja;
        private string _cena;               // string iz textbox-a, parsira se u decimal

        private string _dodeljeniTipVozila; // rezultat poslovnog pravila

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

        public string Tezina
        {
            get { return _tezina; }
            set { _tezina = value; }
        }

        public string OznakaUpozorenja
        {
            get { return _oznakaUpozorenja; }
            set { _oznakaUpozorenja = value; }
        }

        public string Cena
        {
            get { return _cena; }
            set { _cena = value; }
        }

        public string DodeljeniTipVozila
        {
            get { return _dodeljeniTipVozila; }
        }

        // konstruktor
        public FormaPaketUnosKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // private metode
        private decimal ParsirajBroj(string tekst)
        {
            decimal vrednost = 0;
            // prihvata i zarez i tacku kao decimalni separator
            string normalizovano = tekst.Replace(',', '.');
            decimal.TryParse(normalizovano, NumberStyles.Any, CultureInfo.InvariantCulture, out vrednost);
            return vrednost;
        }

        // public metode
        public bool DaLiJeSvePopunjeno()
        {
            bool svePopunjeno = false;

            if ((this._kodPaketa.Length > 0) && (this._posiljalacIme.Length > 0)
                && (this._posiljalacAdresa.Length > 0) && (this._posiljalacGrad.Length > 0)
                && (this._primalacIme.Length > 0) && (this._primalacAdresa.Length > 0)
                && (this._primalacPostanskiBroj.Length > 0) && (this._primalacGrad.Length > 0)
                && (this._tezina.Length > 0) && (this._cena.Length > 0))
            {
                svePopunjeno = true;
            }
            else
            {
                svePopunjeno = false;
            }

            return svePopunjeno;
        }


        public bool DaLiJeJedinstvenZapis()
        {
            bool jedinstvenZapis = false;
            DataSet PodaciDataSet = new DataSet();
            SPPaketDBKlasa SPPaketDBObjekat = new SPPaketDBKlasa(this._stringKonekcije);
            PodaciDataSet = SPPaketDBObjekat.DajPaketPoKodu(this._kodPaketa);

            if (PodaciDataSet.Tables[0].Rows.Count == 0)
            {
                jedinstvenZapis = true;
            }
            else
            {
                jedinstvenZapis = false;
            }

            return jedinstvenZapis;

        }

        // POSLOVNO PRAVILO: na osnovu tezine dodeljuje tip vozila
        public string PrimeniPoslovnoPravilo()
        {
            decimal tezinaVrednost = this.ParsirajBroj(this._tezina);

            DodeljivanjeVozilaKlasa DodeljivanjeObjekat = new DodeljivanjeVozilaKlasa(this._stringKonekcije);
            _dodeljeniTipVozila = DodeljivanjeObjekat.DajTipVozilaZaTezinu(tezinaVrednost);

            return _dodeljeniTipVozila;
        }

        public string SnimiPodatke()
        {
            string porukaUspehaSnimanja = "";
            bool uspehSnimanja = false;

            // 1. provera popunjenosti
            bool SvePopunjeno = this.DaLiJeSvePopunjeno();
            if (SvePopunjeno == true)
            {
                porukaUspehaSnimanja = "Sve je popunjeno! ";
            }
            else
            {
                porukaUspehaSnimanja = "NIJE SVE POPUNJENO!";
                return porukaUspehaSnimanja;
            }

            // 2. provera jedinstvenosti koda paketa
            bool JedinstvenZapis = this.DaLiJeJedinstvenZapis();
            if (JedinstvenZapis == true)
            {
                porukaUspehaSnimanja = porukaUspehaSnimanja + "Jeste jedinstven zapis! ";
            }
            else
            {
                porukaUspehaSnimanja = porukaUspehaSnimanja + "VEC POSTOJI PAKET SA OVIM KODOM!";
                return porukaUspehaSnimanja;
            }

            // 3. POSLOVNO PRAVILO - dodela tipa vozila prema tezini
            string nazivVozila = this.PrimeniPoslovnoPravilo();
            porukaUspehaSnimanja = porukaUspehaSnimanja + "Dodeljeni tip vozila (prema tezini): " + nazivVozila + ". ";

            // 4. snimanje paketa
            SPPaketDBKlasa objPaketDB = new SPPaketDBKlasa(_stringKonekcije);

            PaketKlasa PaketObjekat = new PaketKlasa();
            PaketObjekat.KodPaketa = _kodPaketa;
            PaketObjekat.PosiljalacIme = _posiljalacIme;
            PaketObjekat.PosiljalacAdresa = _posiljalacAdresa;
            PaketObjekat.PosiljalacGrad = _posiljalacGrad;
            PaketObjekat.PrimalacIme = _primalacIme;
            PaketObjekat.PrimalacAdresa = _primalacAdresa;
            PaketObjekat.PrimalacPostanskiBroj = _primalacPostanskiBroj;
            PaketObjekat.PrimalacGrad = _primalacGrad;
            PaketObjekat.Tezina = this.ParsirajBroj(_tezina);
            PaketObjekat.OznakaUpozorenja = _oznakaUpozorenja;
            PaketObjekat.Cena = this.ParsirajBroj(_cena);

            // mapiranje naziva dodeljenog vozila u sifru iz sifarnika
            TipVozilaKlasa TipVozilaObjekat = new TipVozilaKlasa();
            SPTipVozilaDBKlasa SPTipVozilaDBObjekat = new SPTipVozilaDBKlasa(_stringKonekcije);
            TipVozilaObjekat.Sifra = SPTipVozilaDBObjekat.DajSifruVozilaPoNazivu(nazivVozila);
            TipVozilaObjekat.Naziv = nazivVozila;

            PaketObjekat.TipVozila = TipVozilaObjekat;

            uspehSnimanja = objPaketDB.SnimiNoviPaket(PaketObjekat);
            if (uspehSnimanja)
            {
                porukaUspehaSnimanja = porukaUspehaSnimanja + "Uspesno snimljen novi paket!";
            }
            else
            {
                porukaUspehaSnimanja = porukaUspehaSnimanja + "GRESKA SNIMANJA PODATAKA!";
            }
            return porukaUspehaSnimanja;

        }


    }
}
