using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;

namespace KlasePodataka
{
    // INTERFEJS KLASA
    // Definise zajednicku sposobnost DB klasa evidencije: vracanje svih zapisa.
    // Implementiraju je konkretne klase SPPaketDBKlasa i SPTipVozilaDBKlasa,
    // cime se ostvaruje polimorfizam (rad sa razlicitim evidencijama preko istog tipa).
    public interface IEvidencija
    {
        DataSet DajSve();
    }
}
