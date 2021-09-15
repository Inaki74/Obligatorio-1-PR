using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common.ConsoleMenus.Interfaces;
using Common.ConsoleMenus.Factory;
namespace ClientApplication
{
    public class ClientHandler
    {
        private readonly IPEndPoint _clientIpEndPoint;
        private readonly IPEndPoint _serverIpEndPoint;
        private readonly TcpClient _tcpClient;
        
        public ClientHandler()
        {
            //TODO: Create config file with IP and Port
            _clientIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
            _serverIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);
            _tcpClient = new TcpClient(_clientIpEndPoint);
        }

        public void StartClient()
        {
            Console.WriteLine("Client starting...");
            
            Console.WriteLine("Trying to connect to server");
            
            _tcpClient.Connect(_serverIpEndPoint);
            
            Console.WriteLine("Found server!");
        }

        public void Loop()
        {
            

            

            SendMessage();
        }
        
        private void SendMessage()
        {
            using (var networkStream = _tcpClient.GetStream())
            {
                var word = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(word);

                networkStream.Write(data, 0, data.Length);
            }

            _tcpClient.Close();
        }
    }
}