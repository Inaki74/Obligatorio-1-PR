using System.Collections.Generic;
using Domain.BusinessObjects;

namespace BusinessInterfaces
{
    public interface IUserLogic
    {
        User GetUser(int id);
        bool Login(User user);
        void Logout(User user);
        int AddUser(string username);
        void DeleteUser(int id);
        void ModifyUser(string username, int id);
    }
}