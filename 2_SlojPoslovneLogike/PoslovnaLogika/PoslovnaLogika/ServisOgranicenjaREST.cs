using System;
using System.Net;

namespace PoslovnaLogika
{
    // KLIJENT REST SERVISA.
    // Poslovna logika preko ovog klijenta dobija PARAMETAR (granicu tezine za
    // zadati tip vozila) od REST servisa. Analogno klasi ServisProvereKurseva
    // iz reference koja preko REST-a dobija podatak za poslovnu odluku.
    //
    // REST servis (Web API 2) je na ruti:  {baseUrl}/{sifraVozila}
    // i vraca ceo broj - granicu tezine u kilogramima.
    public class ServisOgranicenjaREST
    {
        private string _baseUrl;
        private string _token;

        // podrazumevana (rezervna) granica ako REST servis nije dostupan
        private const int PODRAZUMEVANA_GRANICA_KG = 30;

        // naziv HTTP header-a u kome se salje sigurnosni token
        private const string TOKEN_HEADER = "X-Api-Token";

        // baseUrl npr: "http://localhost:1202/api/ogranicenja"
        // token: sigurnosni token koji servis zahteva za pristup
        public ServisOgranicenjaREST(string baseUrl, string token)
        {
            _baseUrl = baseUrl;
            _token = token;
        }

        public int DajGranicuTezine(string sifraVozila)
        {
            int granica = PODRAZUMEVANA_GRANICA_KG;
            try
            {
                string url = _baseUrl;
                if (url.EndsWith("/") == false)
                {
                    url = url + "/";
                }
                url = url + sifraVozila;

                // sinhroni poziv (WebClient) - bez async/await da se izbegne
                // blokiranje konteksta u klasicnom ASP.NET-u
                using (WebClient klijent = new WebClient())
                {
                    // slanje sigurnosnog tokena u header-u (bezbedno koriscenje servisa)
                    if (!string.IsNullOrEmpty(_token))
                    {
                        klijent.Headers.Add(TOKEN_HEADER, _token);
                    }
                    string odgovor = klijent.DownloadString(url);
                    // REST vraca ceo broj kao tekst, npr "30"
                    granica = int.Parse(odgovor.Trim().Trim('"'));
                }
            }
            catch
            {
                // ako REST servis nije pokrenut, koristi se rezervna vrednost
                granica = PODRAZUMEVANA_GRANICA_KG;
            }
            return granica;
        }
    }
}
