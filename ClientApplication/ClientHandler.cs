using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ClientApplicationInterfaces;

namespace ClientApplication
{
    public class ClientHandler : IClientHandler
    {
        private readonly static IClientHandler _instance = new ClientHandler();
 
        public static IClientHandler Instance
        {
            get
            {
                return _instance;
            }
        }

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

        public bool StartClient()
        {
            try
            {
                _tcpClient.Connect(_serverIpEndPoint);
            }
            catch(Exception e)
            {
                Console.WriteLine($"An unexpected error ocurred {e.Message}");
                return false;
            }
            
            return true;
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