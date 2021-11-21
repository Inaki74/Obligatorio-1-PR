using System;
using System.Collections.Generic;
using Domain.BusinessObjects;
using System.Linq;
using Database;
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

        public LocalUserDataAccess()
        {
            User admin = new User("ADMIN", 3007);
            Add(admin);
        }
        
        public User Get(string id)
        {
            User dummyUser = GetCopy(id);
            User user = InMemoryDatabase.Instance.Users.Get(dummyUser);
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
                User user = InMemoryDatabase.Instance.Users.GetCopyOfInternalList().First(u => u.Username == id);
                return user;
            }
            catch(ArgumentNullException ane)
            {
                throw new FindUserException(ane.Message);
            }
            catch(InvalidOperationException ioe)
            {
                throw new FindUserException(ioe.Message);
            }
        }

        public User GetCopyId(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            return InMemoryDatabase.Instance.Users.GetCopyOfInternalList();
        }

        public void Add(User elem)
        {
            InMemoryDatabase.Instance.Users.Add(elem);
        }

        public void Delete(User elem)
        {
            bool existed = InMemoryDatabase.Instance.Users.Remove(elem);

            if(!existed)
            {
                throw new FindUserException();
            }
        }

        public void Update(User elem)
        {
            try
            {
                User oldUser = InMemoryDatabase.Instance.Users.GetCopyOfInternalList().First(u => u.ID == elem.ID);
                InMemoryDatabase.Instance.Users.Remove(oldUser);
                InMemoryDatabase.Instance.Users.Add(elem);
            }
            catch(ArgumentNullException ane)
            {
                throw new FindUserException(ane.Message);
            }
            catch(InvalidOperationException ioe)
            {
                throw new FindUserException(ioe.Message);
            }
        }

        public bool Exist(User elem)
        {
            List<User> userList = InMemoryDatabase.Instance.Users.GetCopyOfInternalList();
            return userList.Exists(u => u.Username == elem.Username || u.ID == elem.ID);
        }
    }
}