using System;
using System.Linq;
using System.Web.Mvc;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.OtherModels.ServiceModels;
using AUPPRB.Models.ViewModels.jqGridModels;
using AUPPRB.Models.ViewModels.Literature;
using AUPPRB.Web.Controllers.Base;
using Newtonsoft.Json;
using Trirand.Web.Mvc;

namespace AUPPRB.Web.Controllers.Literature
{
    public class LiteratureManageController : BaseController
    {
        private readonly ILiteratureService _literatureService;
        public LiteratureManageController(ILiteratureService literatureService)
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

        public ActionResult Edit(int? id)
        {

            var literature = id.HasValue
                                   ? _literatureService.GetLiterature(id.Value)
                                   : new LiteratureViewModel();
            return View(literature);
        }
        [HttpPost]
        public ActionResult Save(LiteratureViewModel model)
        {
            
            var updateResult = _literatureService.EditLiterature(model);
            return Json(updateResult.Equals(RequestResult.Ok) ? new { type = "success", message = "Литература успешно сохранена" } : new { type = "error", message = updateResult.RequestMessage });
           
        }

        public ActionResult Delete(int id)
        {
            var updateResult = _literatureService.DeleteLiterature(id);
            return Json(updateResult.Equals(RequestResult.Ok) ? new { type = "success", message = "Литература успешно удалена" } : new { type = "error", message = updateResult.RequestMessage });

        }

        public ActionResult AddBookItem()
        {
            return PartialView("_BookEditorRow", new BookOfLiteratureViewModel());
        }

        public ActionResult DeleteBook(int id)
        {
            
            var requestResult = _literatureService.DeleteBook(id);
            return Json(new {Success = requestResult.RequestCode == 0, Message = requestResult.RequestMessage},
                JsonRequestBehavior.AllowGet);
        }

      
        public ActionResult ReservateBook(int? id)
        {
            var model = new ReservateBookViewModel()
            {
                SelectedLiterature = id,
                BookNumbers = _literatureService.GetFreeBookNumbers(id),
                LiteratureNames = _literatureService.GetLiteratureList(),
                UsersGrid = new UserForReservateLiteratureJqGridModel()
            };
            SetUpUsersGrid(model.UsersGrid.UsersGrid);
            
            return View(model);
        }
     
        [HttpPost]
        public ActionResult ReservateBook(int bookId, int userId)
        {
            var updateResult = _literatureService.ReservateBook(bookId, userId, CurrentUserId);
            return Json(updateResult.Equals(RequestResult.Ok) ? new { type = "success", message = "Книга успешно записана на пользователя" } : new { type = "error", message = updateResult.RequestMessage });
        }

        public ActionResult GetReservatorInfo(string bookNumber)
        {
            var data = _literatureService.GetReservatorInfo(bookNumber);
            return Json(new { info = data, success = data !=null});
        }

        private void SetUpUsersGrid(JQGrid grid)
        {
            grid.DataUrl = Url.Action("UsersGridDataRequested");
        }

        public JsonResult UsersGridDataRequested()
        {
            try
            {
                var gridModel = new UserForReservateLiteratureJqGridModel();
                SetUpLiteratureGrid(gridModel.UsersGrid);
                var dictionaries = _literatureService.GetUsersForReservate().AsQueryable();

                return gridModel.UsersGrid.DataBind(dictionaries);
            }
            catch (Exception ex)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ReturnBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReturnBook(string number)
        {
            var updateResult = _literatureService.ReturnBook(number);
            return Json(updateResult.Equals(RequestResult.Ok) ? new { type = "success", message = "Книга успешно списана" } : new { type = "error", message = updateResult.RequestMessage });
        }

        public ActionResult GetReservedBookNumbers()
        {
            var list = _accountService.GetReservateBookNumbers();
            return Json(list,JsonRequestBehavior.AllowGet);
        }

      
          [HttpPost]
        public ActionResult GetBookNumbers(int id)
        {
            try
            {
                var dataList = _literatureService.GetFreeBookNumbers(id).Select(c => new SelectListItem { Value = c.Key.ToString(), Text = c.Value }).OrderBy(c => c.Text).ToList();
                return Json(new SelectList(dataList, "Value", "Text"));
            }
            catch (Exception)
            {
                return Json(new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text"));
            }
        }

          public ActionResult UserDetails(int id)
          {
              return RedirectToAction("HomeUserPage", "Home", new { isPartial = true, userId = id });
          }
    }
}