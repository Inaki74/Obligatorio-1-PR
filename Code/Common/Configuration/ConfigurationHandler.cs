using System;
using Common.Configuration.Interfaces;
using System.Configuration;
using System.Runtime.InteropServices;

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

        public string GetPathFromAppSettings()
        {
            IConfigurationHandler _configurationHandler = new ConfigurationHandler();
            string path = "";
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = _configurationHandler.GetField(ConfigurationConstants.WIN_SERVER_IMAGEPATH_KEY);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                path = _configurationHandler.GetField(ConfigurationConstants.OSX_SERVER_IMAGEPATH_KEY);
            }

            return path;
        }
    }
}
