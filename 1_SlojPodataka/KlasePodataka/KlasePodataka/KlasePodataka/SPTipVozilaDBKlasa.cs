using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using System.Data.SqlClient;

namespace KlasePodataka
{
    public class SPTipVozilaDBKlasa : IEvidencija
    {
        // atributi
        private string _stringKonekcije;

        // implementacija interfejsa IEvidencija
        public DataSet DajSve()
        {
            return this.DajSvaVozila();
        }

        // property
        // 1. nacin
        public string StringKonekcije
        {
            get
            {
                return _stringKonekcije;
            }
            set // OVO NIJE DOBRO, MOZE SE STRING KONEKCIJE STAVITI NA PRAZAN STRING
            {
                if (this._stringKonekcije != value)
                    this._stringKonekcije = value;
            }
        }
        // konstruktor
        // 2. nacin prijema vrednosti stringa konekcije spolja i dodele atributu
        public SPTipVozilaDBKlasa(string noviStringKonekcije)
        // OVO JE DOBRO JER OBAVEZUJE DA SE PRILIKOM INSTANCIRANJA OVE KLASE
        // MORA OBEZBEDITI STRING KONEKCIJE
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // privatne metode

        // javne metode
        public DataSet DajSvaVozila()
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet PodaciDataSet = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajSvaVozila", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return PodaciDataSet;
        }

        public string DajNazivPremaIDVozila(string IDVozilaFilter)
        {
            string pomNazivVozila = "";

            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet PodaciDataSet = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajVoziloPoSifri", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@SifraVozila", SqlDbType.NVarChar).Value = IDVozilaFilter;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            pomNazivVozila = PodaciDataSet.Tables[0].Rows[0].ItemArray[1].ToString();

            return pomNazivVozila;
        }

        public string DajSifruVozilaPoNazivu(string nazivVozilaFilter)
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet PodaciDataSet = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajVoziloPoNazivu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@NazivVozila", SqlDbType.NVarChar).Value = nazivVozilaFilter;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return PodaciDataSet.Tables[0].Rows[0].ItemArray[0].ToString();
        }



        public DataSet DajVozilaPoNazivu(string nazivVozilaFilter)
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet dsPodaci = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajVoziloPoNazivu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@NazivVozila", SqlDbType.NVarChar).Value = nazivVozilaFilter;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(dsPodaci);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return dsPodaci;
        }

        // overloading metoda - isto se zove, ima drugaciji parametar
        public DataSet DajVozilaPoNazivu(TipVozilaKlasa voziloObjekatZaFilter)
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet dsPodaci = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajVoziloPoNazivu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@NazivVozila", SqlDbType.NVarChar).Value = voziloObjekatZaFilter.Naziv;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(dsPodaci);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return dsPodaci;
        }

        public bool SnimiNovoVozilo(TipVozilaKlasa novoVoziloObjekat)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("DodajNovoVozilo", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@Sifra", SqlDbType.NVarChar).Value = novoVoziloObjekat.Sifra;
            pomKomanda.Parameters.Add("@Naziv", SqlDbType.NVarChar).Value = novoVoziloObjekat.Naziv;

            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return (brojSlogova > 0);
        }

        public bool ObrisiVozilo(TipVozilaKlasa voziloObjekatZaBrisanje)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("ObrisiVozilo", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@Sifra", SqlDbType.NVarChar).Value = voziloObjekatZaBrisanje.Sifra;
            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return (brojSlogova > 0);
        }

        // method overloading - ista procedura sa razlicitim parametrom
        public bool ObrisiVozilo(string sifraVozilaZaBrisanje)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("ObrisiVozilo", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@Sifra", SqlDbType.NVarChar).Value = sifraVozilaZaBrisanje;
            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return (brojSlogova > 0);
        }

        public bool IzmeniVozilo(TipVozilaKlasa staroVoziloObjekat, TipVozilaKlasa novoVoziloObjekat)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("IzmeniVozilo", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@StaraSifra", SqlDbType.NVarChar).Value = staroVoziloObjekat.Sifra;
            pomKomanda.Parameters.Add("@Sifra", SqlDbType.NVarChar).Value = novoVoziloObjekat.Sifra;
            pomKomanda.Parameters.Add("@Naziv", SqlDbType.NVarChar).Value = novoVoziloObjekat.Naziv;
            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return (brojSlogova > 0);
        }

        // method overloading - ista metoda, samo drugaciji parametri
        public bool IzmeniVozilo(string sifraStarogVozila, TipVozilaKlasa novoVoziloObjekat)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("IzmeniVozilo", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@StaraSifra", SqlDbType.NVarChar).Value = sifraStarogVozila;
            pomKomanda.Parameters.Add("@Sifra", SqlDbType.NVarChar).Value = novoVoziloObjekat.Sifra;
            pomKomanda.Parameters.Add("@Naziv", SqlDbType.NVarChar).Value = novoVoziloObjekat.Naziv;
            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return (brojSlogova > 0);
        }


    }
}
