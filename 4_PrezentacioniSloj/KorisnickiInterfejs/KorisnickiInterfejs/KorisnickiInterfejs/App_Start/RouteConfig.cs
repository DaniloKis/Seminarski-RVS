using System.Web.Mvc;
using System.Web.Routing;

namespace KorisnickiInterfejs
{
    // Konfiguracija MVC ruta. Aplikacija je u potpunosti MVC:
    // pocetna strana ("/") mapira se na Pocetna/Index (nema vise .aspx stranica).
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Pocetna", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
