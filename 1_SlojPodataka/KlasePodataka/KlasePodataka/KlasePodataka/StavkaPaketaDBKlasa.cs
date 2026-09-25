using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
//
using DBUtils;
using System.Data;
using System.Data.SqlClient;

namespace KlasePodataka
{
    // DB KLASA ZA MASTER-DETAIL UPIS U JEDNOJ TRANSAKCIJI.
    // Koristi tehnolosku klasu TabelaKlasa iz DBUtils i njenu preklopljenu
    // metodu IzvrsiAzuriranje(List<string>) koja sve upite izvrsava u OKVIRU
    // JEDNE SQL transakcije (BeginTransaction / Commit / Rollback).
    // Tako se master (Paket) i svi detalji (StavkaPaketa) snime atomicno -
    // ili sve uspe, ili se sve ponisti.
    public class StavkaPaketaDBKlasa
    {
        // atributi
        private string _stringKonekcije;

        // property
        public string StringKonekcije
        {
            get { return _stringKonekcije; }
        }

        // konstruktor - obavezno prima string konekcije
        public StavkaPaketaDBKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // privatne pomocne metode
        private string Apostrof(string tekst)
        {
            // jednostavno "bekovanje" apostrofa da upit ne pukne
            if (tekst == null) return "";
            return tekst.Replace("'", "''");
        }

        private string Broj(decimal vrednost)
        {
            // decimalni separator mora biti tacka u SQL upitu
            return vrednost.ToString(CultureInfo.InvariantCulture);
        }

        // GLAVNA METODA: snima master paket + listu stavki u JEDNOJ transakciji
        public bool SnimiPaketSaStavkama(PaketKlasa paketObjekat, List<StavkaPaketaKlasa> listaStavki)
        {
            bool uspeh = false;

            // 1. otvaranje konekcije (TabelaKlasa dobija vec otvorenu konekciju)
            KonekcijaKlasa konekcijaObjekat = new KonekcijaKlasa(_stringKonekcije);
            bool uspehKonekcije = konekcijaObjekat.OtvoriKonekciju();
            if (uspehKonekcije == false)
            {
                return false;
            }

            // 2. priprema liste upita koji idu u jednu transakciju
            List<string> listaUpita = new List<string>();

            // 2a. MASTER - upis paketa
            string upitPaket =
                "insert into Paket values('"
                + Apostrof(paketObjekat.KodPaketa) + "','"
                + Apostrof(paketObjekat.PosiljalacIme) + "','"
                + Apostrof(paketObjekat.PosiljalacAdresa) + "','"
                + Apostrof(paketObjekat.PosiljalacGrad) + "','"
                + Apostrof(paketObjekat.PrimalacIme) + "','"
                + Apostrof(paketObjekat.PrimalacAdresa) + "','"
                + Apostrof(paketObjekat.PrimalacPostanskiBroj) + "','"
                + Apostrof(paketObjekat.PrimalacGrad) + "',"
                + Broj(paketObjekat.Tezina) + ",'"
                + Apostrof(paketObjekat.OznakaUpozorenja) + "',"
                + Broj(paketObjekat.Cena) + ",'"
                + Apostrof(paketObjekat.TipVozila.Sifra) + "')";
            listaUpita.Add(upitPaket);

            // 2b. DETALJI - upis svih stavki (vezane preko KodPaketa)
            if (listaStavki != null)
            {
                for (int i = 0; i < listaStavki.Count; i++)
                {
                    StavkaPaketaKlasa stavkaObjekat = listaStavki[i];
                    string upitStavka =
                        "insert into StavkaPaketa (KodPaketa, NazivArtikla, Kolicina, TezinaStavke) values('"
                        + Apostrof(paketObjekat.KodPaketa) + "','"
                        + Apostrof(stavkaObjekat.NazivArtikla) + "',"
                        + stavkaObjekat.Kolicina + ","
                        + Broj(stavkaObjekat.TezinaStavke) + ")";
                    listaUpita.Add(upitStavka);
                }
            }

            // 3. izvrsavanje SVIH upita u JEDNOJ transakciji
            TabelaKlasa tabelaObjekat = new TabelaKlasa(konekcijaObjekat, "Paket");
            uspeh = tabelaObjekat.IzvrsiAzuriranje(listaUpita);

            // 4. zatvaranje konekcije
            konekcijaObjekat.ZatvoriKonekciju();

            return uspeh;
        }

        // citanje svih stavki jednog paketa (za master-detail PRIKAZ)
        public List<StavkaPaketaKlasa> DajStavkePaketa(string kodPaketa)
        {
            List<StavkaPaketaKlasa> listaStavki = new List<StavkaPaketaKlasa>();

            DataSet podaciDataSet = new DataSet();
            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand(
                "select Id, KodPaketa, NazivArtikla, Kolicina, TezinaStavke from StavkaPaketa where KodPaketa=@Kod",
                pomKonekcija);
            pomKomanda.Parameters.Add("@Kod", SqlDbType.NVarChar).Value = kodPaketa;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(podaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            for (int brojac = 0; brojac < podaciDataSet.Tables[0].Rows.Count; brojac++)
            {
                DataRow red = podaciDataSet.Tables[0].Rows[brojac];
                StavkaPaketaKlasa stavkaObjekat = new StavkaPaketaKlasa();
                stavkaObjekat.Id = Convert.ToInt32(red["Id"]);
                stavkaObjekat.KodPaketa = red["KodPaketa"].ToString();
                stavkaObjekat.NazivArtikla = red["NazivArtikla"].ToString();
                stavkaObjekat.Kolicina = Convert.ToInt32(red["Kolicina"]);
                stavkaObjekat.TezinaStavke = Convert.ToDecimal(red["TezinaStavke"]);
                listaStavki.Add(stavkaObjekat);
            }

            return listaStavki;
        }
    }
}
