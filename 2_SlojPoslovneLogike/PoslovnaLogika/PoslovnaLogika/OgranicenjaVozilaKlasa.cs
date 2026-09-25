using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoslovnaLogika
{
    public class OgranicenjaVozilaKlasa
    {
        // preko web servisa cita granicu tezine (u kg) do koje odredjeni tip vozila sme da nosi paket
        public int DajGranicuTezineZaVozilo(String SifraVozilaWSParametar)
        {
            WSOgranicenja.OgranicenjaVozila objOgranicenja = new WSOgranicenja.OgranicenjaVozila();
            int pomGranicaTezine = objOgranicenja.DajGranicuTezine(SifraVozilaWSParametar);
            return pomGranicaTezine;
        }

    }
}
