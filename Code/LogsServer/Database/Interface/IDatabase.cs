using System.Collections.Concurrent;
using System.Collections.Generic;
using ServerDomain;

namespace Database.Interface
{
    public interface IDatabase
    {
        ConcurrentDictionary<string, Dictionary<string, List<Log>>> ErrorLogs {get;}
        ConcurrentDictionary<string, Dictionary<string, List<Log>>> InfoLogs {get;}
    }
}
