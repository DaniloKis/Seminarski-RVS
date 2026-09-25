using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
//
using DBUtils;
using System.Configuration;
//
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace KorisnickiInterfejs
{
    public class Global : System.Web.HttpApplication
    {
        // ATRIBUTI - GLOBALNE PROMENLJIVE ZA CELU APLIKACIJU
        public static KonekcijaKlasa otvorenaKonekcija;
        public static bool uspehKonekcije;

        // PROCEDURE
        public static bool OtvoriKonekcijuDoBazePodataka()
        
        {
             // OCITAVANJE PARAMETARA KONEKCIJE
            string stringKonekcije = ConfigurationManager.ConnectionStrings["NasaKonekcija"].ConnectionString;


            // KONEKTOVANJE NA BAZU PODATAKA
            otvorenaKonekcija = new KonekcijaKlasa(stringKonekcije);
            uspehKonekcije = otvorenaKonekcija.OtvoriKonekciju();

            // VRACANJE REZULTATA KONEKCIJE
            return uspehKonekcije;
        }


        public void ZatvoriKonekciju()
        {
            otvorenaKonekcija.ZatvoriKonekciju();
        }

        // DOGADJAJI - GLOBALNI ZA CELU APLIKACIJU
        void Application_Start(object sender, EventArgs e)
        {
            uspehKonekcije = OtvoriKonekcijuDoBazePodataka();

            // REGISTRACIJA REST SERVISA (Web API 2) - mora pre MVC ruta
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // DEPENDENCY INJECTION - kontroler dobija zavisnost preko interfejsa
            DependencyResolver.SetResolver(new KurirskiResolver());

            // REGISTRACIJA MVC RUTA
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Application_End(object sender, EventArgs e)
        {
            ZatvoriKonekciju();

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
