using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaPaketStampaKlasa
    {
        // atributi
        private string _stringKonekcije;

        // konstruktor
        public FormaPaketStampaKlasa(string noviStringKonekcije)
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
                // filtriranje po kodu paketa
                PodaciDataSet = SPPaketDBObjekat.DajPaketPoKoduFilter(filter);
            }
            return PodaciDataSet;
        }
    }
}
