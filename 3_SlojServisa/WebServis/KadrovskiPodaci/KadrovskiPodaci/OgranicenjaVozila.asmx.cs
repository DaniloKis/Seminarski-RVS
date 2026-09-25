using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
//
using System.Data;

namespace OgranicenjaServis
{
    /// <summary>
    /// Web servis kurirske sluzbe - daje granice tezine po tipu vozila.
    /// (analogno servisu koji je davao ogranicenja sistematizacije radnih mesta)
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // [System.Web.Script.Services.ScriptService]
    public class OgranicenjaVozila : System.Web.Services.WebService
    {

        [WebMethod]
        public DataSet DajSvaOgranicenja()
        {
            DataSet dsOgranicenja = new DataSet();
            dsOgranicenja.ReadXml(Server.MapPath("~/") + "XML/OgranicenjaVozila.XML");

            return dsOgranicenja;
        }


        [WebMethod]
        public int DajGranicuTezine(string pomSifraVozila)
        {
            int GranicaTezine = 0;
            DataSet dsOgranicenja = new DataSet();
            dsOgranicenja.ReadXml(Server.MapPath("~/") + "XML/OgranicenjaVozila.XML");
            // filtriranje dataset-a
            DataRow[] result = dsOgranicenja.Tables[0].Select("SifraVozila='" + pomSifraVozila + "'");
            GranicaTezine = int.Parse(result[0].ItemArray[1].ToString());

            return GranicaTezine;
        }


    }
}
