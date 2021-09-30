using System.Collections.Generic;
using Domain.BusinessObjects;

namespace Database
{
    public class InMemoryDatabase
    {
        private static InMemoryDatabase _instance = null;
        private static readonly object _instanceLock = new object();

        private InMemoryDatabase(){}
        public static InMemoryDatabase Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock(_instanceLock)
                    {
                        if(_instance == null)
                        {
                            _instance = new InMemoryDatabase();
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