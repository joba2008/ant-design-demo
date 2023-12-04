using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCSUI.MCSWebServiceReference;

namespace MCSUI
{
    public class ServiceHelper
    {
        public static MCSWebServiceSoap GetService()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.ConnectionStrings["WEBSERVICE_URL"].ConnectionString;
                MCSWebServiceSoapClient mcswebservicesoapclient = new MCSWebServiceSoapClient("MCSWebServiceSoap", url); 
                return mcswebservicesoapclient;
            }
            catch
            {
                return null;
            }
        }
    }
}
