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
        

        public int AddUser(string username)
        {
            User dummy = new User(username,-1);
            if (_userDataAccess.Exist(dummy)) { throw new UserAlreadyExistsException(); }
            if (string.IsNullOrEmpty(username)) { throw new UserInvalidRequestException(); }
            
            User newUser = new User(username, LocalUserDataAccess.CurrentId);
            _userDataAccess.Add(newUser);
            return newUser.ID;
        }

        public void ModifyUser(string username, int id)
        {
            User dummy = new User(username, id);
            if (!_userDataAccess.Exist(dummy)) { throw new UserDoesntExistException(); }
            
            _userDataAccess.Update(dummy);
        }
    }
}