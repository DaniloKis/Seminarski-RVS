using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using System.Data.SqlClient;

namespace KlasePodataka
{
    public class SPPaketDBKlasa : IEvidencija, IPaketRepo
    {
        // atributi
        private string _stringKonekcije;

        // implementacija interfejsa IEvidencija
        public DataSet DajSve()
        {
            return this.DajSvePakete();
        }

        // property
        // 1. nacin
        public string StringKonekcije
        {
            get
            {
                return _stringKonekcije;
            }
        }
        // konstruktor
        // 2. nacin prijema vrednosti stringa konekcije spolja i dodele atributu
        public SPPaketDBKlasa(string noviStringKonekcije)
        // OVO JE DOBRO JER OBAVEZUJE DA SE PRILIKOM INSTANCIRANJA OVE KLASE
        // MORA OBEZBEDITI STRING KONEKCIJE
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // privatne metode

        // javne metode
        public DataSet DajSvePakete()
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet PodaciDataSet = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajSvePaketeSaJoin", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return PodaciDataSet;
        }

        public DataSet DajPaketPoPrimaocu(string primalacFilter)
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet PodaciDataSet = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajPaketPoPrimaocu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@PaketPrimalac", SqlDbType.NVarChar).Value = primalacFilter;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return PodaciDataSet;
        }


        public DataSet DajPaketPoKoduFilter(string kodFilter)
        {
            // filtrira pakete po kodu (LIKE) i vraca kolone za prikaz (sa nazivom vozila)
            DataSet PodaciDataSet = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajPaketPoKoduFilter", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@PaketKod", SqlDbType.NVarChar).Value = kodFilter;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return PodaciDataSet;
        }


        public DataSet DajPaketPoKodu(string kodFilter)
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet PodaciDataSet = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajPaketPoKodu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@PaketKod", SqlDbType.NVarChar).Value = kodFilter;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return PodaciDataSet;
        }

        public int DajUkupnoPaketaZaVozilo(string IDVozilaFilter)
        {
            int ukupnoPaketa = 0;
            DataSet PodaciDataSet = new DataSet();
            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajBrojPaketaPremaSifriVozila", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@SifraVozila", SqlDbType.NVarChar).Value = IDVozilaFilter;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();
            ukupnoPaketa = int.Parse(PodaciDataSet.Tables[0].Rows[0].ItemArray[0].ToString());
            return ukupnoPaketa;
        }


        private PaketListaKlasa DajListuSvihPaketa()
        {
            // PRIPREMA PROMENLJIVIH
            PaketListaKlasa paketListaObjekat = new PaketListaKlasa();
            DataSet podaciDataSetPaketa = new DataSet();
            PaketKlasa PaketObjekat;
            TipVozilaKlasa TipVozilaObjekat;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajSvePaketeSaJoinSifromVozila", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(podaciDataSetPaketa);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            // FORMIRANJE OBJEKATA I UBACIVANJE U LISTU
            for (int brojac = 0; brojac < podaciDataSetPaketa.Tables[0].Rows.Count; brojac++)
            {
                TipVozilaObjekat = new TipVozilaKlasa();
                TipVozilaObjekat.Naziv = podaciDataSetPaketa.Tables[0].Rows[brojac].ItemArray[3].ToString();
                TipVozilaObjekat.Sifra = podaciDataSetPaketa.Tables[0].Rows[brojac].ItemArray[4].ToString();

                PaketObjekat = new PaketKlasa();
                PaketObjekat.KodPaketa = podaciDataSetPaketa.Tables[0].Rows[brojac].ItemArray[0].ToString();
                PaketObjekat.PosiljalacIme = podaciDataSetPaketa.Tables[0].Rows[brojac].ItemArray[1].ToString();
                PaketObjekat.PrimalacIme = podaciDataSetPaketa.Tables[0].Rows[brojac].ItemArray[2].ToString();
                PaketObjekat.TipVozila = TipVozilaObjekat;
                paketListaObjekat.DodajElementListe(PaketObjekat);
            }

            return paketListaObjekat;
        }


        public bool SnimiNoviPaket(PaketKlasa noviPaketObjekat)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;

            SqlConnection pompomKonekcija = new SqlConnection(_stringKonekcije);
            pompomKonekcija.Open();

            SqlCommand pompomKomanda = new SqlCommand("DodajNoviPaket", pompomKonekcija);
            pompomKomanda.CommandType = CommandType.StoredProcedure;
            pompomKomanda.Parameters.Add("@KodPaketa", SqlDbType.NVarChar).Value = noviPaketObjekat.KodPaketa;
            pompomKomanda.Parameters.Add("@PosiljalacIme", SqlDbType.NVarChar).Value = noviPaketObjekat.PosiljalacIme;
            pompomKomanda.Parameters.Add("@PosiljalacAdresa", SqlDbType.NVarChar).Value = noviPaketObjekat.PosiljalacAdresa;
            pompomKomanda.Parameters.Add("@PosiljalacGrad", SqlDbType.NVarChar).Value = noviPaketObjekat.PosiljalacGrad;
            pompomKomanda.Parameters.Add("@PrimalacIme", SqlDbType.NVarChar).Value = noviPaketObjekat.PrimalacIme;
            pompomKomanda.Parameters.Add("@PrimalacAdresa", SqlDbType.NVarChar).Value = noviPaketObjekat.PrimalacAdresa;
            pompomKomanda.Parameters.Add("@PrimalacPostanskiBroj", SqlDbType.NVarChar).Value = noviPaketObjekat.PrimalacPostanskiBroj;
            pompomKomanda.Parameters.Add("@PrimalacGrad", SqlDbType.NVarChar).Value = noviPaketObjekat.PrimalacGrad;
            pompomKomanda.Parameters.Add("@Tezina", SqlDbType.Decimal).Value = noviPaketObjekat.Tezina;
            pompomKomanda.Parameters.Add("@OznakaUpozorenja", SqlDbType.NVarChar).Value = noviPaketObjekat.OznakaUpozorenja;
            pompomKomanda.Parameters.Add("@Cena", SqlDbType.Decimal).Value = noviPaketObjekat.Cena;
            pompomKomanda.Parameters.Add("@IDTipaVozila", SqlDbType.NVarChar).Value = noviPaketObjekat.TipVozila.Sifra;

            brojSlogova = pompomKomanda.ExecuteNonQuery();
            pompomKonekcija.Close();
            pompomKonekcija.Dispose();

            // 2. varijanta
            return (brojSlogova > 0);

        }

        public bool ObrisiPaket(string KodPaketaZaBrisanje)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("ObrisiPaket", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@KodPaketa", SqlDbType.NVarChar).Value = KodPaketaZaBrisanje;
            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return (brojSlogova > 0);

        }

        public bool IzmeniPaket(PaketKlasa stariPaketObjekat, PaketKlasa noviPaketObjekat)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("IzmeniPaket", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@StariKod", SqlDbType.NVarChar).Value = stariPaketObjekat.KodPaketa;
            pomKomanda.Parameters.Add("@KodPaketa", SqlDbType.NVarChar).Value = noviPaketObjekat.KodPaketa;
            pomKomanda.Parameters.Add("@PosiljalacIme", SqlDbType.NVarChar).Value = noviPaketObjekat.PosiljalacIme;
            pomKomanda.Parameters.Add("@PosiljalacAdresa", SqlDbType.NVarChar).Value = noviPaketObjekat.PosiljalacAdresa;
            pomKomanda.Parameters.Add("@PosiljalacGrad", SqlDbType.NVarChar).Value = noviPaketObjekat.PosiljalacGrad;
            pomKomanda.Parameters.Add("@PrimalacIme", SqlDbType.NVarChar).Value = noviPaketObjekat.PrimalacIme;
            pomKomanda.Parameters.Add("@PrimalacAdresa", SqlDbType.NVarChar).Value = noviPaketObjekat.PrimalacAdresa;
            pomKomanda.Parameters.Add("@PrimalacPostanskiBroj", SqlDbType.NVarChar).Value = noviPaketObjekat.PrimalacPostanskiBroj;
            pomKomanda.Parameters.Add("@PrimalacGrad", SqlDbType.NVarChar).Value = noviPaketObjekat.PrimalacGrad;
            pomKomanda.Parameters.Add("@Tezina", SqlDbType.Decimal).Value = noviPaketObjekat.Tezina;
            pomKomanda.Parameters.Add("@OznakaUpozorenja", SqlDbType.NVarChar).Value = noviPaketObjekat.OznakaUpozorenja;
            pomKomanda.Parameters.Add("@Cena", SqlDbType.Decimal).Value = noviPaketObjekat.Cena;
            pomKomanda.Parameters.Add("@IDTipaVozila", SqlDbType.NVarChar).Value = noviPaketObjekat.TipVozila.Sifra;
            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return (brojSlogova > 0);

        }

        // CITANJE za MVC prikaz - vraca jako tipizirane objekte (List<PaketKlasa>),
        // a ne DataSet/DataTable. Ovako podaci ostaju u sloju podataka, a MVC
        // kontroler samo poziva metodu i prosledjuje model prikazu.
        public List<PaketKlasa> DajSvePaketeLista()
        {
            List<PaketKlasa> listaPaketa = new List<PaketKlasa>();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand(
                "SELECT p.KodPaketa, p.PosiljalacIme, p.PrimalacIme, p.Tezina, " +
                "ISNULL(v.Naziv, p.IDTipaVozila) AS NazivVozila " +
                "FROM Paket p LEFT JOIN TipVozila v ON p.IDTipaVozila = v.Sifra " +
                "ORDER BY p.KodPaketa", pomKonekcija);

            SqlDataReader citac = pomKomanda.ExecuteReader();
            while (citac.Read())
            {
                TipVozilaKlasa voziloObjekat = new TipVozilaKlasa();
                voziloObjekat.Naziv = citac["NazivVozila"].ToString();

                PaketKlasa paketObjekat = new PaketKlasa();
                paketObjekat.KodPaketa = citac["KodPaketa"].ToString();
                paketObjekat.PosiljalacIme = citac["PosiljalacIme"].ToString();
                paketObjekat.PrimalacIme = citac["PrimalacIme"].ToString();
                paketObjekat.Tezina = citac["Tezina"] == DBNull.Value ? 0 : Convert.ToDecimal(citac["Tezina"]);
                paketObjekat.TipVozila = voziloObjekat;

                listaPaketa.Add(paketObjekat);
            }
            citac.Close();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return listaPaketa;
        }

        // vraca jedan paket kao objekat (za master-detail prikaz)
        public PaketKlasa DajPaketPoKoduObjekat(string kodPaketa)
        {
            PaketKlasa paketObjekat = null;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand(
                "SELECT p.KodPaketa, p.PosiljalacIme, p.PrimalacIme, p.PrimalacGrad, " +
                "p.Tezina, p.Cena, ISNULL(v.Naziv, p.IDTipaVozila) AS NazivVozila " +
                "FROM Paket p LEFT JOIN TipVozila v ON p.IDTipaVozila = v.Sifra " +
                "WHERE p.KodPaketa = @Kod", pomKonekcija);
            pomKomanda.Parameters.Add("@Kod", SqlDbType.NVarChar).Value = kodPaketa;

            SqlDataReader citac = pomKomanda.ExecuteReader();
            if (citac.Read())
            {
                TipVozilaKlasa voziloObjekat = new TipVozilaKlasa();
                voziloObjekat.Naziv = citac["NazivVozila"].ToString();

                paketObjekat = new PaketKlasa();
                paketObjekat.KodPaketa = citac["KodPaketa"].ToString();
                paketObjekat.PosiljalacIme = citac["PosiljalacIme"].ToString();
                paketObjekat.PrimalacIme = citac["PrimalacIme"].ToString();
                paketObjekat.PrimalacGrad = citac["PrimalacGrad"].ToString();
                paketObjekat.Tezina = citac["Tezina"] == DBNull.Value ? 0 : Convert.ToDecimal(citac["Tezina"]);
                paketObjekat.Cena = citac["Cena"] == DBNull.Value ? 0 : Convert.ToDecimal(citac["Cena"]);
                paketObjekat.TipVozila = voziloObjekat;
            }
            citac.Close();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return paketObjekat;
        }




    }
}
