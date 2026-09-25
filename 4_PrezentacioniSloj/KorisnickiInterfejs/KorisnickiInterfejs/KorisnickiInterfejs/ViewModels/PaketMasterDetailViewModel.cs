using System.Collections.Generic;
using KlasePodataka;

namespace KorisnickiInterfejs.ViewModels
{
    // VIEW MODEL za MASTER-DETAIL PRIKAZ jednog paketa sa svim stavkama.
    public class PaketMasterDetailViewModel
    {
        // master
        public string KodPaketa { get; set; }
        public string PosiljalacIme { get; set; }
        public string PrimalacIme { get; set; }
        public string PrimalacGrad { get; set; }
        public decimal Tezina { get; set; }
        public decimal Cena { get; set; }
        public string NazivVozila { get; set; }

        // detalji
        public List<StavkaPaketaKlasa> Stavke { get; set; }

        public PaketMasterDetailViewModel()
        {
            Stavke = new List<StavkaPaketaKlasa>();
        }
    }
}
