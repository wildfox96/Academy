using System;
using System.Linq;
using System.Web.Mvc;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.ViewModels.jqGridModels;
using AUPPRB.Web.Controllers.Base;
using Trirand.Web.Mvc;

namespace AUPPRB.Web.Controllers.Literature
{
    public class UserLiteratureController : BaseController
    {
         private readonly ILiteratureService _literatureService;
         public UserLiteratureController(ILiteratureService literatureService)
        {
            _literatureService = literatureService;
        }
         public ActionResult Index()
         {
             var gridModel = new ReservateBookMetaJqGridModel();
             SetUpBookGrid(gridModel.BookJqGridModelGrid);
             return View(gridModel);
         }

         private void SetUpBookGrid(JQGrid grid)
         {
             grid.DataUrl = Url.Action("BookGridDataRequested");
         }

         public JsonResult BookGridDataRequested()
         {
             try
             {
                 var gridModel = new ReservateBookMetaJqGridModel();
                 SetUpBookGrid(gridModel.BookJqGridModelGrid);
                 var books = _literatureService.GetUserBooksMetas(CurrentUserId).AsQueryable();

                 return gridModel.BookJqGridModelGrid.DataBind(books);
             }
             catch (Exception ex)
             {
                 return Json(new { }, JsonRequestBehavior.AllowGet);
             }
         }

        public ActionResult View(int id)
        {
            var book = _literatureService.GetBook(id);
            return View(book);
        }
    }
}