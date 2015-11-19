using System;
using System.Linq;
using System.Web.Mvc;
using AUPPRB.Models.OtherModels.ServiceModels;
using AUPPRB.Models.ViewModels.Dictionary;
using AUPPRB.Models.ViewModels.jqGridModels;
using AUPPRB.Web.Controllers.Base;
using Trirand.Web.Mvc;

namespace AUPPRB.Web.Controllers.Administration
{
    public class DictionaryController : BaseController
    {
        public ActionResult Index()
        {
            var dictionaryJqGridModel = new DictionaryJqGridModel();
            SetUpDictionaryGrid(dictionaryJqGridModel.DictionaryGrid);
            return View(dictionaryJqGridModel);
        }

        public ActionResult Edit(int id)
        {
            var dictionary = _dictionaryService.GetDictionary(id);// id.HasValue ? _dictionaryService.GetDictionary(id.Value) : new DictionaryViewModel();
            return View(dictionary);
        }

        public ActionResult AddDictionaryItem(int dictType)
        {
            return PartialView("_DictionaryEditorRow", new DictionaryItemViewModel() { DictionaryType = dictType });
        }

        public ActionResult Save(DictionaryViewModel dictionary)
        {
            var rs = Request.IsAjaxRequest();
            var updateResult = _dictionaryService.UpdateDictionary(dictionary);
            return Json(updateResult.Equals(RequestResult.Ok) ? new { type = "success", message = "Справочник успешно сохранен" } : new { type = "error", message = updateResult.RequestMessage });
        }

        private void SetUpDictionaryGrid(JQGrid grid)
        {
            grid.DataUrl = Url.Action("DictionaryGridDataRequested");
        }

        public JsonResult DictionaryGridDataRequested()
        {
            try
            {
                var gridModel = new DictionaryJqGridModel();
                SetUpDictionaryGrid(gridModel.DictionaryGrid);
                var dictionaries = _dictionaryService.GetAllDictionariesMeta().AsQueryable();

                return gridModel.DictionaryGrid.DataBind(dictionaries);
            }
            catch (Exception ex)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
