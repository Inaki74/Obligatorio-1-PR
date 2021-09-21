using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;
using ConsoleMenusFactory;
using ConsoleMenusInterfaces;
using System.Configuration;
using Common.Configuration;

namespace ServerApplication
{
    class ServerMain
    {
        private static void Main(string[] args)
        {
            new ServerHandler();
            ConsoleMenuManagerFactory consoleFactory = new ConsoleMenuManagerFactory();
            IConsoleMenuManager consoleManager = consoleFactory.Create(false);
            
            // 2 threads
            // 1. El que hace listen de clientes y los acepta.
            // 2. El menu del server (que tiene el exit en esta iteracion)
            while(!consoleManager.Exit)
            {
                consoleManager.ExecuteMenu();
            }
        }

        
    }
}
