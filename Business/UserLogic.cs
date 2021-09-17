using System;
using BusinessInterfaces;
using DataAccess;
using Domain;

namespace Business
{
    public class UserLogic : IUserLogic
    {
        private IDataAccess<User> _userDataAccess = new LocalUserDataAccess();

        public void Login(string username)
        {
            User user = _userDataAccess.Get(username);
            if (user == null)
            {
                AddUser(username);
                Login(username);
                return;
            }

            user.LoggedIn = true;
            _userDataAccess.Update(user);
        }

        private void AddUser(string username)
        {
            User newUser = new User(username, LocalUserDataAccess.CurrentId);
            _userDataAccess.Add(newUser);
        }
    }
}