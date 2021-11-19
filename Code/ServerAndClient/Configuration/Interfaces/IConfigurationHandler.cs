using System;

namespace Configuration.Interfaces
{
    public interface IConfigurationHandler
    {
        string GetField(string key);

        string GetPathFromAppSettings();
    }
}
