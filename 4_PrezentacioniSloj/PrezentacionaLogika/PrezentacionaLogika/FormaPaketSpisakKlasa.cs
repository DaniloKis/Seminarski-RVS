using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaPaketSpisakKlasa
    {
        // atributi
        private string _stringKonekcije;

        // property

        // konstruktor
        public FormaPaketSpisakKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // private metode

        // public metode
        public DataSet DajPodatkeZaGrid(string filter)
        {
            DataSet PodaciDataSet = new DataSet();
            SPPaketDBKlasa objPaketDB = new SPPaketDBKlasa(_stringKonekcije);
            if (filter.Equals(""))
            {
                PodaciDataSet = objPaketDB.DajSvePakete();
            }
            else
            {
                // filtriranje po kodu paketa
                PodaciDataSet = objPaketDB.DajPaketPoKoduFilter(filter);
            }
            return PodaciDataSet;
        }

    }
}
