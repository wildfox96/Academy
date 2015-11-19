using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.ViewModels.UsersModels;
using Newtonsoft.Json;

namespace AUPPRB.Web.Controllers.ExpertCenter
{
    public class ExpertCenterController : Controller
    {
        private readonly IExpertCenterService IExpertCenterService;
        private readonly ILikesService ILikesService;
        public ExpertCenterController(IExpertCenterService _IExpertCenterService, ILikesService _ILikesService)
        {
            IExpertCenterService = _IExpertCenterService;
            ILikesService = _ILikesService;
        }

        public ActionResult Prep(int page = 1)
        {
            return PartialView(IExpertCenterService.PrepodModel(page));
        }

        public ActionResult Disp(int page = 1)
        {
            return PartialView(IExpertCenterService.DispModel(page));
        }

        public ActionResult Search(string find)
        {
            return PartialView(IExpertCenterService.SeacrhModel(find));
        }

        public ActionResult Index()
        {
            return View(IExpertCenterService.StartPageModel());
        }

        public JsonResult AddLike(int idSotr)
        {
            try
            {
                int userId =
              JsonConvert.DeserializeObject<ClientUser>(
                  FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).UserData).Id;

                if (ILikesService.IsUserAddLike(userId, idSotr))
                {
                    Prepod_PrepodLikes prepodLike = new Prepod_PrepodLikes()
                         {
                             IdSotr = idSotr,
                             IdUser = userId
                         };

                    ILikesService.AddLike(prepodLike);
                    int count = ILikesService.GetPrepodLikesCount(idSotr);
                    return Json(new { type = "success", count = count });
                }

                return Json(new { type = "error", message = "Вы уже ставили лайк" });

            }
            catch (Exception ex)
            {
                return Json(new { type = "error", message = "Ошибка при сохранении данных. " + ex.Message });
            }
        }
    }
}