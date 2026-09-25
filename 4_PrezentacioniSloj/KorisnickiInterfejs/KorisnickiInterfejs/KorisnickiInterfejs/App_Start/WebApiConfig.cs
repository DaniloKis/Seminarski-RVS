using System.Web.Http;

namespace KorisnickiInterfejs
{
    // Konfiguracija REST servisa (ASP.NET Web API 2).
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // atributne rute ([Route("api/ogranicenja/{sifraVozila}")])
            config.MapHttpAttributeRoutes();

            // konvencionalna ruta (rezerva)
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
