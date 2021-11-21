using System.Collections.Generic;
using Domain.BusinessObjects;

namespace BusinessInterfaces
{
    public interface IUserLogic
    {
        bool Login(User user);
        void Logout(User user);
        int AddUser(string username);

        void ModifyUser(string username, int id);
    }
}