using System.Web;
using AUPPRB.Common.Extensions;

namespace AUPPRB.Common.Utils.Web
{
    public class MvcHelper
    {
        /// <summary>
        /// Current area
        /// </summary>
        public string CurrentArea
        {
            get
            {
                var dataTokens = HttpContext.Current.Request.RequestContext.RouteData.DataTokens;
                return dataTokens["area"] != null ? dataTokens["area"].ToString().ToLower() : "";
            }
        }
        /// <summary>
        /// Current controller
        /// </summary>
        public string CurrentController
        {
            get
            {
                var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
                return routeValues["controller"] != null ? routeValues["controller"].ToString().ToLower() : "";
            }
        }
        /// <summary>
        /// Current action
        /// </summary>
        public string CurrentAction
        {
            get
            {
                var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
                return routeValues["action"] != null ? routeValues["action"].ToString().ToLower() : "";
            }
        }

        public static int CurrentUserId
        {
            get
            {
                return int.Parse((HttpContext.Current.Session["UserId"]!=null)?HttpContext.Current.Session["UserId"].ToStringWithValue():"0");
            }
            set
            {
                HttpContext.Current.Session["UserId"] = value;
            }
        }

        public static string CurrentUserLogin
        {
            get
            {
                return HttpContext.Current.Session["Login"].ToStringWithValue();
            }
            set
            {
                HttpContext.Current.Session["Login"] = value;
            }
        }
    }
}
