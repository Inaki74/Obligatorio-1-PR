using System;
using System.Collections.Generic;
using Domain;

namespace DataAccess.Interface
{
    public interface ILogDataAccess
    {
        List<Log> GetAll();
        List<Log> Get(string username);
        List<Log> Get(string username, string gamename);
    }
}
