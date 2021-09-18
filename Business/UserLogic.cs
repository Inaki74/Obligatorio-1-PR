using System;
using BusinessInterfaces;
using DataAccess;
using Domain;

namespace Business
{
    public class UserLogic : IUserLogic
    {
        private IDataAccess<User> _userDataAccess = new LocalUserDataAccess();

        public bool Login(string username)
        {
            User user = _userDataAccess.Get(username);
            if (user == null)
            {
                AddUser(username);
                Login(username);
                return false;
            }

            if(!user.LoggedIn)
            {
                user.LoggedIn = true;
                _userDataAccess.Update(user);
            }
            else
            {
                throw new Exception("User already logged in!");
            }
            
            return true;
        }

        private void AddUser(string username)
        {
            User newUser = new User(username, LocalUserDataAccess.CurrentId);
            _userDataAccess.Add(newUser);
        }
    }
}