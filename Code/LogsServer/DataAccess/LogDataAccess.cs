using System.Collections.Generic;
using DataAccess.Interface;
using Domain;

namespace DataAccess
{
    public class LogDataAccess : ILogDataAccess
    {
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