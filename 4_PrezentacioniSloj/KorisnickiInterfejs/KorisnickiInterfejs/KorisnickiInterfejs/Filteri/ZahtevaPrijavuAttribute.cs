using System.Web.Mvc;
using System.Web.Routing;

namespace KorisnickiInterfejs.Filteri
{
    // MVC AKCIONI FILTER - zahteva da je korisnik prijavljen (postoji uloga u sesiji).
    // Ako nije, preusmerava na stranicu za prijavu.
    public class ZahtevaPrijavuAttribute : ActionFilterAttribute
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

            base.OnActionExecuting(filterContext);
        }
    }
}
