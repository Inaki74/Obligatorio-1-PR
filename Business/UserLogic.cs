using System;
using BusinessInterfaces;
using DataAccess;
using Domain.BusinessObjects;
using Domain.HelperObjects;

namespace Business
{
    public class UserLogic : IUserLogic
    {
        private IDataAccess<User> _userDataAccess = new LocalUserDataAccess();

        public bool Login(User dummyUser)
        {
            string username = dummyUser.Username;
            User user = _userDataAccess.GetCopy(username);
            if (user == null)
            {
                AddUser(username);
                Login(dummyUser);
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

        public void Logout(User user)
        {
            string username = user.Username;
            User loggedUser = _userDataAccess.GetCopy(username);
            loggedUser.LoggedIn = false;
            _userDataAccess.Update(loggedUser);
        }

        private void AddUser(string username)
        {
            User newUser = new User(username, LocalUserDataAccess.CurrentId);
            _userDataAccess.Add(newUser);
        }
    }
}