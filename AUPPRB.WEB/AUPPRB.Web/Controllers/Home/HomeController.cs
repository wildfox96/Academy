using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AUPPRB.Common.Enums;
using AUPPRB.Common.Utils.Web;
using AUPPRB.Domain.Authorization;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.ViewModels;
using AUPPRB.Models.ViewModels.UsersModels;
using AUPPRB.Web.Controllers.Base;
using Newtonsoft.Json;
using Trirand.Web.Mvc;

namespace AUPPRB.Web.Controllers.Home
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null)
                    {
                        var userData = JsonConvert.DeserializeObject<ClientUser>(ticket.UserData);

                        CurrentUserLogin = userData.Login;
                        return RedirectToAction("HomeUserPage", "Home", new { login = CurrentUserLogin });

                    }
                    else
                    {
                        return View("Login");
                    }
                }
                else
                {
                    return View("Login");
                }

            }
            else
            {
                return View("Login");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [ValidateInput(false)]
        //[TasksAuthorize(TaskEnum.Администраторская_ДобавлениеПользователя,)]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            //return RedirectToAction("HomeUserPage", "Home");
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(loginViewModel.Login, loginViewModel.Password))
                {
                    loginViewModel.RememberMe = true;
                    // Create the authentication ticket with custom user data.
                    var userData = JsonConvert.SerializeObject(new ClientUser { Id = MvcHelper.CurrentUserId, Login = MvcHelper.CurrentUserLogin });

                    var ticket = new FormsAuthenticationTicket(
                        1,
                        loginViewModel.Login,
                        DateTime.Now,
                        DateTime.Now + FormsAuthentication.Timeout,
                        loginViewModel.RememberMe,
                        userData,
                        FormsAuthentication.FormsCookiePath);

                    // Encrypt the ticket.
                    var encTicket = FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket) { Expires = ticket.Expiration });

                    return RedirectToAction("HomeUserPage", "Home");


                }
                else
                {
                    ModelState.AddModelError("", "Неверные учетные данные.");
                }



            }
            // If we got this far, something failed, redisplay form
            return View(loginViewModel);


        }

        public ActionResult HomeUserPage(bool? isPartial, int? userId)
        {
            var id = userId.HasValue ? userId.Value : CurrentUserId;

            var user = _accountService.GetUser(id);
            if (user == null)
            {
                return View("Login");
            }
            else
            {

                UserCommonViewModel model;
                var userMeta = user.UserMeta.FirstOrDefault();

                var studentStudentMeta = user.Student_StudentMeta.FirstOrDefault();
                string numberTasks = user.UserTask.Aggregate("", (current, task) => current + (task.TaskId + ";"));

                if (studentStudentMeta != null)
                {
                    model = new UserCommonViewModel()
                    {
                        //IsNewUser = false,
                        StudentMeta = new StudentViewModel()
                        {
                            AdmissionDate = studentStudentMeta.AdmissionDate.Year,
                            FacultyId = studentStudentMeta.FacultyId,
                            //FlowId = studentStudentMeta.FlowId,
                            GraduationDate = studentStudentMeta.GraduationDate.Year,
                            MarkBookNumber = studentStudentMeta.MarkBookNumber,
                            //GroupId = studentStudentMeta.GroupId,
                            IsDismissed = studentStudentMeta.IsDismissed,
                            //SpecialityId = studentStudentMeta.SpecialtyId,
                            StudentCardNumber = studentStudentMeta.StudentCardNumber
                        },
                        Login = user.Login,
                        //Password = user.Password,
                        FirstName = userMeta.FirstName,
                        LastName = userMeta.LastName,
                        MiddleName = userMeta.MiddleName,
                        IsBlocked = user.BlockDate != null,
                        Phone = userMeta.Contact.PhoneNumber,
                        AdditionalPhone = userMeta.Contact.AdditionalPhoneNumber,
                        Email = userMeta.Contact.Email,
                        Skype = userMeta.Contact.Skype,
                        Tasks = numberTasks


                    };
                    ViewBag.FacultyName =
                        GetItemsOfDictionary(DictionaryTypeEnum.Факультеты)
                            .Find(p => p.Id == studentStudentMeta.FacultyId)
                            .Name;
                }
                else
                {
                    var prepodPrepodMeta = user.Prepod_PrepodMeta.FirstOrDefault();

                    var prepodMeta = new PrepodViewModel() { };
                    if (prepodPrepodMeta != null)
                    {
                        prepodMeta = new PrepodViewModel()
                        {
                            DegreeId = prepodPrepodMeta.DegreeId
                        };
                    }

                    model = new UserCommonViewModel()
                    {
                        IsNewUser = false,
                        PrepodMeta = prepodMeta,
                        Login = user.Login,
                        //Password = user.Password,
                        FirstName = userMeta.FirstName,
                        LastName = userMeta.LastName,
                        MiddleName = userMeta.MiddleName,
                        IsBlocked = user.BlockDate != null,
                        Phone = userMeta.Contact.PhoneNumber,
                        AdditionalPhone = userMeta.Contact.AdditionalPhoneNumber,
                        Email = userMeta.Contact.Email,
                        Skype = userMeta.Contact.Skype,
                        Tasks = numberTasks
                    };

                }

                model.IsPartialView = (isPartial.HasValue && isPartial.Value);

                return View(model);
            }

        }

        public ActionResult LogOff()
        {
            // Delete the user details from cache.
            Session.Abandon();

            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();

            // Clear authentication cookie.
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "") { Expires = DateTime.Now.AddYears(-1) };
            Response.Cookies.Add(cookie);
            return RedirectToAction("Login", "Home");
        }


        public ActionResult About()
        {
            return View("About");
        }
    }
}
