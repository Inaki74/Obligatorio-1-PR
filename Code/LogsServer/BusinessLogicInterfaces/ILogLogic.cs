using System;
using System.Collections.Generic;
using Domain;
using Models;

namespace BusinessLogicInterfaces
{
    public interface ILogLogic
    {
        bool Add(LogModel log);

        List<Log> Get(string username="", string gamename="", DateTime? date=null);
    }
}