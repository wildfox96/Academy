using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AUPPRB.Common.Enums;
using AUPPRB.Common.Utils;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.OtherModels.ServiceModels;
using AUPPRB.Models.ViewModels;
using AUPPRB.Models.ViewModels.jqGridModels;
using AUPPRB.Models.ViewModels.UsersModels;
using AUPPRB.Web.Controllers.Base;
using AUPPRB.Domain.Interfaces;
using Trirand.Web.Mvc;


namespace AUPPRB.Web.Controllers.Administration
{

    public class UsersController : BaseController
    {
        private IAdminScheduleService _adminData;

        public UsersController(IAdminScheduleService adminData)
        {
            _adminData = adminData;

        }
        //
        // GET: /Administration/

        public ActionResult Users()
        {
            var gridModel = new UserJqGridModel();
            SetUpGrid(gridModel.UserGrid);

            return View(gridModel);
        }

        private void SetUpGrid(JQGrid usersGrid)
        {
            usersGrid.DataUrl = Url.Action("GridDataRequested");
            usersGrid.EditUrl = Url.Action("EditRows");
            usersGrid.ClientSideEvents.GridInitialized = "gridInitialized";

        }
        public JsonResult GridDataRequested()
        {
            try
            {
                var gridModel = new UserJqGridModel();
                SetUpGrid(gridModel.UserGrid);
                // Для глобального админа
                //                    if (IsUserInRole(CurrentUserLogin, Roles.БРС_Администратор.GetEnumText()))
                //                    {
                // Пользователи только текущего сервиса
                //var users = Db.Users
                //    .Where(c => c.UsersRoles.Any(z => z.Role.ServiceId == (int)ServicesEnum.GovernmentTesting))
                //    .Select(c => new
                //    {
                //        Id = c.Id,
                //        FirstName = c.FirstName,
                //        LastName = c.LastName,
                //        MiddleName = c.MiddleName,
                //        Company = c.CompanyDictionary.Name,
                //        UniqueNumber = c.UniqueNumber,
                //        Login = c.Login,
                //        Roles = c.Roles,
                //        IsBlocked = c.IsBlocked ? "Да" : "Нет"
                //    });

                var users = GetAllUsers().Select(c =>
                                                 {
                                                     var firstOrDefault = c.UserMeta.FirstOrDefault();
                                                     return firstOrDefault != null ? new
                                                                                          {
                                                                                              RoleIcon = c.Student_StudentMeta.FirstOrDefault() != null ? "0" : "1",
                                                                                              Id = c.Id,
                                                                                              Login = c.Login,
                                                                                              LastName = c.UserMeta != null ? firstOrDefault.LastName : "",
                                                                                              FirstName = c.UserMeta != null ? firstOrDefault.FirstName : "",
                                                                                              MiddleName = c.UserMeta != null ? firstOrDefault.MiddleName : "",
                                                                                              Email = c.UserMeta != null ? firstOrDefault.Contact.Email : ""

                                                                                          } : new
                                                                                          {
                                                                                              RoleIcon = c.Student_StudentMeta.FirstOrDefault() != null ? "0" : "1",
                                                                                              Id = c.Id,
                                                                                              Login = c.Login,
                                                                                              LastName = "",
                                                                                              FirstName = "",
                                                                                              MiddleName = "",
                                                                                              Email = ""

                                                                                          };
                                                 });

                return gridModel.UserGrid.DataBind(users.AsQueryable());
                //}


            }
            catch (Exception ex)
            {
                //#region Логирование
                //LogHelper.AddSystemLogMessage(new SystemLogMessage()
                //{
                //    AuditEventId = AuditEvents.СистемнаяОшибка,
                //    UserId = CurrentUserId,
                //    EventTypeId = EventTypes.Ошибка,
                //    IPAddress = Request.UserHostAddress,
                //    ComputerName = NetworkHelper.GetHostNameByHostAddress(Request.UserHostAddress),
                //    Date = DateTime.Now,
                //    Detalization = String.Format("Произошла ошибка при запросе информации: {0}", ex.Message + "\r\n" + ex.InnerException + "\r\n" + ex.StackTrace)

                //});
                //#endregion

                //return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditRows(User editedUser)
        {
            var gridModel = new UserJqGridModel();
            if (gridModel.UserGrid.AjaxCallBackMode == AjaxCallBackMode.DeleteRow)
            {
                //TODO: Проверить на удаление самого себя

                User user = _accountService.GetUser(editedUser.Id);
                //DependencyResolver.Current.GetService<IDataProvider>()
                //    .Users.FirstOrDefault(c => c.Id == editedUser.Id);


                try
                {
                    //DependencyResolver.Current.GetService<IDataProvider>().Save();
                }
                catch (Exception ex)
                {
                    //#region Логирование
                    //LogHelper.AddSystemLogMessage(new SystemLogMessage()
                    //{
                    //    AuditEventId = AuditEvents.СистемнаяОшибка,
                    //    UserId = CurrentUserId,
                    //    EventTypeId = EventTypes.Ошибка,
                    //    IPAddress = Request.UserHostAddress,
                    //    ComputerName = NetworkHelper.GetHostNameByHostAddress(Request.UserHostAddress),
                    //    Date = DateTime.Now,
                    //    Detalization = String.Format("Произошла ошибка при удалении пользователя: {0}", ex.Message + "\r\n" + ex.InnerException + "\r\n" + ex.StackTrace)

                    //});
                    //#endregion

                    // Ошибка удаления
                }
            }
            return RedirectToAction("Users");
        }
        public ActionResult CreateStudent()
        {
          
            var model = new UserCommonViewModel()
                        {
                            StudentMeta = new StudentViewModel(),
                            IsNewUser = true
                        };
            return View("Create", model);

        }

        public ActionResult CreatePrepod()
        {
            ViewBag.Dapartments = new SelectList(GetItemsOfDictionary(DictionaryTypeEnum.Кафедры).ToList(), "Id", "Name");
            ViewBag.Degree = new SelectList(GetItemsOfDictionary(DictionaryTypeEnum.УченаяСтепень).ToList(), "Id", "Name");
            var model = new UserCommonViewModel()
            {
                PrepodMeta = new PrepodViewModel()
                {
                    DepartmentsIds = new List<int>()
                },
                IsNewUser = true
            };
            return View("Create", model);
        }

        public ActionResult Edit(int id)
        {
            CacheService.Remove("UserData" + id + "|");
            var user = GetUserById(id);
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
                        FlowId = studentStudentMeta.FlowId,
                        GraduationDate = studentStudentMeta.GraduationDate.Year,
                        MarkBookNumber = studentStudentMeta.MarkBookNumber,
                        GroupId = studentStudentMeta.GroupId,
                        IsDismissed = studentStudentMeta.IsDismissed,
                        SpecialityId = studentStudentMeta.SpecialtyId,
                        StudentCardNumber = studentStudentMeta.StudentCardNumber,
                        IdSpecialtyMeta = studentStudentMeta.IdSpecialtyMeta
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
            }
            else
            {
                var prepodMeta = new PrepodViewModel() { };
                var prepodPrepodMeta = user.Prepod_PrepodMeta.FirstOrDefault();

                if (prepodPrepodMeta != null)
                {
                    prepodMeta = new PrepodViewModel()
                    {
                        DegreeId = prepodPrepodMeta.DegreeId,
                        DepartmentsIds = prepodPrepodMeta.Prepod_PrepodiCafedri.Select(dep => dep.IdCafedri).ToList(),
                        DepartmentsIdsString = prepodPrepodMeta.Prepod_PrepodiCafedri.Aggregate("", (current, depID) => current + (depID.IdCafedri + ";"))
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


                if (!String.IsNullOrEmpty(prepodMeta.DepartmentsIdsString))
                {
                    List<SelectList> departmentsList = new List<SelectList>();

                    foreach (var depId in prepodMeta.DepartmentsIdsString.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        departmentsList.Add(new SelectList(GetItemsOfDictionary(DictionaryTypeEnum.Кафедры).ToList(), "Id", "Name", depId));
                    }

                    ViewBag.Dapartments = departmentsList;
                }
                else
                    ViewBag.Dapartments = new SelectList(GetItemsOfDictionary(DictionaryTypeEnum.Кафедры).ToList(), "Id", "Name");

            }

            ViewBag.Degree = new SelectList(GetItemsOfDictionary(DictionaryTypeEnum.УченаяСтепень).ToList(), "Id", "Name");
            ViewBag.Facultaty = new SelectList(GetItemsOfDictionary(DictionaryTypeEnum.Факультеты).ToList(), "Id", "Name");
            ViewBag.Speciality = new SelectList(GetItemsOfDictionary(DictionaryTypeEnum.Специальности).ToList(), "Id", "Name");
            ViewBag.Years = new SelectList(Enumerable.Range((DateTime.Now.Year - 6), 15));
            ViewBag.Number = new SelectList(Enumerable.Range(1, 8));
            ViewBag.Groups = new SelectList(GetItemsOfDictionary(DictionaryTypeEnum.Группы).ToList(), "Id", "Name");
            ViewBag.Flows = new SelectList(GetItemsOfDictionary(DictionaryTypeEnum.Потоки).ToList(), "Id", "Name");


            return View("Create", model);
        }
        public ActionResult Delete(int id)
        {

            User deleteUser = GetUserById(id);
            var deleteResult = DeleteUser(deleteUser);

            return Json(new { data = 11 });
        }

        /// <summary>
        /// Сохранение пользователя
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        #region сохранить пользователя
        public ActionResult Save(UserCommonViewModel userModel)
        {
            var newUser = new User();
            if (userModel.IsNewUser != true)
            {
                CacheService.Remove("UserData" + userModel.Login + "|");
                newUser = GetUserByLogin(userModel.Login);
            }

            var isNew = userModel.IsNewUser;

            try
            {
                if (!isNew && String.IsNullOrEmpty(userModel.Password))
                {
                    if (ModelState["Password"].Errors.Count > 0)
                        ModelState["Password"].Errors.RemoveAt(0);
                }
                // При создании пользователя надо проигнорить поле Id
            }
            catch (Exception ex)
            {
                #region Логирование
                //LogHelper.AddSystemLogMessage(new SystemLogMessage()
                //{
                //    AuditEventId = AuditEvents.СистемнаяОшибка,
                //    UserId = CurrentUserId,
                //    EventTypeId = EventTypes.Ошибка,
                //    IPAddress = Request.UserHostAddress,
                //    ComputerName = NetworkHelper.GetHostNameByHostAddress(Request.UserHostAddress),
                //    Date = DateTime.Now,
                //    Detalization = String.Format("Произошла ошибка при правке модели пользователя: {0}", ex.Message + "\r\n" + ex.InnerException + "\r\n" + ex.StackTrace)

                //});
                #endregion
            }

            #region Validity
            if (ModelState.IsValid)
            {
                newUser.Login = userModel.Login;
                if (!String.IsNullOrEmpty(userModel.Password))
                    newUser.Password = PasswordHelper.Sha1EncryptPassword(userModel.Password);
                newUser.IsDeleted = false;
                newUser.BlockReason = String.Empty;
                UserMeta userMeta = newUser.UserMeta.FirstOrDefault();

                if (userMeta != null)
                {
                    userMeta.LastName = userModel.LastName;
                    userMeta.FirstName = userModel.FirstName;
                    userMeta.MiddleName = userModel.MiddleName;
                    if (userMeta.Contact != null)
                    {
                        userMeta.Contact.Email = userModel.Email;
                        userMeta.Contact.Skype = userModel.Skype;
                        userMeta.Contact.PhoneNumber = userModel.Phone;
                        userMeta.Contact.AdditionalPhoneNumber = userModel.AdditionalPhone;
                    }
                }
                else
                {
                    newUser.UserMeta.Add(new UserMeta
                    {
                        LastName = userModel.LastName,
                        FirstName = userModel.FirstName,
                        MiddleName = userModel.MiddleName,
                        Contact = new Contact
                        {
                            Email = userModel.Email,
                            Skype = userModel.Skype,
                            PhoneNumber = userModel.Phone,
                            AdditionalPhoneNumber = userModel.AdditionalPhone
                        }
                    });
                }

                if (userModel.StudentMeta != null)
                {
                    var studentMeta = newUser.Student_StudentMeta.FirstOrDefault();
                    Spezialnost_SpezialnostMeta meta = _adminData.GetSpezMetaInfo(userModel.StudentMeta.IdSpecialtyMeta);
                    if (studentMeta != null)
                    {

                        studentMeta.FacultyId = userModel.StudentMeta.FacultyId;
                        studentMeta.SpecialtyId = userModel.StudentMeta.SpecialityId;
                        studentMeta.AdmissionDate = new DateTime(meta.GodPostup, 1, 1);
                        studentMeta.GraduationDate = new DateTime(meta.GodPostup + meta.SrokObuch, 1, 1);
                        studentMeta.FlowId = userModel.StudentMeta.FlowId;
                        studentMeta.GroupId = userModel.StudentMeta.GroupId;
                        studentMeta.StudentCardNumber = userModel.StudentMeta.StudentCardNumber;
                        studentMeta.MarkBookNumber = userModel.StudentMeta.MarkBookNumber;
                        studentMeta.IsDismissed = false;
                        studentMeta.IdSpecialtyMeta = userModel.StudentMeta.IdSpecialtyMeta;
                        studentMeta.FormOfStuduId = meta.IdFrmObuch;
                    }
                    else
                    {
                        newUser.Student_StudentMeta.Add(new Student_StudentMeta()
                                                        {
                                                            FacultyId = userModel.StudentMeta.FacultyId,
                                                            SpecialtyId = userModel.StudentMeta.SpecialityId,
                                                            AdmissionDate =
                                                               new DateTime(meta.GodPostup, 1, 1),
                                                            GraduationDate =
                                                                new DateTime(meta.GodPostup + meta.SrokObuch, 1, 1),
                                                            FlowId = userModel.StudentMeta.FlowId,
                                                            GroupId = userModel.StudentMeta.GroupId,
                                                            StudentCardNumber = userModel.StudentMeta.StudentCardNumber,
                                                            MarkBookNumber = userModel.StudentMeta.MarkBookNumber,
                                                            IsDismissed = false,
                                                            IdSpecialtyMeta = userModel.StudentMeta.IdSpecialtyMeta,
                                                            FormOfStuduId = meta.IdFrmObuch

                                                        });
                    }
                }

                if (userModel.PrepodMeta != null)
                {
                    if (userModel.IsNewUser)
                    {
                        Prepod_PrepodMeta prepod_meta = new Prepod_PrepodMeta();
                        var departments = userModel.PrepodMeta.DepartmentsIdsString;

                        if (!String.IsNullOrEmpty(departments))
                        {
                            foreach (var depId in departments.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                prepod_meta.Prepod_PrepodiCafedri.Add(new Prepod_PrepodiCafedri()
                                {
                                    IdCafedri = Convert.ToInt32(depId),
                                    IdSotr = userModel.Id,
                                    Stavka = 1,
                                    TipZanyatosti = 79
                                });
                            }
                        }

                        prepod_meta.DegreeId = userModel.PrepodMeta.DegreeId;
                        prepod_meta.IsDismissed = false;

                        newUser.Prepod_PrepodMeta.Add(prepod_meta);
                    }
                    else
                    {
                        //В КОДЕ УСТАНОВЛЕНО МИНИМУМ ОДНА КАФЕДРА
                        List<Prepod_PrepodiCafedri> cafedry = new List<Prepod_PrepodiCafedri>();
                        var departments = userModel.PrepodMeta.DepartmentsIdsString;

                        if (!String.IsNullOrEmpty(departments))
                        {
                            foreach (var depId in departments.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                cafedry.Add(new Prepod_PrepodiCafedri()
                                {
                                    IdCafedri = Convert.ToInt32(depId),
                                    IdSotr = newUser.Prepod_PrepodMeta.First().Id,
                                    Stavka = 1,
                                    TipZanyatosti = 79
                                });
                            }

                            UpdateCafedry(cafedry);
                            newUser.Prepod_PrepodMeta.First().DegreeId = userModel.PrepodMeta.DegreeId;
                            newUser.Prepod_PrepodMeta.First().IsDismissed = false;
                        }
                        else
                        {
                            newUser.Prepod_PrepodMeta.Add(new Prepod_PrepodMeta()
                            {
                                DegreeId = userModel.PrepodMeta.DegreeId,
                                IsDismissed = false
                            });
                        }
                    }
                }

                bool result;
                if (isNew)
                    AddUser(newUser);
                else
                {
                    UpdateUser(newUser);
                }

                var firstResult = DB_SaveChanges();
                if (firstResult)
                {
                    var editresult = AddingRolesAndTasks(newUser.Login, userModel.Tasks);
                    result = editresult;
                }
                else
                {
                    result = false;
                }

                if (result)
                {
                    if (isNew)
                        return Json(new { isValid = true, type = "success", message = "Пользователь " + userModel.Login + " добавлен успешно!" });
                    else
                    {
                        return Json(new { isValid = true, type = "success", message = "Пользователь " + userModel.Login + " редактирован успешно!" });
                    }
                }
                else
                {
                    if (isNew)
                        return
                            Json(
                                new
                                {
                                    isValid = true,
                                    type = "error",
                                    message = "Пользователь " + userModel.Login + " не добавлен. Ошибка при сохранении данных!"
                                });
                    else
                    {
                        return Json(new { isValid = true, type = "success", message = "Не удалось сохранить изменения для  пользователя " + userModel.Login + " !" });
                    }
                }
            }
            #endregion

            //Добавила (валидация видимая)
           
            if (userModel.PrepodMeta != null)
            {
                List<SelectList> departmentsList = new List<SelectList>();
                userModel.PrepodMeta.DepartmentsIds = new List<int>();

                foreach (var depId in userModel.PrepodMeta.DepartmentsIdsString.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    userModel.PrepodMeta.DepartmentsIds.Add(Convert.ToInt16(depId));
                    departmentsList.Add(new SelectList(GetItemsOfDictionary(DictionaryTypeEnum.Кафедры).ToList(), "Id", "Name", depId));
                }

                ViewBag.Dapartments = departmentsList;
                ViewBag.Degree = new SelectList(GetItemsOfDictionary(DictionaryTypeEnum.УченаяСтепень).ToList(), "Id", "Name");
            }

            return Json(new
            {
                view = RenderView.RenderViewToString(ControllerContext, "Create", userModel),
                isValid = false,
                type = "error",
                message = "Проверьте введенные данные!"
            });



        }
        #endregion
        private bool AddingRolesAndTasks(string login, String rolesAndTasksString)
        {
            var user = GetUserByLogin(login);
            // если новый пользователь
            if (user.Id == 0)
            {
                // если роли не назначены
                if (String.IsNullOrEmpty(rolesAndTasksString))
                {
                    // назначить все таски пользователя Студент
                    var standartTasks = GetTasksByRole(RoleEnum.Студент);

                    foreach (var task in standartTasks)
                    {
                        AddUserTask(user.Id, task.Id);
                    }
                }
                else
                {
                    // если назначены
                    foreach (var rolesAndTasks in rolesAndTasksString.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        //var taskById = GetTaskById(Int32.Parse(rolesAndTasks));
                        AddUserTask(user.Id, Int32.Parse(rolesAndTasks));
                    }
                }

                var saveResult = DB_SaveChanges();
                return saveResult;
            }
            else
            {
                if (String.IsNullOrEmpty(rolesAndTasksString))
                {

                    // назначить все таски пользователя Студент
                    var standartTasks = GetTasksByRole(RoleEnum.Студент);

                    foreach (var task in standartTasks)
                    {
                        AddUserTask(user.Id, task.Id);
                    }
                }
                else
                {
                    foreach (var rolesAndTasks in rolesAndTasksString.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        AddUserTask(user.Id, Int32.Parse(rolesAndTasks));
                    }
                }

                var saveResult = DB_SaveChanges();
                return saveResult;
            }
        }
        public ActionResult JsonEdit([Bind(Exclude = "Photo")] User editedUser)
        {
            return Json(new { success = false, callbackFunction = "removeLoading" });
        }

        public IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        public ActionResult GetFacultiesList()
        {
            return Json(new
            {
                Faculty = GetItemsOfDictionary(DictionaryTypeEnum.Факультеты)
                    .ToList()
                    .Select(p => new { Id = p.Id, Name = p.Name })
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListOfSpesial(int facultyId)
        {


            return Json(new
            {
                Spec = _adminData.GetListOfSpecialities(facultyId).Select(p => new { Id = p.IdSpez, Name = p.Spezialnost1 })
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSpezMetasForSpes(int specialostId)
        {
            return Json(
                new
                {
                    Meta = _adminData.GetSpezialnostMeta(specialostId).Select(
                                                           p => new
                                                           {
                                                               Id = p.Id,
                                                               GodPostup = p.GodPostup,
                                                               GodOkonch = p.SrokObuch + p.GodPostup,
                                                               FormOfStudy = GetItemsOfDictionary(DictionaryTypeEnum.ФормаОбучения).ToList().Where(o => o.Id == p.IdFrmObuch).First().Name.ToLower(),
                                                           }
                        ).ToArray().OrderBy(p => p.GodPostup).OrderBy(p => p.FormOfStudy)
                }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetPotokiForSpezialnostMeta(int id)
        {

            return Json(new
            {
                Potoki = _adminData.GetPotokiForSpezMeta(id).
                    Select(x => new { Id = x.Pot, Name = x.PotokDictionary.Name })
                    .Distinct()
                    .OrderBy(x => x.Name)
                    .ToArray()
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetGroupsForPotok(int idPotok, int idSpezMeta)
        {

            return Json(new
            {
                Groups = _adminData.GetSpisokGrupp(idPotok, idSpezMeta).
                       Select(x => new { Id = x.IdGroup, Name = x.Gruppa })
                       .OrderBy(x => x.Name)
                       .ToArray()

            }, JsonRequestBehavior.AllowGet);

        }

    }
}

