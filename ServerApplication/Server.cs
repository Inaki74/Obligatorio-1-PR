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
        private static void Main(string[] args)
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
                var clientThread = new Thread(() => HandleClient(clientFound));
                clientThread.Start();
            }
        }

        private static void HandleClient(TcpClient acceptedTcpClient)
        {
            try
            {
                NetworkStream stream = acceptedTcpClient.GetStream();

                byte[] messageBuffer = new byte[1024];
                stream.Read(messageBuffer, 0, 1024);
                string message = Encoding.UTF8.GetString(messageBuffer);

                Console.WriteLine($"Client says: {message}");
            }
            catch(SocketException e)
            {
                Console.WriteLine($"Something went wrong: {e.Message}");
            }
            finally
            {
                Console.WriteLine("Goodbye client!");
            }
        }
    }
}
