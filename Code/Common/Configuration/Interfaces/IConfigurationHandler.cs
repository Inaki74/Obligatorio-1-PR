using System;

namespace Common.Configuration.Interfaces
{
    public interface IConfigurationHandler
    {
        string GetField(string key);

        string GetPathFromAppSettings();
    }
}
