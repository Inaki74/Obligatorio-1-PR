using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common.Commands;
using ClientApplicationInterfaces;
using Common.Interfaces;
using Common;
using Common.NetworkUtilities;

namespace ClientApplication
{
    public class ClientHandler : IClientHandler
    {
        public static IClientHandler Instance
        {
            get
            {
                return IClientHandler.Instance;
            }
        }

        private readonly IPEndPoint _clientIpEndPoint;
        private readonly IPEndPoint _serverIpEndPoint;
        private readonly TcpClient _tcpClient;

        private ClientCommandHandler _commandHandler;
        
        public ClientHandler()
        {
            if(IClientHandler.Instance == null)
            {
                IClientHandler.Instance = this;
            }
            else
            {
                throw new Exception("Singleton already instanced. Do not instance singleton twice!");
            }
            
            //TODO: Create config file with IP and Port
            _clientIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
            _serverIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);
            _tcpClient = new TcpClient(_clientIpEndPoint);
        }

        public bool ConnectToServer()
        {
            try
            {
                _tcpClient.Connect(_serverIpEndPoint);
                _commandHandler = new ClientCommandHandler(new NetworkStreamHandler(_tcpClient.GetStream()));
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

        public void Login(string username)
        {
            IPayload payload = new StringPayload(username);
            _commandHandler.ExecuteCommand(CommandConstants.COMMAND_LOGIN_CODE, payload);
            // devuelve un codigo
            // si fue bueno return true
            // si no return false
        }
    }
}