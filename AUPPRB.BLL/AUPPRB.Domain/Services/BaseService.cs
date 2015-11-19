using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AUPPRB.Common.Enums;
using AUPPRB.Common.Extensions;
using AUPPRB.Common.Utils.Caching;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Repository.DB;

namespace AUPPRB.Domain.Services
{
    public class BaseServices
    {
        protected readonly IDataProvider DataProvider;

        protected readonly ILikesService LikesService;

        protected readonly ICache ServiceCache;

        protected readonly TimeSpan DefaultExpirationTime;

        public BaseServices()
        {
            DataProvider = DependencyResolver.Current.GetService<IDataProvider>();
            LikesService = DependencyResolver.Current.GetService<ILikesService>();
            ServiceCache = CacheProvider.Current.GetCache();
            DefaultExpirationTime = new TimeSpan(0, 20, 0);
        }

        ~BaseServices()
        {
            DataProvider.Dispose();
        }

        private IEnumerable<int> _getUserTaskIds(int userId)
        {
            var user = DataProvider.Users.FirstOrDefault(p => p.Id == userId);
            if (user == null)
                throw new Exception("User not exist exception");
            return ServiceCache.GetOrAdd("UserTasks:" + userId + "|",
                () => user.UserTask.Select(p => p.TaskId).ToArray(), DefaultExpirationTime);
        }

        private IEnumerable<int> _getUserRolesIds(int userId)
        {
            var user = DataProvider.Users.FirstOrDefault(p => p.Id == userId);
            if (user == null)
                throw new Exception("User not exist exception");
            return ServiceCache.GetOrAdd("UserTasks:" + userId + "|",
                () => user.UserRole.Select(p => p.RoleId).ToArray(), DefaultExpirationTime);
        }

        public bool IsTasksAllowedForUser(int userId, params TaskEnum[] tasks)
        {
            if (tasks == null || tasks.Count() == 0)
            {
                return true;
            }
            var userTasks = _getUserTaskIds(userId);
            return tasks.Select(p => p.GetEnumValue()).Any(userTasks.Contains);
        }

        public bool IsUserInRoles(int userId, params RoleEnum[] roles)
        {
            if (roles == null || roles.Count() == 0)
            {
                return true;
            }
            IEnumerable<int> userRoles = _getUserRolesIds(userId);
            return roles.Select(p => p.GetEnumValue()).Any(userRoles.Contains);
        }

        public User GetUser(int id)
        {
            User user = ServiceCache.GetOrAdd("UserData" + id + "|", () => DataProvider.Users.GetById(id),
                DefaultExpirationTime);
            return user;
        }

        public User GetUser(string login)
        {
            User user = ServiceCache.GetOrAdd("UserData" + login + "|",
                () => DataProvider.Users.FirstOrDefault(c => c.Login == login), DefaultExpirationTime);
            return user;
        }

        public List<Role> GetAllRoles()
        {
            return DataProvider.Roles.GetAll().ToList();
        }

        public List<Task> GetTasksByRole(RoleEnum role)
        {
            var id = role.GetEnumValue();
            return DataProvider.RoleTasks.Filter(c => c.RoleId == id).Select(c => c.Task).ToList();


        }

        public bool SaveChanges()
        {

            try
            {
                DataProvider.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task GetTaskById(int id)
        {
            return DataProvider.Tasks.GetById(id);
        }
    }
}
