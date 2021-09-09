using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;

namespace ServerApplication
{
    class Server
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server starting...");

            var serverIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);
           
            var tcpServer = new TcpListener(serverIpEndPoint);
            
            tcpServer.Start(100);

            Console.WriteLine("Listening for clients...");

            while(true)
            {
                var clientFound = tcpServer.AcceptTcpClient();
                Console.WriteLine("Client found!");
                return;
                //var clientThread = new Thread(() => HandleClient(clientFound));
                //clientThread.Start();
            }
        }

        private void HandleClient(TcpClient acceptedTcpClient)
        {
            try
            {
                NetworkStream stream = acceptedTcpClient.GetStream();

                //stream.Read();
            }
            catch(SocketException e)
            {

            }
            finally
            {

            }

            

            Console.WriteLine("Goodbye client!");
        }
    }
}
