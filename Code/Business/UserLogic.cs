using System;
using System.Collections.Generic;
using BusinessInterfaces;
using DataAccess;
using Domain.BusinessObjects;
using Domain.HelperObjects;
using Exceptions.BusinessExceptions;

namespace Business
{
    public class UserLogic : IUserLogic
    {
        private IDataAccess<User> _userDataAccess = new LocalUserDataAccess();

        public bool Login(User dummyUser)
        {
            string username = dummyUser.Username;
            User user = new User("", User.DEFAULT_USER_ID);

            try
            {
                user = _userDataAccess.GetCopy(username);
            }
            catch(FindUserException fue)
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
                throw new UserLoggedException();
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