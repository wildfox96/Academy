using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.ViewModels.jqGridModels;
using AUPPRB.Web.Controllers.Base;
using Trirand.Web.Mvc;

namespace AUPPRB.Web.Controllers.Literature
{
    public class AcademyLiteratureController : BaseController
    {
        private readonly ILiteratureService _literatureService;
        public AcademyLiteratureController(ILiteratureService literatureService)
        {
            _literatureService = literatureService;
        }

        public ActionResult Index()
        {
            var gridModel = new LiteratureMetaJqGridModel();
            SetUpLiteratureGrid(gridModel.LiteratureGrid);
            return View(gridModel);
        }

        private void SetUpLiteratureGrid(JQGrid grid)
        {
            grid.DataUrl = Url.Action("LiteratureGridDataRequested");
        }

        public JsonResult LiteratureGridDataRequested()
        {
            try
            {
                var gridModel = new LiteratureMetaJqGridModel();
                SetUpLiteratureGrid(gridModel.LiteratureGrid);
                var dictionaries = _literatureService.GetLiteratureMetas().AsQueryable();

                return gridModel.LiteratureGrid.DataBind(dictionaries);
            }
            catch (Exception ex)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult View(int id)
        {
            var book = _literatureService.GetLiterature(id);
            return View(book);
        }
    }
}