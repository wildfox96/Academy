using System.Linq;
using System.Web.Mvc;
using AUPPRB.Common.Enums;
using AUPPRB.Common.Utils.Web;
using AUPPRB.Domain.Interfaces;

namespace AUPPRB.Domain.Authorization
{
    public class TasksAuthorizeAttribute:AuthorizeAttribute
    {
        private readonly TaskEnum[] _tasks;
        private readonly IAccountService _accountService;

        public TasksAuthorizeAttribute(params TaskEnum[] tasks)
        {
            _tasks = tasks;
            _accountService = DependencyResolver.Current.GetService<IAccountService>();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var isTasksAuthorised = false;

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (filterContext.HttpContext != null && MvcHelper.CurrentUserId != 0)
                {
                    isTasksAuthorised = _accountService.IsTasksAllowedForUser(MvcHelper.CurrentUserId,_tasks);
                }
            }

            if (isTasksAuthorised || (_tasks != null && !_tasks.Any()))
                base.OnAuthorization(filterContext);
            else
                filterContext.Result = new RedirectResult("..........");
        }
    }
}
