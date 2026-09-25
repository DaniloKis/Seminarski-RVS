using System.Web.Mvc;
using System.Web.Routing;

namespace KorisnickiInterfejs.Filteri
{
    // MVC AKCIONI FILTER - dozvoljava pristup samo administratoru.
    // Kurir se preusmerava na unos paketa (jedina njegova funkcija),
    // a neprijavljeni korisnik na stranicu za prijavu.
    public class SamoAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sesija = filterContext.HttpContext.Session;
            string uloga = (sesija != null) ? sesija["Uloga"] as string : null;

            if (string.IsNullOrEmpty(uloga))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Nalog", action = "Prijava" }));
            }
            else if (uloga != "Admin")
            {
                // kurir - samo unos paketa
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "PaketMvc", action = "Unos" }));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
