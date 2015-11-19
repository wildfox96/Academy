using System.IO;
using System.Web.Mvc;

namespace AUPPRB.Models.OtherModels.ServiceModels
{
    public static class RenderView
    {
        public static string RenderViewToString(ControllerContext context, string viewPath, object model = null)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }
    }
}
