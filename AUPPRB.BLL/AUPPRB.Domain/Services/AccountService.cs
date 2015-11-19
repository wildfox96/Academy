using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using AUPPRB.Common.Enums;
using AUPPRB.Common.Extensions;
using AUPPRB.Common.Utils;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.OtherModels.ServiceModels;


namespace AUPPRB.Domain.Services
{
    public class AccountService : BaseServices, IAccountService
    {
        public UserValidationResult ValidateUser(string login, string password)
        {

            string encryptPassword = PasswordHelper.Sha1EncryptPassword(password);
            //string encryptPassword = password;


            User user = DataProvider.Users.FirstOrDefault(p => (p.Login == login));
            UserValidationResult userValidationResult = new UserValidationResult();
            if (user == null)
            {
                userValidationResult.IsExist = false;
                userValidationResult.IsBlocked = false;
                userValidationResult.IsExist = false;
            }
            else
            {
                userValidationResult.IsExist = true;

                userValidationResult.IsRightPassword = encryptPassword == user.Password;

                //TODO: fill this data
                if (!userValidationResult.IsRightPassword)
                {
                    //user.ErrorPinCount++;
                    //if (user.ErrorPinCount >= 5)
                    //{
                    //    user.BlockDate = DateTime.Now;
                    //    user.BlockReason = "Пятикратный ввод неверного пароля";
                    //}
                    //DataProvider.Save();
                }
                //TODO: СБРОСИТЬ ЕРР ПАСС ЕСЛИ ВЕРНЫЙ ВХОД
                userValidationResult.IsBlocked = user.BlockDate.HasValue;
                userValidationResult.BlockReason = user.BlockReason;

                userValidationResult.IsDeleted = user.IsDeleted;

            }

            return userValidationResult;

        }

        public UserValidationResult ValidateUserById(int id)
        {
            var user = DataProvider.Users.FirstOrDefault(p => (p.Id == id));
            var userValidationResult = new UserValidationResult();
            if (user == null)
            {
                userValidationResult.IsExist = false;
                userValidationResult.IsBlocked = false;
                userValidationResult.IsExist = false;
            }
            else
            {
                userValidationResult.IsExist = true;

                userValidationResult.IsRightPassword = true;

                //TODO: fill this data
                if (!userValidationResult.IsRightPassword)
                {
                    //user.ErrorPinCount++;
                    //if (user.ErrorPinCount >= 5)
                    //{
                    //    user.BlockDate = DateTime.Now;
                    //    user.BlockReason = "Пятикратный ввод неверного пароля";
                    //}
                    //DataProvider.Save();
                }
                //TODO: СБРОСИТЬ ЕРР ПАСС ЕСЛИ ВЕРНЫЙ ВХОД
                userValidationResult.IsBlocked = user.BlockDate.HasValue;
                userValidationResult.BlockReason = user.BlockReason;

                userValidationResult.IsDeleted = user.IsDeleted;

            }

            return userValidationResult;
        }

        public new User GetUser(int id)
        {
            return base.GetUser(id);
        }
        public new User GetUser(string login)
        {
            return base.GetUser(login);
        }
        public User GetUser(string login, string password)
        {

            //string encryptPassword = PasswordHelper.Sha1EncryptPassword(password);
            string encryptPassword = PasswordHelper.Sha1EncryptPassword(password);
            User user = DataProvider.Users.FirstOrDefault(p => (p.Login == login) && encryptPassword == p.Password);

            if (user != null)
                ServiceCache.Add("UserData" + user.Id + "|", user, DefaultExpirationTime);
            return user;
        }


        public List<User> GetAllUsers()
        {
            List<User> users = DataProvider.Users.GetAll().ToList();
            //if (users != null)
            //    ServiceCache.Add("UserData" + user.Id + "|", user, DefaultExpirationTime);
            return users;
        }


        public new bool IsUserInRoles(int userId, params RoleEnum[] roles)
        {
            return base.IsUserInRoles(userId, roles);
        }



        public new bool IsTasksAllowedForUser(int userId, params TaskEnum[] tasks)
        {
            return base.IsTasksAllowedForUser(userId, tasks);
        }

        public bool AddUser(User user)
        {
            try
            {
                DataProvider.Users.Add(user);
                return true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", RequestResult: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
                return false;
            }
        }

        public bool UpdateUser(User user)
        {
            try
            {
                DataProvider.Users.Update(user);
                return true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", RequestResult: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
                return false;
            }
        }


        public bool DeleteUser(User user)
        {
            try
            {
                DataProvider.Users.Delete(user);
                DataProvider.Save();
                return true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", RequestResult: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
                return false;
            }
        }

        public bool AddUserTask(int userId, int taskId)
        {
            try
            {
                DataProvider.UserTasks.Add(new UserTask()
                {
                    UserId = userId,
                    TaskId = taskId
                });

                DataProvider.Save();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool AddCafedry(IEnumerable<Prepod_PrepodiCafedri> caderfList)
        {
            try
            {
                int idSotr = caderfList.First().IdSotr;
                DataProvider.PrepodiCafedri.DeleteAll(DataProvider.PrepodiCafedri.Filter(a => a.IdSotr == idSotr).ToList());
                DataProvider.Save();
                
                foreach (Prepod_PrepodiCafedri caf in caderfList)
                {
                    DataProvider.PrepodiCafedri.Add(caf);
                }

                DataProvider.Save();
                return true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", RequestResult: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
                return false;
            }
        }
        public string[] GetReservateBookNumbers()
        {
            return DataProvider.LibraryReservateBook.Get().Select(p => p.Library_Book.BookNumber).ToArray();
        }


        public List<Role> GetRoles()
        {
            return GetAllRoles();
        }
    }
}
