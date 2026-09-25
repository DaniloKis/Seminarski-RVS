using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;

namespace KlasePodataka
{
    public class PaketDBKlasa : TabelaKlasa
    {


        public PaketDBKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele) : base(novaKonekcija, noviNazivTabele)
        {
            // nesto drugo u vezi specificno ove klase
        }


        public DataSet DajSvePakete()
        {
            return this.DajPodatke("select * from Paket");
        }

        public bool DodajNoviPaket(PaketKlasa noviPaketObjekat)
        {
            string upit = "insert into Paket values('"
                + noviPaketObjekat.KodPaketa + "', '"
                + noviPaketObjekat.PosiljalacIme + "','"
                + noviPaketObjekat.PosiljalacAdresa + "','"
                + noviPaketObjekat.PosiljalacGrad + "','"
                + noviPaketObjekat.PrimalacIme + "','"
                + noviPaketObjekat.PrimalacAdresa + "','"
                + noviPaketObjekat.PrimalacPostanskiBroj + "','"
                + noviPaketObjekat.PrimalacGrad + "',"
                + noviPaketObjekat.Tezina.ToString().Replace(",", ".") + ",'"
                + noviPaketObjekat.OznakaUpozorenja + "',"
                + noviPaketObjekat.Cena.ToString().Replace(",", ".") + ",'"
                + noviPaketObjekat.TipVozila.Sifra + "')";
            return this.IzvrsiAzuriranje(upit);

        }
    }
}
