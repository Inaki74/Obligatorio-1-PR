using System;
using System.Collections.Generic;
using Domain.BusinessObjects;
using System.Linq;

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

        public User GetCopy(string id)
        {
            User user = Database.Instance.Users.GetCopyOfInternalList().FirstOrDefault(u => u.Username == id);
            return user;
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
            Database.Instance.Users.Remove(elem);
        }

        public void Update(User elem)
        {
            User oldUser = Database.Instance.Users.GetCopyOfInternalList().First(u => u.ID == elem.ID);
            Database.Instance.Users.Remove(oldUser);
            Database.Instance.Users.Add(elem);
        }
    }
}