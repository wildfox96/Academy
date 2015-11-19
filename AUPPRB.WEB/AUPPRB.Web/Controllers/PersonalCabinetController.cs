using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AUPPRB.Web.Controllers
{
    public class PersonalCabinetController : Controller
    {
        //
        // GET: /PersonalCabinet/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Основная инфа о студенте и его фотка
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentInformation()
        {

            return PartialView();

        }


    }
}
