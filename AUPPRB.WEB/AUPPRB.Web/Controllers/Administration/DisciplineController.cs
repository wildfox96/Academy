using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.OtherModels.ServiceModels;
using AUPPRB.Models.ViewModels.Discipline;
using AUPPRB.Models.ViewModels.jqGridModels;
using Trirand.Web.Mvc;

namespace AUPPRB.Web.Controllers.Administration
{
    public class DisciplineController : Controller
    {
        private IDisciplineService _disciplineService;
        public DisciplineController(IDisciplineService disciplineService)
        {
            _disciplineService = disciplineService;
        }

        //
        // GET: /Discipline/
        public ActionResult Index()
        {
            DisciplineJqGridModel gridModel = new DisciplineJqGridModel();

            SetUpGrid(gridModel.DisciplineJqGrid);

            return View(gridModel);
        }


        private void SetUpGrid(JQGrid jqGrid)
        {
            jqGrid.DataUrl = Url.Action("GridDataRequested");
            jqGrid.ClientSideEvents.GridInitialized = "gridInitialized";

        }


        public JsonResult GridDataRequested()
        {
            try
            {
                DisciplineJqGridModel disciplineJqGridModel = new DisciplineJqGridModel();

                SetUpGrid(disciplineJqGridModel.DisciplineJqGrid);

                IQueryable<Discipline> allDisciplines = _disciplineService.GetAllDisciplines();


                return disciplineJqGridModel.DisciplineJqGrid.DataBind(allDisciplines.Select(x => new
                {
                    x.Id,
                    FullName = x.Name,
                    ShortName = x.ShortName,
                    IsActive = x.IsActive
                }));

            }
            catch (Exception ex)
            {

                return Json(null, JsonRequestBehavior.AllowGet);

            }
        }

        /// <summary>
        /// Create new discipline
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return RedirectToAction("Edit", "Discipline", new { id = 0 });
        }


        /// <summary>
        /// Edit discipline
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Discipline discipline = _disciplineService.GetDisciplineById(id) ?? new Discipline();

            return View(discipline);
        }


        /// <summary>
        /// Save discipline
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Discipline discipline)
        {
            if (!String.IsNullOrEmpty(discipline.Name) && !String.IsNullOrEmpty(discipline.ShortName))
            {
                List<Discipline> allDisciolines = _disciplineService.GetAllDisciplines().ToList();

                if (discipline.Id == 0)
                {
                    if (allDisciolines.Any(x => x.Name.Replace(" ", "").Trim().ToUpperInvariant() == discipline.Name.Replace(" ", "").Trim().ToUpperInvariant()))
                    {
                        return Json(new { type = "error", message = "Такая дисциплина уже существует" }, JsonRequestBehavior.AllowGet);
                    }

                    RequestResult saveDiscipline = _disciplineService.SaveDiscipline(discipline);
                    return Json(saveDiscipline.Equals(RequestResult.Ok) ?
                        new { type = "success", message = "Дисциплина успешно сохранена" } : new { type = "error", message = saveDiscipline.RequestMessage });
                }
                else
                {
                    if (
                        allDisciolines.Any(
                            x =>
                                x.Id != discipline.Id &&
                                x.Name.Replace(" ", "").Trim().ToUpperInvariant() ==
                                discipline.Name.Replace(" ", "").Trim().ToUpperInvariant()))
                    {
                        return Json(new { type = "error", message = "Такая дисциплинв уже существует" }, JsonRequestBehavior.AllowGet);
                    }

                    RequestResult updateDiscipline = _disciplineService.UpdateDiscipline(discipline);
                    return Json(updateDiscipline.Equals(RequestResult.Ok) ?
                           new { type = "success", message = "Дисциплина успешно обновлена" } : new { type = "error", message = updateDiscipline.RequestMessage });
                }
            }
            else
            {
                return Json(new { type = "error", message = "Произошла ошибка при сохранении изменений: не заполнены обязательные поля" });
            }
        }

    }
}