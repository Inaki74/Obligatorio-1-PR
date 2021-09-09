using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerApplication
{
    public class ServerHandler
    {
        private readonly IPEndPoint _serverIpEndPoint;
        private readonly TcpListener _tcpServerListener;
        //private tcpClient[] _tcpCLients;

        public ServerHandler()
        {
            //TODO: Create config file with IP and Port 
            _serverIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);
            _tcpServerListener = new TcpListener(_serverIpEndPoint);
        }

        public void StartServer()
        {
            Console.WriteLine("Server starting...");
            _tcpServerListener.Start(100);
        }

        public void Loop()
        {
            Console.WriteLine("Listening for clients...");
            while(true)
            {
                
                var clientFound = _tcpServerListener.AcceptTcpClient();
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