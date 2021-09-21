using System.Collections.Generic;
using Domain;

namespace DataAccess
{
    public class Database
    {
        private static Database _instance = null;
        private static readonly object _instanceLock = new object();

        static Database(){}
        
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

        public List<User> Users = new List<User>();
        
        public List<Game> Games = new List<Game>();
    }
}