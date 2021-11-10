using System.Collections.Generic;
using DataAccess.Interface;
using Database.Interface;
using Domain;

namespace DataAccess
{
    public class LogInfoDataAccess : ILogDataAccess
    {
        private readonly IDatabase _database;
        public LogInfoDataAccess(IDatabase database)
        {
            _database = database;
        }
        
        public List<Log> Get(string username)
        {
            throw new System.NotImplementedException();
        }

        public List<Log> Get(string username, string gamename)
        {
            throw new System.NotImplementedException();
        }

        public List<Log> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}