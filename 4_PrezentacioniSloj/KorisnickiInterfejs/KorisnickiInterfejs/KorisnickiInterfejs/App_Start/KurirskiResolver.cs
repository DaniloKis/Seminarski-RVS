using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
//
using KlasePodataka;
using KorisnickiInterfejs.Controllers;

namespace KorisnickiInterfejs
{
    // JEDNOSTAVAN DEPENDENCY INJECTION RESOLVER (bez spoljnog kontejnera).
    // MVC ga koristi da napravi kontroler i da mu "ubaci" zavisnost preko
    // INTERFEJSA (IPaketRepo). Tako kontroler ne zavisi od konkretne klase,
    // vec od apstrakcije - a implementaciju odredjuje ovaj resolver.
    public class KurirskiResolver : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            string konekcioniString =
                ConfigurationManager.ConnectionStrings["NasaKonekcija"].ConnectionString;

            // 1) kada se trazi interfejs IPaketRepo -> vrati konkretnu implementaciju
            if (serviceType == typeof(IPaketRepo))
            {
                return new SPPaketDBKlasa(konekcioniString);
            }

            // 2) kada se trazi PaketMvcController -> napravi ga i UBACI mu
            //    zavisnost (IPaketRepo) kroz konstruktor
            if (serviceType == typeof(PaketMvcController))
            {
                IPaketRepo paketRepoObjekat = new SPPaketDBKlasa(konekcioniString);
                return new PaketMvcController(paketRepoObjekat);
            }

            // za sve ostalo -> null (MVC koristi podrazumevani nacin kreiranja)
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }
    }
}
