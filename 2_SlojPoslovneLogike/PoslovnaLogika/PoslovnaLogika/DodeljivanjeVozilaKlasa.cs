using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using KlasePodataka;
using KlaseMapiranja;

namespace PoslovnaLogika
{
    public class DodeljivanjeVozilaKlasa
    {
        // POSLOVNO PRAVILO:
        // AKO paket prelazi odredjenu granicu tezine (granica standardnog dostavnog vozila - Kombi)
        // ONDA se dodeljuje posebnom tipu vozila (Kamion).
        //
        // Preradjeno iz 3 klase (analogno originalu):
        // OgranicenjaVozila - preko web servisa izdvaja granicu tezine za tip vozila
        // Opterecenje       - stanje u bazi povodom broja paketa po vozilu
        // DodeljivanjeVozila - primenjuje poslovno pravilo i vraca dodeljeni tip vozila

        // atributi
        private string _stringKonekcije;

        // podrazumevana (rezervna) granica ako web servis nije pokrenut
        private const int PODRAZUMEVANA_GRANICA_KG = 30;

        // konstruktor
        public DodeljivanjeVozilaKlasa(string NoviStringKonekcije)
        {
            _stringKonekcije = NoviStringKonekcije;
        }

        // vraca granicu tezine standardnog vozila (Kombi) - preko web servisa, uz rezervnu vrednost
        public int DajGranicuStandardnogVozila()
        {
            int granica = PODRAZUMEVANA_GRANICA_KG;
            try
            {
                // ################################################################
                // MAPIRANJE SLOJEVA - naziv vozila iz baze -> sifra za web servis
                MaperKlasa maperObjekat = new MaperKlasa(_stringKonekcije);
                string sifraVozilaWS = maperObjekat.DajSifruVozilaZaWebServis("Kombi");

                // ################################################################
                // IZDVAJANJE GRANICE TEZINE ZA STANDARDNO VOZILO PREKO WEB SERVISA
                OgranicenjaVozilaKlasa ogranicenjaObjekat = new OgranicenjaVozilaKlasa();
                granica = ogranicenjaObjekat.DajGranicuTezineZaVozilo(sifraVozilaWS);
            }
            catch
            {
                // ako web servis nije dostupan, koristi se rezervna (podrazumevana) granica
                granica = PODRAZUMEVANA_GRANICA_KG;
            }
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
                nazivDodeljenogVozila = "Kamion";
            }
            else
            {
                // paket je u granicama -> standardno dostavno vozilo
                nazivDodeljenogVozila = "Kombi";
            }

            return nazivDodeljenogVozila;
        }
    }
}
