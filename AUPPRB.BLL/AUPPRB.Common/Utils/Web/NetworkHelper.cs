using System;
using System.Web;

namespace AUPPRB.Common.Utils.Web
{
    public static class NetworkHelper
    {
        public static string CurrentIP
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.UserHostAddress;
                }
                catch (Exception)
                {
                    return "";
                }
            }

        }

        public static string CurrentHostName
        {
            get
            {
                try
                {
                    return GetHostNameByHostAddress(CurrentIP);
                }
                catch (Exception)
                {
                    return "";
                }
            }

        }

        public static String GetHostNameByHostAddress(String hostAddress)
        {
            try
            {
                if (HttpContext.Current.Session["CurrentHostName"] == null)
                {
                    var hostName = System.Net.Dns.GetHostEntry(hostAddress).HostName;
                    HttpContext.Current.Session["CurrentHostName"] = hostName;
                }

                return HttpContext.Current.Session["CurrentHostName"].ToString();
            }
            catch (Exception)
            {
                HttpContext.Current.Session["CurrentHostName"] = String.Empty;
                return "";
            }
        }
    }
}
