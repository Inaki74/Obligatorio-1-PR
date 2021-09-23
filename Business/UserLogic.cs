using System;
using BusinessInterfaces;
using DataAccess;
using Domain;

namespace Business
{
    public class UserLogic : IUserLogic
    {
        private IDataAccess<User> _userDataAccess = new LocalUserDataAccess();

        public bool Login(User dummyUser)
        {
            string username = dummyUser.Username;
            User user = _userDataAccess.Get(username);
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

        public void Logout(string username)
        {
            User loggedUser = _userDataAccess.Get(username);
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