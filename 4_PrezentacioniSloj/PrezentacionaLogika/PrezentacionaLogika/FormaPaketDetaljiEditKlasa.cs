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
    public class FormaPaketDetaljiEditKlasa
    {
        // atributi
        private string _stringKonekcije;
        private SPPaketDBKlasa SPPaketDBObjekat;

        private string _stariKod;              // originalni kod (kljuc za WHERE)

        // polja paketa (koriste se i za prikaz posle ucitavanja i za izmenjene vrednosti)
        private string _kodPaketa;
        private string _posiljalacIme;
        private string _posiljalacAdresa;
        private string _posiljalacGrad;
        private string _primalacIme;
        private string _primalacAdresa;
        private string _primalacPostanskiBroj;
        private string _primalacGrad;
        private string _tezina;
        private string _oznakaUpozorenja;
        private string _cena;

        private string _dodeljeniTipVozilaNaziv;

        // PROPERTY
        public string StariKod
        {
            get { return _stariKod; }
            set { _stariKod = value; }
        }

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

        public string DodeljeniTipVozilaNaziv
        {
            get { return _dodeljeniTipVozilaNaziv; }
        }

        // konstruktor
        public FormaPaketDetaljiEditKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
            SPPaketDBObjekat = new SPPaketDBKlasa(_stringKonekcije);
        }

        // privatne metode
        private decimal ParsirajBroj(string tekst)
        {
            decimal vrednost = 0;
            string normalizovano = (tekst == null ? "" : tekst).Replace(',', '.');
            decimal.TryParse(normalizovano, NumberStyles.Any, CultureInfo.InvariantCulture, out vrednost);
            return vrednost;
        }

        // javne metode
        public void UcitajPaket()
        {
            DataSet PodaciDataSet = SPPaketDBObjekat.DajPaketPoKodu(_stariKod);
            if (PodaciDataSet.Tables[0].Rows.Count > 0)
            {
                DataRow red = PodaciDataSet.Tables[0].Rows[0];
                _kodPaketa = red.ItemArray[0].ToString();
                _posiljalacIme = red.ItemArray[1].ToString();
                _posiljalacAdresa = red.ItemArray[2].ToString();
                _posiljalacGrad = red.ItemArray[3].ToString();
                _primalacIme = red.ItemArray[4].ToString();
                _primalacAdresa = red.ItemArray[5].ToString();
                _primalacPostanskiBroj = red.ItemArray[6].ToString();
                _primalacGrad = red.ItemArray[7].ToString();
                _tezina = red.ItemArray[8].ToString();
                _oznakaUpozorenja = red.ItemArray[9].ToString();
                _cena = red.ItemArray[10].ToString();

                // naziv trenutno dodeljenog vozila (iz sifre)
                string sifraVozila = red.ItemArray[11].ToString();
                SPTipVozilaDBKlasa SPTipVozilaDBObjekat = new SPTipVozilaDBKlasa(_stringKonekcije);
                _dodeljeniTipVozilaNaziv = SPTipVozilaDBObjekat.DajNazivPremaIDVozila(sifraVozila);
            }
        }

        public bool ObrisiPaket()
        {
            return SPPaketDBObjekat.ObrisiPaket(_stariKod);
        }

        public string IzmeniPodatke()
        {
            // POSLOVNO PRAVILO se ponovo primenjuje na izmenjenu tezinu
            decimal tezinaVrednost = this.ParsirajBroj(_tezina);
            DodeljivanjeVozilaKlasa DodeljivanjeObjekat = new DodeljivanjeVozilaKlasa(_stringKonekcije);
            _dodeljeniTipVozilaNaziv = DodeljivanjeObjekat.DajTipVozilaZaTezinu(tezinaVrednost);

            // mapiranje naziva vozila u sifru
            SPTipVozilaDBKlasa SPTipVozilaDBObjekat = new SPTipVozilaDBKlasa(_stringKonekcije);
            TipVozilaKlasa TipVozilaObjekat = new TipVozilaKlasa();
            TipVozilaObjekat.Naziv = _dodeljeniTipVozilaNaziv;
            TipVozilaObjekat.Sifra = SPTipVozilaDBObjekat.DajSifruVozilaPoNazivu(_dodeljeniTipVozilaNaziv);

            // stari paket (samo kljuc je bitan za WHERE)
            PaketKlasa stariPaketObjekat = new PaketKlasa();
            stariPaketObjekat.KodPaketa = _stariKod;

            // novi (izmenjeni) paket
            PaketKlasa noviPaketObjekat = new PaketKlasa();
            noviPaketObjekat.KodPaketa = _kodPaketa;
            noviPaketObjekat.PosiljalacIme = _posiljalacIme;
            noviPaketObjekat.PosiljalacAdresa = _posiljalacAdresa;
            noviPaketObjekat.PosiljalacGrad = _posiljalacGrad;
            noviPaketObjekat.PrimalacIme = _primalacIme;
            noviPaketObjekat.PrimalacAdresa = _primalacAdresa;
            noviPaketObjekat.PrimalacPostanskiBroj = _primalacPostanskiBroj;
            noviPaketObjekat.PrimalacGrad = _primalacGrad;
            noviPaketObjekat.Tezina = tezinaVrednost;
            noviPaketObjekat.OznakaUpozorenja = _oznakaUpozorenja;
            noviPaketObjekat.Cena = this.ParsirajBroj(_cena);
            noviPaketObjekat.TipVozila = TipVozilaObjekat;

            bool uspehIzmene = SPPaketDBObjekat.IzmeniPaket(stariPaketObjekat, noviPaketObjekat);
            if (uspehIzmene)
            {
                return "Uspesno izmenjen paket! Dodeljeni tip vozila (prema tezini): " + _dodeljeniTipVozilaNaziv + ".";
            }
            else
            {
                return "NEUSPEH IZMENE paketa!";
            }
        }
    }
}
