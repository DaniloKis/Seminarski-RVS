using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using KlasePodataka;

namespace KlaseMapiranja
{
    public class MaperKlasa
    {
        // atributi
        private string pStringKonekcije;

        // property

        // konstruktor
        public MaperKlasa(string noviStringKonekcije)
        {
            pStringKonekcije = noviStringKonekcije;
        }
        public string DajSifruVozilaZaWebServis(string NazivVozilaIzBazePodatakaParametar)
        {
            string pomIDVozilaWS = "";

            // OVO JE HEURISTIKA:
            // prva tri slova naziva (velikim slovima) su sifra u drugom sistemu (web servisu)
            // DAKLE: za "Kombi" je "KOM", za "Kamion" je "KAM", za "Motor" je "MOT"
            if (NazivVozilaIzBazePodatakaParametar.Length >= 3)
                pomIDVozilaWS = NazivVozilaIzBazePodatakaParametar.Substring(0, 3).ToUpper();
            else
                pomIDVozilaWS = NazivVozilaIzBazePodatakaParametar.ToUpper();

            return pomIDVozilaWS;

        }

    }
}
