using System;

namespace Common.Configuration
{
    public class ConfigurationConstants
    {
        //ServerAndCLient
        public const string CLIENT_IP_KEY = "ClientIp";
        public const string CLIENT_PORT_KEY = "ClientPort";
        public const string SERVER_IP_KEY = "ServerIp";
        public const string SERVER_PORT_KEY = "ServerPort";
        public const string WIN_SERVER_IMAGEPATH_KEY = "WindowsImagesPath";
        public const string OSX_SERVER_IMAGEPATH_KEY = "MacImagesPath";
        
        //RabbitMQ
        public const string HOSTNAME_KEY = "HostName";
        public const string INFO_QUEUENAME_KEY = "InfoQueueName";
        public const string ERROR_QUEUENAME_KEY = "ErrorQueueName";
        public const string ERROR_ROUTINGKEY_KEY = "ErrorRoutingKey";
        public const string INFO_ROUTINGKEY_KEY = "InfoRoutingKey";
        public const string EXCHANGENAME_KEY = "ExchangeName";
    }
}
