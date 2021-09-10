using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;

namespace ServerApplication
{
    class ServerMain
    {
        private static void Main(string[] args)
        {
            
            
            ServerHandler serverHandler = new ServerHandler();
            serverHandler.StartServer();
            serverHandler.Loop();
            
            
        }

        
    }
}
