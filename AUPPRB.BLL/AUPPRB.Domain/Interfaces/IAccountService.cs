using System.Collections.Generic;
using System.Linq;
using AUPPRB.Common.Enums;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.OtherModels.ServiceModels;

namespace AUPPRB.Domain.Interfaces
{
    public interface IAccountService
    {
        UserValidationResult ValidateUser(string login, string password);

        UserValidationResult ValidateUserById(int id);

        User GetUser(int id);

        User GetUser(string login);

        User GetUser(string login, string password);

        List<User> GetAllUsers();

        bool IsUserInRoles(int userId, params RoleEnum[] role);

        bool IsTasksAllowedForUser(int userId, params TaskEnum[] role);

        bool AddUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        List<Role> GetAllRoles();
        List<Task> GetTasksByRole(RoleEnum role);

        Task GetTaskById(int id);
        bool SaveChanges();

        bool AddUserTask(int userId, int taskId);

        string[] GetReservateBookNumbers();

        bool AddCafedry(IEnumerable<Prepod_PrepodiCafedri> caderfList);
    }
}
