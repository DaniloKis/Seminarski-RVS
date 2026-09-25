using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    // DETALJNA (CHILD) KLASA ZA MASTER-DETAIL:
    // Jedan Paket (master) moze imati vise Stavki (detalj) - artikala u paketu.
    public class StavkaPaketaKlasa
    {
        // atributi
        private int _id;
        private string _kodPaketa;      // FK na Paket.KodPaketa (master)
        private string _nazivArtikla;
        private int _kolicina;
        private decimal _tezinaStavke;

        // property
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string KodPaketa
        {
            get { return _kodPaketa; }
            set { _kodPaketa = value; }
        }

        public string NazivArtikla
        {
            get { return _nazivArtikla; }
            set { _nazivArtikla = value; }
        }

        public int Kolicina
        {
            get { return _kolicina; }
            set { _kolicina = value; }
        }

        public decimal TezinaStavke
        {
            get { return _tezinaStavke; }
            set { _tezinaStavke = value; }
        }

        // konstruktor
        public StavkaPaketaKlasa()
        {
            _id = 0;
            _kodPaketa = "";
            _nazivArtikla = "";
            _kolicina = 0;
            _tezinaStavke = 0;
        }
    }
}
