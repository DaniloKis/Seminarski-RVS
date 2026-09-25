using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaTipVozilaSpisakKlasa
    {
        // atributi
        private string _stringKonekcije;

        // konstruktor
        public FormaTipVozilaSpisakKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // public metode
        public DataSet DajPodatkeZaGrid(string filter)
        {
            DataSet PodaciDataSet = new DataSet();
            SPTipVozilaDBKlasa SPTipVozilaDBObjekat = new SPTipVozilaDBKlasa(_stringKonekcije);
            if (filter.Equals(""))
            {
                PodaciDataSet = SPTipVozilaDBObjekat.DajSvaVozila();
            }
            else
            {
                PodaciDataSet = SPTipVozilaDBObjekat.DajVozilaPoNazivu(filter);
            }
            return PodaciDataSet;
        }
    }
}
