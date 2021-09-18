using System.Collections.Generic;
using Domain;

namespace DataAccess
{
    public class Database
    {
        private static readonly Database _instance = new Database();

        static Database(){}
        
        private Database(){}
        
        
        public static Database Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<User> Users = new List<User>();
        
        public List<Game> Games = new List<Game>();
    }
}