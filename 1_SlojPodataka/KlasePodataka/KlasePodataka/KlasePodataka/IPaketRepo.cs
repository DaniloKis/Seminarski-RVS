using System;
using System.Collections.Generic;

namespace KlasePodataka
{
    // INTERFEJS REPOZITORIJUMA PAKETA.
    // Koristi se za DEPENDENCY INJECTION: MVC kontroler zavisi od ovog
    // interfejsa (apstrakcije), a ne od konkretne klase. Konkretnu
    // implementaciju (SPPaketDBKlasa) obezbedjuje DI resolver.
    public interface IPaketRepo
    {
        List<PaketKlasa> DajSvePaketeLista();
        PaketKlasa DajPaketPoKoduObjekat(string kodPaketa);
    }
}
