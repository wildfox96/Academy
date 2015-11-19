using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.ViewModels.Notifications;
using AUPPRB.Web.Controllers.Base;

namespace AUPPRB.Web.Controllers
{
    public class NotificationController : BaseController
    {
        private  readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public ActionResult EditNotification(int? id)
        {
            var notification = id.HasValue
                                    ? _notificationService.GetNotification(id.Value)
                                    : new NotificationViewModel()
                                       {
                                           NotificationDate = DateTime.Now.ToShortDateString()
                                       };
            
        
            return View("EditNotificationModal",notification);
        }

        [HttpPost]
        public ActionResult EditNotification(NotificationViewModel notice)
        {
           // return Json(new { success = true, callbackFunction = "GenerateSchedule" });
            //return null;

            try
            {
                var result = notice.NotificationId == 0
                       ? _notificationService.AddNotice(CurrentUserId, notice)
                       : _notificationService.EditNotice(notice);
                return Json(new { success = true, callbackFunction = "GenerateSchedule" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
           

        }

        [HttpPost]
        public ActionResult DeleteNotification(int id)
        {
            try
            {
                 _notificationService.DeleteNotification(CurrentUserId, id);
                return Json(new { success = true, callbackFunction = "GenerateSchedule" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }


        }

        [HttpPost]
        public ActionResult GetNotificationTypes()
        {
            try
            {
                var dataList = _notificationService.GetNotificationTypes().Select(c => new SelectListItem { Value = c.Key.ToString(), Text = c.Value }).OrderBy(c => c.Text).ToList();
                return Json(new SelectList(dataList, "Value", "Text"));
            }
            catch (Exception)
            {
                return Json(new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text"));
            }
        }

        [HttpPost]
        public ActionResult GetNotificationTimes()
        {
            try
            {
                var dataList = _notificationService.GetNotificationTimes().Select(c => new SelectListItem { Value = c.Key.ToString(), Text = c.Value }).ToList();
                return Json(new SelectList(dataList, "Value", "Text"));
            }
            catch (Exception)
            {
                return Json(new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text"));
            }
        }

        [HttpPost]
        public ActionResult GetNotification(int id)
        {
            try
            {
                var notification = _notificationService.GetNotification(id);
                return Json(new {notification});
            }
            catch (Exception)
            {
                return Json(null);
            }
        }

       
    }
}