using System;
using System.Linq;
using System.Web.Mvc;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.ViewModels.Schedule;
using AUPPRB.Web.Controllers.Base;
using System.IO;
using AUPPRB.Domain.Services;
namespace AUPPRB.Web.Controllers.Schedule
{
    //TODO make paginating
    public class ScheduleRenderController : BaseController
    {
        private readonly IScheduleService _scheduleService;
        public ScheduleRenderController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult GetScheduleForUser(SchedualConfiguration data)
        {
            ScheduleViewModel scheduleViewModel;
            if (data.GroupId.HasValue)
                scheduleViewModel = _scheduleService.GenerateScheduleForGroup(data.GroupId.Value, data.DateFrom, data.DateTo);
            else
            {
                var userForId = data.UserId ?? CurrentUserId;
                scheduleViewModel = _scheduleService.GenerateSchedule(userForId, data.DateFrom, data.DateTo);
            }

          
            return Json(new { Schedule = scheduleViewModel });
        }

        public ActionResult GetScheduleInExcellFormatForUser(SchedualConfiguration data)
        {
            var templatePath = Path.Combine(Server.MapPath("~"), "Content/FileTemplates/raspisanie.xlsx");
            byte[] schedualBinaryData;
            if (data.GroupId.HasValue)
                schedualBinaryData = _scheduleService.GetScheduleInExcellFormatForGroup(data.GroupId.Value, data.DateFrom, data.DateTo, templatePath);
            else
            {
                var userForId = data.UserId ?? CurrentUserId;
                schedualBinaryData = _scheduleService.GetScheduleInExcellFormat(userForId, data.DateFrom, data.DateTo, templatePath);
            }

            SetUnblockWindowCookie();
            return File(schedualBinaryData, "application/vnd.ms-excel", "ScheduleAupprb.xlsx");
        }
   
        public ActionResult GetPrepodsForSchedualConfiguration()
        {
            var prepods = _scheduleService.GetPrepodsForSchedualConfiguration();
            return Json(new
            {
                Prepods = prepods.Select(p => new
                {
                    Id = p.Key,
                    FIO = p.Value
                }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSpezialnostiForConfiguration()
        {
            var specialnosti = _scheduleService.GetSpezialnostiForConfiguration();
            return Json(new
            {
                Prepods = specialnosti.Select(p => new
                {
                    Id = p.Key,
                    FIO = p.Value
                }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGodiPostupForConfiguration(int specialostId)
        {
            return Json(new
            {
                Godi = _scheduleService.GetGodiPostupForConfiguration(specialostId)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGroupsForConfiguration(int specialostId, int god)
        {
            var grupps = _scheduleService.GetGroupsForConfiguration(specialostId,god);
            return Json(new
            {
                Groups = grupps.Select(p => new
                {
                    Id = p.Key,
                    Data = p.Value
                }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

       
        public ActionResult AddNotification(string time)
        {
            return View();
        }

   
    }
}