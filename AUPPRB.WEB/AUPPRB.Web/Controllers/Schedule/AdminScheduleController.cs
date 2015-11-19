using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using AUPPRB.Common.Extensions;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.ViewModels.Schedule;

namespace AUPPRB.Web.Controllers.Schedule
{
    public class AdminScheduleController : Controller
    {
        private readonly IAdminScheduleService _adminScheduleService;
        private ISyncronizationService _syncronizationService;
        private IScheduleService _scheduleService;

        public AdminScheduleController(IAdminScheduleService adminScheduleService, IScheduleService scheduleService, ISyncronizationService syncronizationService)
        {
            _adminScheduleService = adminScheduleService;
            _scheduleService = scheduleService;
            _syncronizationService = syncronizationService;
        }


        public ActionResult Index()
        {

            return View("Index");
        }


        [HttpPost]
        public JsonResult GetCalendar(int month, int year)
        {
            var basicCalendar = new BasicCalendar(month, year);
            return Json(new { Calendar = basicCalendar }, JsonRequestBehavior.AllowGet);
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
                Spezialnosti = specialnosti.Select(p => new
                {
                    Id = p.Key,
                    Name = p.Value
                }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// выбрать формы обучения для дропдауна
        /// </summary>
        /// <param name="specialostId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFormsOfStudyForConfiguration(int specialostId)
        {
            Dictionary<int, string> formOfStudyForConfiguration = _scheduleService.GetFormOfStudyForConfiguration(specialostId);
            return Json(new
            {
                FormsOfStudy = formOfStudyForConfiguration.Select(x => new
                {
                    Id = x.Key,
                    Name = x.Value
                }).ToArray()
            }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Выбрать годы поступления для дропдауна 
        /// </summary>
        /// <param name="formOfStudyId"></param>
        /// <param name="spezialnostId"></param>
        /// <returns></returns>
        public ActionResult GetGodiPostupForConfiguration(int formOfStudyId, int spezialnostId)
        {
            return Json(new
            {
                Godi = _scheduleService.GetGodiPostupForConfiguration(formOfStudyId, spezialnostId)
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Отобрать все потоки специальности и данного года
        /// </summary>
        /// <param name="specialostId"></param>
        /// <param name="god"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPotokiForConfiguration(int specialostId, int god)
        {
            Dictionary<int, string> potoki = _adminScheduleService.GetPotoki(specialostId, god);
            return Json(new
            {
                Potoki = potoki.Select(x => new
                {
                    Id = x.Key,
                    Data = x.Value
                }).ToArray()
            }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Настройка базовых параметров для формирования расписания для группы
        /// </summary>
        /// <param name="spezialnostId"></param>
        /// <param name="yearId"></param>
        /// <param name="potokId"></param>
        /// <param name="prepodId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetUpScheduleForADay(int? spezialnostId, int? yearId, int? potokId, int? prepodId, int? formOfStudyId)
        {
            if (prepodId.HasValue)
            {
                KeyValuePair<int, string> prepod = _scheduleService.GetPrepodsForSchedualConfiguration().FirstOrDefault(x => x.Key == prepodId.Value);

                ViewBag.PrepodId = (int)prepod.Key;
                ViewBag.PrepodName = prepod.Value;
                ViewBag.Title = "Расписание для ППС";
                return View("SetScheduleView");
            }

            ViewBag.IdSpezMeta = _adminScheduleService.GetSpezialnostMeta(spezialnostId.Value, year: yearId.Value).Id;

            ViewBag.FormOfStudy = _scheduleService.GetFormOfStudyForConfiguration(spezialnostId.Value).Select(x => x.Value).FirstOrDefault();

            ViewBag.Spezialnost = _adminScheduleService.GetSpezialnosti()
                .Where(x => x.Value == spezialnostId.ToString())
                .Select(x => x.Text)
                .FirstOrDefault();

            ViewBag.YearOfEntering = _scheduleService.GetGodiPostupForConfiguration(formOfStudyId.Value, spezialnostId.Value).Where(x => x == yearId)
                .Select(x => x.ToString())
                .FirstOrDefault();

            KeyValuePair<int, string> potok = _adminScheduleService.GetPotoki(spezialnostId.Value, yearId.Value).FirstOrDefault();
            ViewBag.PotokId = potok.Key;
            ViewBag.PotokName = potok.Value;
            ViewBag.Title = "Расписание для групп";

            return View("SetScheduleView");
        }

        /// <summary>
        /// Отображение расписания на день
        /// </summary>
        /// <param name="spezialnost"></param>
        /// <param name="year"></param>
        /// <param name="potok">1 (1 поток)</param>
        /// <param name="scheduleDate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RenderGroupScheduleForADay(string spezialnost, int year, int potok, string scheduleDate)
        {
            Spezialnost spezialnostByName = _adminScheduleService.GetSpezialnostByName(spezialnost);

            Spezialnost_SpezialnostMeta spezialnostSpezialnostMeta = _adminScheduleService.GetSpezialnostMeta(spezialnostByName.IdSpez, year);

            SpezialRazdeliDisziplini spezialRazdeliDisziplini = _adminScheduleService.GetSpezialRazdeliDisziplini(spezialnostSpezialnostMeta.IdSpez);

            DateTime dateTime = DateTime.Parse(scheduleDate);

            ScheduleDayViewModel raspisanieForADay = _adminScheduleService.GetRaspisanieForADay(dateTime, potok, spezialRazdeliDisziplini.IdUpPlanDisciplini);


            return Json(new { Schedule = raspisanieForADay }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Расписание для препода
        /// </summary>
        /// <param name="prepodId"></param>
        /// <param name="scheduleDate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RenderPrepodScheduleForADay(int prepodId, string scheduleDate)
        {
            DateTime date = DateTime.Parse(scheduleDate);
            ScheduleDayViewModel raspisanieForADay = _adminScheduleService.GetRaspisanieForADay(date, prepodId);

            return Json(new { Schedule = raspisanieForADay }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get dropdown of тип занятия
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTypesOfPara()
        {
            Dictionary<int, string> typesOfPara = _adminScheduleService.GetTypesOfPara();

            return Json(new
            {
                TypesOfPara = typesOfPara.Select(x => new
                    {
                        Id = x.Key,
                        Name = x.Value
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get dropdown of время занятий 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetVremiaZaniatiu()
        {
            Dictionary<int, string> vremiaZaniatiu = _adminScheduleService.GetVremiaZaniatiu();

            return Json(new
            {
                VremiaZaniatiu = vremiaZaniatiu.Select(x => new
                {
                    Id = x.Key,
                    Name = x.Value
                }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get dropdown of disciplines
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDisziplini()
        {
            Dictionary<int, string> disziplini = _adminScheduleService.GetDisziplini();

            return Json(new { Disziplini = disziplini.Select(x => new { Id = x.Key, Name = x.Value }).ToArray() },
                JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Отобрать дисциплины для преподавателя
        /// </summary>
        /// <param name="prepodId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDisciplinesOfPrepod(int prepodId)
        {
            Dictionary<int, string> disciplinesOfPrepod = _adminScheduleService.GetDisciplinesOfPrepod(prepodId);

            return Json(new
            {
                Disziplini = disciplinesOfPrepod.Select(x => new
                {
                    Id = x.Key,
                    Name = x.Value

                })
            }, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Get dropdown of prepods для  конкретной дисциплины
        /// </summary>
        /// <param name="disciplineId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPrepodiDisciplines(int disciplineId)
        {
            Dictionary<int, string> prepodiDisciplines = _adminScheduleService.GetPrepodiDisciplines(disciplineId);

            return Json(new
            {
                PrepodiDisciplines = prepodiDisciplines.Select(x => new
                    {
                        Id = x.Key,
                        Name = x.Value

                    })
            }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public ActionResult GetGroups(int idSpezMeta)
        {
            Dictionary<int, string> groups = _adminScheduleService.GetGroups(idSpezMeta);


            return Json(new
            {
                Groups = groups.Select(x => new
                    {
                        Id = x.Key,
                        Name = x.Value
                    })
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Выбрать все подгруппы
        /// </summary>
        /// <param name="idSpezMeta"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSubGroups(int idSpezMeta)
        {
            Dictionary<int, string> subGroups = _adminScheduleService.GetSubGroups(idSpezMeta);
            return Json(new
            {
                SubGroups = subGroups.Select(x => new
                {
                    Id = x.Key,
                    Name = x.Value
                })
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetSpezMetaId(int spezialnostId, int god)
        {
            Spezialnost_SpezialnostMeta spezialnostSpezialnostMeta =
                _adminScheduleService.GetSpezialnostMeta(spezialnostId, god);

            return Json(new { spezMetaId = spezialnostSpezialnostMeta.Id }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Добавление пары
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPara(ParaViewModel model)
        {
            try
            {
                if (model.FormOfStudyId == 0 && model.SpezialnostId == 0)
                {

                    Raspisanie paraForGroups = _adminScheduleService.AddPara(model);

                    Spezialnost_SpezialnostMeta spezialnostSpezialnostMeta = paraForGroups.SpezialRazdeliDisziplini.Spezialnost_SpezialnostMeta;
                    string spezialnost1 = spezialnostSpezialnostMeta.Spezialnost.Spezialnost1;

                    return Json(new { message = "Пара добавлена успешно", type = "success", spez = spezialnost1, year = spezialnostSpezialnostMeta.GodPostup, potokId = paraForGroups.Pot, date = paraForGroups.Data.ToShortDateString() });
                }
                else
                {

                    Raspisanie paraForPrepod = _adminScheduleService.AddParaForPrepod(model);

                    int prepodId = paraForPrepod.Prepod_PrepodiCafedri.Prepod_PrepodMeta.UserId;

                    return Json(new { message = "Пара добавлена успешно", type = "success", prepodId, date = paraForPrepod.Data.ToShortDateString() });
                }

            }
            catch (Exception ex)
            {
                return Json(new { message = "Произошла ошибка при добавлении: " + ex.Message, type = "error" });
            }

        }

        /// <summary>
        /// Удаление пары
        /// </summary>
        /// <param name="paraId"></param>
        /// <param name="typeOfEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePara(int paraId, string typeOfEntity)
        {
            try
            {

                if (typeOfEntity == "Groups")
                {
                    Raspisanie raspisanie = _adminScheduleService.GetRaspisanie(paraId);

                    Spezialnost_SpezialnostMeta spezialnostSpezialnostMeta = raspisanie.SpezialRazdeliDisziplini.Spezialnost_SpezialnostMeta;
                    string spezialnost1 = spezialnostSpezialnostMeta.Spezialnost.Spezialnost1;

                    _adminScheduleService.DeletePara(paraId);

                    return
                        Json(
                            new
                            {
                                message = "Пара удалена успешно",
                                type = "success",
                                spez = spezialnost1,
                                year = spezialnostSpezialnostMeta.GodPostup,
                                potokId = raspisanie.Pot,
                                date = raspisanie.Data.ToShortDateString()
                            });
                }
                else
                {
                    Raspisanie raspisanie = _adminScheduleService.GetRaspisanie(paraId);

                    int prepodId = raspisanie.Prepod_PrepodiCafedri.Prepod_PrepodMeta.UserId;

                    _adminScheduleService.DeletePara(paraId);

                    return Json(new { message = "Пара удалена успешно", type = "success", prepodId = prepodId, date = raspisanie.Data.ToShortDateString() });
                }


            }
            catch (Exception ex)
            {
                return Json(new { message = "Произошла ошибка при удалении: " + ex.Message, type = "success" });
            }

        }


        /// <summary>
        /// Редактирование пары
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            Raspisanie raspisanie = _adminScheduleService.GetRaspisanie(id);

            //TODO: дописать модель для редактирования
            ParaViewModel paraViewModel = new ParaViewModel()
            {
                Id = raspisanie.Id,
                DateOfPara = raspisanie.Data.ToShortDateString(),
                Auditory = raspisanie.Auditoriya,
                DisciplineId = raspisanie.SpezialRazdeliDisziplini.IdDisciplini,
                TimeOfParaId = raspisanie.IdVremyaZanyatia,


            };

            ViewBag.Id = raspisanie.Id;


            ViewBag.Auditoria = raspisanie.Auditoriya;
            ViewBag.DataZaniat = raspisanie.Data.ToShortDateString();

            ViewBag.VremiaZaniatia = _adminScheduleService.GetVremiaZaniatiu().Select(x => new SelectListItem()
            {
                Selected = raspisanie.IdVremyaZanyatia == x.Key,
                Text = x.Value,
                Value = x.Key.ToString()
            });

            ViewBag.DiszipliniFromSpezialRazdeliDisziplini =
                  _adminScheduleService.GetDiszipliniFromSpezialRazdeliDisziplini(id);


            ViewBag.Prepods = _adminScheduleService.GetAllPrepods().Select(x => new SelectListItem()
             {
                 Selected = raspisanie.Prepod_PrepodiCafedri.Id == x.Key,
                 Text = x.Value,
                 Value = x.Key.ToString()
             });

            ViewBag.Spezialnosti = _scheduleService.GetSpezialnostiForConfiguration().Select(x => new SelectListItem()
            {
                Selected = raspisanie.SpezialRazdeliDisziplini.Spezialnost_SpezialnostMeta.IdSpez == x.Key,
                Text = x.Value,
                Value = x.Key.ToString()
            });

            ViewBag.FormsOfStudy = _scheduleService.GetFormOfStudyForConfiguration(
                raspisanie.SpezialRazdeliDisziplini.Spezialnost_SpezialnostMeta.IdSpez).Select(x => new SelectListItem()
                {
                    Selected = raspisanie.SpezialRazdeliDisziplini.Spezialnost_SpezialnostMeta.IdFrmObuch == x.Key,
                    Text = x.Value,
                    Value = x.Key.ToString()
                });



            ViewBag.GodiPostuplenia = _adminScheduleService.GetGodPostuplinia(
                 raspisanie.SpezialRazdeliDisziplini.Spezialnost_SpezialnostMeta.IdSpez).Select(x => new SelectListItem()
                 {
                     Selected =
                         raspisanie.SpezialRazdeliDisziplini.Spezialnost_SpezialnostMeta.GodPostup ==
                         Int32.Parse(x.Value),
                     Text = x.Text,
                     Value = x.Value
                 });

            ViewBag.Potoki = _adminScheduleService.GetPotoki(raspisanie.SpezialRazdeliDisziplini.Spezialnost_SpezialnostMeta.IdSpez,
                 raspisanie.SpezialRazdeliDisziplini.Spezialnost_SpezialnostMeta.GodPostup)
                 .Select(x => new SelectListItem()
                 {
                     Selected = raspisanie.Pot == x.Key,
                     Text = x.Value,
                     Value = x.Key.ToString()
                 });

            ViewBag.TypesOfZaniatia
                  = _adminScheduleService.GetTypesOfPara().Select(x => new SelectListItem()
              {
                  Selected = raspisanie.IdVidaRaboti == x.Key,
                  Text = x.Value,
                  Value = x.Key.ToString()
              });


            if (raspisanie.IdSpiskaGrupp != null)
            {
                ViewBag.Groups = _adminScheduleService.GetGroups(raspisanie.SpezialRazdeliDisziplini.IdSpezMeta)
              .Select(x => new SelectListItem()
              {
                  Selected = raspisanie.IdSpiskaGrupp == x.Key,
                  Text = x.Value,
                  Value = x.Key.ToString()
              });

                ViewBag.SubGroups = _adminScheduleService.GetSubGroups(raspisanie.SpezialRazdeliDisziplini.IdSpezMeta)
                    .Select(x => new SelectListItem()
                    {
                        Selected = raspisanie.NomerPodGrupp == x.Key,
                        Text = x.Value,
                        Value = x.Key.ToString()
                    });

            }


            return View("Edit", paraViewModel);
        }



        [HttpPost]
        public ActionResult Save(ParaViewModel model)
        {
            //TODO: тестирование соединения с БД Белодеда
            string connStringOleDb = ConfigurationManager.AppSettings["OleDbConnString"];
            List<string> dataFromT0005ВремяЗанятия = _syncronizationService.GetDataFrom_t0005_ВремяЗанятия(@connStringOleDb);

            return View();
        }
    }

}