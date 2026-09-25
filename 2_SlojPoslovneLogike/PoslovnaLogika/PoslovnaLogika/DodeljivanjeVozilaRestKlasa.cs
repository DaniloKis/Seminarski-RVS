using System;
//
using KlasePodataka;

namespace PoslovnaLogika
{
    // POSLOVNO PRAVILO KOJE KORISTI REST SERVIS KAO IZVOR PARAMETRA.
    //
    // Ista logika kao DodeljivanjeVozilaKlasa (tezina -> Kombi/Kamion),
    // ali se granica tezine standardnog vozila dobija preko REST servisa
    // (ServisOgranicenjaREST), a ne preko SOAP/ASMX web servisa.
    //
    // Time je ispunjen zahtev: "REST servis obezbedjuje parametar za poslovnu logiku".
    public class DodeljivanjeVozilaRestKlasa
    {
        private string _stringKonekcije;
        private string _restBaseUrl;
        private string _restToken;

        private const string NAZIV_STANDARDNOG_VOZILA = "Kombi";
        private const string NAZIV_POSEBNOG_VOZILA = "Kamion";

        public DodeljivanjeVozilaRestKlasa(string noviStringKonekcije, string restBaseUrl, string restToken)
        {
            _stringKonekcije = noviStringKonekcije;
            _restBaseUrl = restBaseUrl;
            _restToken = restToken;
        }

        // vraca granicu tezine standardnog vozila (Kombi) - PREKO REST SERVISA
        public int DajGranicuStandardnogVozila()
        {
            // 1. sifra standardnog vozila iz sifarnika (TipVozila) u bazi
            SPTipVozilaDBKlasa tipVozilaDBObjekat = new SPTipVozilaDBKlasa(_stringKonekcije);
            string sifraVozila = tipVozilaDBObjekat.DajSifruVozilaPoNazivu(NAZIV_STANDARDNOG_VOZILA);

            // 2. granica tezine za tu sifru - DOBIJA SE OD REST SERVISA
            ServisOgranicenjaREST servisRestObjekat = new ServisOgranicenjaREST(_restBaseUrl, _restToken);
            int granica = servisRestObjekat.DajGranicuTezine(sifraVozila);

            return granica;
        }

        // POSLOVNO PRAVILO: na osnovu tezine vraca naziv dodeljenog tipa vozila
        public string DajTipVozilaZaTezinu(decimal tezinaPaketa)
        {
            string nazivDodeljenogVozila;

            int granicaStandardnog = this.DajGranicuStandardnogVozila();

            if (tezinaPaketa > granicaStandardnog)
            {
                // paket prelazi granicu -> posebni tip vozila
                nazivDodeljenogVozila = NAZIV_POSEBNOG_VOZILA;
            }
            else
            {
                // paket je u granicama -> standardno dostavno vozilo
                nazivDodeljenogVozila = NAZIV_STANDARDNOG_VOZILA;
            }

            return nazivDodeljenogVozila;
        }
    }
}
