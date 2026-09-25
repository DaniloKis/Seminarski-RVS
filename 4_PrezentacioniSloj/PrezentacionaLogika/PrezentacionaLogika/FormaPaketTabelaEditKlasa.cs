using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaPaketTabelaEditKlasa
    {
        // atributi
        private string _stringKonekcije;

        // konstruktor
        public FormaPaketTabelaEditKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // public metode
        public DataSet DajPodatkeZaGrid(string filter)
        {
            DataSet PodaciDataSet = new DataSet();
            SPPaketDBKlasa SPPaketDBObjekat = new SPPaketDBKlasa(_stringKonekcije);
            if (filter.Equals(""))
            {
                PodaciDataSet = SPPaketDBObjekat.DajSvePakete();
            }
            else
            {
                PodaciDataSet = SPPaketDBObjekat.DajPaketPoPrimaocu(filter);
            }
            return PodaciDataSet;
        }
    }
}
