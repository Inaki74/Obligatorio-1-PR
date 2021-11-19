using System;
using System.Collections.Generic;
using ServerDomain;

namespace DataAccess.Interface
{
    public interface ILogDataAccess<T> where T : Log
    {
        bool Add(Log log);
        List<Log> GetAll();
        List<Log> Get(string username);
        List<Log> Get(string username, string gamename);
    }
}
