using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AUPPRB.Common.Enums;
using AUPPRB.Common.Utils.Caching;
using AUPPRB.Common.Utils.Web;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.ViewModels.Dictionary;
using AUPPRB.Models.ViewModels.UsersModels;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace AUPPRB.Web.Controllers.Base
{
    public class BaseController : Controller
    {
        protected readonly ICache CacheService;
        protected readonly IAccountService _accountService;
        protected readonly IDictionaryService _dictionaryService;


        public BaseController()
        {
            _accountService = DependencyResolver.Current.GetService<IAccountService>();
            _dictionaryService = DependencyResolver.Current.GetService<IDictionaryService>();
            CacheService = CacheProvider.Current.GetCache();
        }

        private void _continueCoockie()
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return;
            authCookie.Expires = DateTime.Now.Add(FormsAuthentication.Timeout);
            Response.SetCookie(authCookie);
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated && CurrentUserId == 0)
            {
                var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null)
                    {
                        var userData = JsonConvert.DeserializeObject<ClientUser>(ticket.UserData);
                        //if (_accountService.ValidateUser(userData.Id).IsValid)
                        //{
                        CurrentUserId = userData.Id;
                        CurrentUserLogin = userData.Login;
                        _continueCoockie();
                        base.OnActionExecuting(filterContext);
                        return;
                        //}
                    }
                }
                FormsAuthentication.SignOut();
                Session.RemoveAll();
                filterContext.Result = new RedirectResult(Url.Action("Login", "Home", new { area = "" }));
                return;
            }


            _continueCoockie();
            base.OnActionExecuting(filterContext);

        }
        //TODO:тут этого не должно быть
        protected List<User> GetAllUsers()
        {
            return _accountService.GetAllUsers();
        }
        //TODO:тут этого не должно быть
        protected bool AddUser(User user)
        {

            return _accountService.AddUser(user);
        }
        //TODO:тут этого не должно быть
        protected bool UpdateUser(User user)
        {
            return _accountService.UpdateUser(user);
        }
        //TODO:тут этого не должно быть
        protected bool DeleteUser(User user)
        {
            return _accountService.DeleteUser(user);
        }


        protected User GetUserById(int id)
        {
            return _accountService.GetUser(id);
        }

        protected User GetUserByLogin(string login)
        {
            return _accountService.GetUser(login);
        }

        //TODO:тут этого не должно быть
        protected bool AddUserTask(int userId, int taskId)
        {
            return _accountService.AddUserTask(userId, taskId);
        }



        protected int CurrentUserId
        {
            get { return MvcHelper.CurrentUserId; }
            set { MvcHelper.CurrentUserId = value; }
        }


        protected string CurrentUserLogin
        {
            get { return MvcHelper.CurrentUserLogin; }
            set { MvcHelper.CurrentUserLogin = value; }
        }

        protected User CurrentUser
        {
            get
            {
                try
                {
                    User user = _accountService.GetUser(CurrentUserId);
                    return user;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        protected bool IsTasksAllowedForUser(params TaskEnum[] taskEnums)
        {
            return _accountService.IsTasksAllowedForUser(CurrentUserId, taskEnums);
        }

        protected bool IsCurrentUserInRole(params RoleEnum[] taskEnums)
        {
            return _accountService.IsUserInRoles(CurrentUserId, taskEnums);
        }

        public List<Role> GetAllRoles()
        {
            return _accountService.GetAllRoles();
        }

        public List<DictionaryItemViewModel> GetItemsOfDictionary(DictionaryTypeEnum type)
        {
            return _dictionaryService.GetItemsOfDictionary(type);
        }

        public List<Task> GetTasksByRole(RoleEnum role)
        {

            return _accountService.GetTasksByRole(role);
        }

        public Task GetTaskById(int id)
        {
            return _accountService.GetTaskById(id);
        }

        //TODO:тут этого не должно быть
        //TODO:Какая то хрень так не делается
        protected bool DB_SaveChanges()
        {
            return _accountService.SaveChanges();
        }

        protected void SetUnblockWindowCookie()
        {
            var cookie = new HttpCookie("fileDownload") { Value = "true" };
            Response.AppendCookie(cookie);
        }

        protected void UpdateCafedry(IEnumerable<Prepod_PrepodiCafedri> caf )
        {
            _accountService.AddCafedry(caf);
        }
    }
}
