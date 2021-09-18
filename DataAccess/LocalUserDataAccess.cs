﻿using System;
using System.Collections.Generic;
using Domain;
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
        

        public User Get(int id)
        {
            try
            {
                User user = Database.Instance.Users.First(u => u.ID == id);
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong in the data access");
                return null;
            }
            
        }

        public User Get(string id)
        {
            User user = Database.Instance.Users.FirstOrDefault(u => u.Username == id);
            return user;
        }

        public List<User> GetALl()
        {
            return Database.Instance.Users;
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
            User oldUser = Database.Instance.Users.First(u => u.ID == elem.ID);
            Database.Instance.Users.Remove(oldUser);
            Database.Instance.Users.Add(elem);
        }
    }
}