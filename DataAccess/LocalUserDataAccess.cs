using System;
using System.Collections.Generic;
using Domain.BusinessObjects;
using System.Linq;
using Exceptions.BusinessExceptions;

namespace DataAccess
{
    public class LocalUserDataAccess : IDataAccess<User>
    {
        private static int _currentId = -1;
        public static int CurrentId
        {
            get
            {
                _currentId++;
                return _currentId;
            }
        }
        

        public User Get(string id)
        {
            User dummyUser = GetCopy(id);
            User user = Database.Instance.Users.Get(dummyUser);
            return user;
        }

        public User Get(int id)
        {
            throw new NotImplementedException();
        }

        public User GetCopy(string id)
        {
            try
            {
                User user = Database.Instance.Users.GetCopyOfInternalList().First(u => u.Username == id);
                return user;
            }
            catch(ArgumentNullException ane)
            {
                throw new FindUserException();
            }
            catch(InvalidOperationException ioe)
            {
                throw new FindUserException();
            }
        }

        public User GetCopyId(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            return Database.Instance.Users.GetCopyOfInternalList();
        }

        public void Add(User elem)
        {
            Database.Instance.Users.Add(elem);
        }

        public void Delete(User elem)
        {
            bool existed = Database.Instance.Users.Remove(elem);

            if(!existed)
            {
                throw new FindUserException();
            }
        }

        public void Update(User elem)
        {
            try
            {
                User oldUser = Database.Instance.Users.GetCopyOfInternalList().First(u => u.ID == elem.ID);
                Database.Instance.Users.Remove(oldUser);
                Database.Instance.Users.Add(elem);
            }
            catch(ArgumentNullException ane)
            {
                throw new FindUserException();
            }
            catch(InvalidOperationException ioe)
            {
                throw new FindUserException();
            }
        }
    }
}