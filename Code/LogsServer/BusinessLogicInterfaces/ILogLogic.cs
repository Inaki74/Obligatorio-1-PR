using System;
using System.Collections.Generic;
using Domain;

namespace BusinessLogicInterfaces
{
    public interface ILogLogic
    {
        bool Add(Log log);

        List<Log> Get(string username="", string gamename="", DateTime? date=null);
    }
}