using System.Collections.Generic;
using Domain.BusinessObjects;

namespace DataAccess
{
    public class Database
    {
        private static Database _instance = null;
        private static readonly object _instanceLock = new object();

        private Database(){}
        public static Database Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock(_instanceLock)
                    {
                        if(_instance == null)
                        {
                            _instance = new Database();
                        }
                    }
                }
                return _instance;
            }
        }

        public ThreadSafeList<User> Users = new ThreadSafeList<User>();
        public ThreadSafeList<Game> Games = new ThreadSafeList<Game>();
        public ThreadSafeList<Review> Reviews = new ThreadSafeList<Review>();
    }
}