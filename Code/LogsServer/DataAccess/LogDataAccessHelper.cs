using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Interface;
using Database.Interface;
using ServerDomain;

namespace DataAccess
{
    public class LogDataAccessHelper
    {

        public static bool AddLogsToDictionary(ConcurrentDictionary<string, Dictionary<string, List<Log>>> dictionary, string nameIfEmpty, Log log)
        {
            bool done = false;

            try
            {
                if(dictionary.ContainsKey(log.Username))
                {
                    dictionary[log.Username][log.Gamename].Add(log);
                }
                else
                {
                    Dictionary<string, List<Log>> firstDictionary = new Dictionary<string, List<Log>>();
                    List<Log> firstList = new List<Log>();
                    firstList.Add(log);

                    if(String.IsNullOrEmpty(log.Gamename))
                    {
                        firstDictionary.Add(log.Gamename, firstList);
                    }
                    else
                    {
                        firstDictionary.Add(nameIfEmpty, firstList);
                    }

                    dictionary.AddOrUpdate(log.Username, (str) => firstDictionary, (str,dict) => firstDictionary);
                }
                
                done = true;
            }
            catch(Exception e)
            {
                done = false;
            }

            return done;
        }

        public static List<Log> GetLogsFromDictionary(Dictionary<string, List<Log>> logs)
        {
            List<Log> finalList = new List<Log>();

            foreach(KeyValuePair<string, List<Log>> gameLogs in logs)
            {
                finalList.Union(gameLogs.Value);
            }

            return finalList;
        }
        public static List<Log> GetUserLogsFromConcurrentDictionary(ConcurrentDictionary<string, Dictionary<string, List<Log>>> logs)
        {
            List<Log> finalList = new List<Log>();

            foreach(KeyValuePair<string, Dictionary<string, List<Log>>> userLogs in logs)
            {
                finalList.Union(GetLogsFromDictionary(userLogs.Value));
            }

            return finalList;
        }

        public static List<Log> GetGameLogsFromConcurrentDictionary(ConcurrentDictionary<string, Dictionary<string, List<Log>>> logs, string gamename)
        {
            List<Log> finalList = new List<Log>();

            foreach(KeyValuePair<string, Dictionary<string, List<Log>>> userLogs in logs)
            {
                finalList.Union(GetLogsFromDictionary(userLogs.Value).Where(l => l.Gamename == gamename));
            }

            return finalList;
        }
    }
}