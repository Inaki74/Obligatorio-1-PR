using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientApplication
{
    class Client
    {
        static void Main(string[] args)
        {

            ClientHandler clientHandler = new ClientHandler();
            
            clientHandler.StartClient();
            
            clientHandler.Loop();

        }
        
    }
}
