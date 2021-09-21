using System;
using Common.Configuration.Interfaces;
using System.Configuration;

namespace Common.Configuration
{
    public class ConfigurationHandler : IConfigurationHandler
    {
        public string GetField(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if(string.IsNullOrEmpty(appSettings[key]))
                {
                    return string.Empty;
                }

                return appSettings[key];
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);

                return "";
            }
        }
    }
}
