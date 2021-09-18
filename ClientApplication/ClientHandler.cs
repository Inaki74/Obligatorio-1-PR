using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common.Commands;
using ClientApplicationInterfaces;
using Common.Interfaces;
using Common.Protocol;
using Common.NetworkUtilities;
using Common;
using Domain;

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

        private IClientSession _clientSession;
        private IClientCommandHandler _commandHandler;
        
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
                //_commandHandler = new ClientCommandHandler();
            }
            catch(Exception e)
            {
                Console.WriteLine($"An unexpected error ocurred {e.Message}");
                return false;
            }
            
            return true;
        }

        public VaporStatusResponse PublishGame(string game)
        {
            VaporStatusResponse response = ExecuteCommand(CommandConstants.COMMAND_LOGIN_CODE, game);

            return response;
        }

        public VaporStatusResponse Login(string username)
        {
            VaporStatusResponse response = ExecuteCommand(CommandConstants.COMMAND_LOGIN_CODE, username);

            if(response.Code == StatusCodeConstants.OK || response.Code == StatusCodeConstants.INFO)
            {
                _clientSession = new ClientSession(username);
            }

            return response;
        }

        public VaporStatusResponse Exit()
        {
            VaporStatusResponse response = ExecuteCommand(CommandConstants.COMMAND_EXIT_CODE, _clientSession.Username);
            _tcpClient.Close();
            return response;
        }

        private VaporStatusResponse ExecuteCommand(string command, string payload)
        {
            VaporProtocol vp = new VaporProtocol(new NetworkStreamHandler(_tcpClient.GetStream()));
            vp.Send(ReqResHeader.REQ, command, payload.Length, payload);
            VaporProcessedPacket vaporProcessedPacket = vp.Receive();
            IClientCommandHandler clientCommandHandler = new ClientCommandHandler();
            return clientCommandHandler.ExecuteCommand(vaporProcessedPacket);
        }
    }
}
