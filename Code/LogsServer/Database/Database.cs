using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Database.Interface;
using ServerDomain;

namespace Database
{
    public class Database : IDatabase
    {
        private ConcurrentDictionary<string, Dictionary<string, List<Log>>> _errorLogs = new ConcurrentDictionary<string, Dictionary<string, List<Log>>>();
        private ConcurrentDictionary<string, Dictionary<string, List<Log>>> _infoLogs = new ConcurrentDictionary<string, Dictionary<string, List<Log>>>();

        public ConcurrentDictionary<string, Dictionary<string, List<Log>>> ErrorLogs => _errorLogs;

        public ConcurrentDictionary<string, Dictionary<string, List<Log>>> InfoLogs => _infoLogs;
    }
}
