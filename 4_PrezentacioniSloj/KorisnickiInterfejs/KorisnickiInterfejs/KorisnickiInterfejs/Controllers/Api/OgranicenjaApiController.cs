using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace KorisnickiInterfejs.Controllers.Api
{
    // REST SERVIS (ASP.NET Web API 2).
    // Ruta:  GET /api/ogranicenja/{sifraVozila}
    // Vraca ceo broj - granicu tezine (kg) za zadati tip vozila.
    //
    // Ovaj servis obezbedjuje PARAMETAR za poslovnu logiku (dodela vozila).
    // Granice se citaju iz fajla App_Data/ogranicenja.json.
    public class OgranicenjaApiController : ApiController
    {
        private const int PODRAZUMEVANA_GRANICA_KG = 30;

        // naziv HTTP header-a u kome se ocekuje sigurnosni token
        private const string TOKEN_HEADER = "X-Api-Token";

        // GET api/ogranicenja/{sifraVozila}
        [HttpGet]
        [Route("api/ogranicenja/{sifraVozila}")]
        public IHttpActionResult DajGranicuTezine(string sifraVozila)
        {
            // BEZBEDNO KORISCENJE SERVISA - provera tokena.
            // Servis se sme koristiti samo ako klijent posalje ispravan token
            // u header-u "X-Api-Token" (vrednost je u Web.config -> RestApiToken).
            if (!TokenJeIspravan())
            {
                return Unauthorized();
            }

            int granica = UcitajGranicu(sifraVozila);
            return Ok(granica);
        }

        private bool TokenJeIspravan()
        {
            string ocekivaniToken = ConfigurationManager.AppSettings["RestApiToken"];
            if (string.IsNullOrEmpty(ocekivaniToken))
            {
                // ako token nije podesen na serveru, provera se ne sprovodi
                return true;
            }

            IEnumerable<string> vrednostiHeadera;
            if (!Request.Headers.TryGetValues(TOKEN_HEADER, out vrednostiHeadera))
            {
                return false;
            }

            string prosledjeniToken = vrednostiHeadera.FirstOrDefault();
            return prosledjeniToken == ocekivaniToken;
        }

        private int UcitajGranicu(string sifraVozila)
        {
            try
            {
                string putanja = HostingEnvironment.MapPath("~/App_Data/ogranicenja.json");
                if (putanja == null || !File.Exists(putanja))
                {
                    return PODRAZUMEVANA_GRANICA_KG;
                }

                string json = File.ReadAllText(putanja);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, int> mapa = serializer.Deserialize<Dictionary<string, int>>(json);

                if (mapa == null)
                {
                    return PODRAZUMEVANA_GRANICA_KG;
                }

                if (sifraVozila != null && mapa.ContainsKey(sifraVozila))
                {
                    return mapa[sifraVozila];
                }

                if (mapa.ContainsKey("Podrazumevano"))
                {
                    return mapa["Podrazumevano"];
                }

                return PODRAZUMEVANA_GRANICA_KG;
            }
            catch
            {
                return PODRAZUMEVANA_GRANICA_KG;
            }
        }
    }
}
