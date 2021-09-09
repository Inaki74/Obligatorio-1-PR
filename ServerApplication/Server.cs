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

            var serverIpEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.175"), 6000);
           
            var tcpServer = new TcpClient(serverIpEndPoint);

            tcpServer.Start(100);

            Console.WriteLine("Listening for clients...");

            while(true)
            {
                var clientFound = tcpServer.AcceptTcpClient();
                Console.WriteLine("Client found!");

                var clientThread = new Thread(() => HandleClient(clientFound));
                clientThread.Start();
            }
        }

        private void HandleClient(TcpClient acceptedTcpClient)
        {
            try
            {
                NetworkStream stream = acceptedTcpClient.GetStream();

                stream.Read();
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
