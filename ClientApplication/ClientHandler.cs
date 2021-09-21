﻿using System;
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
using Common.Protocol.Interfaces;
using Common.Protocol.NTOs;
using Common.Configuration.Interfaces;
using Common.Configuration;

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

        private readonly IConfigurationHandler _configurationHandler;
        private readonly IPEndPoint _clientIpEndPoint;
        private readonly IPEndPoint _serverIpEndPoint;
        private readonly TcpClient _tcpClient;

        private VaporProtocol _vaporProtocol;
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

            _configurationHandler = new ConfigurationHandler();
            string clientIp = _configurationHandler.GetField(ConfigurationConstants.CLIENT_IP_KEY);
            int clientPort = int.Parse(_configurationHandler.GetField(ConfigurationConstants.CLIENT_PORT_KEY));
            string serverIp = _configurationHandler.GetField(ConfigurationConstants.SERVER_IP_KEY);
            int serverPort = int.Parse(_configurationHandler.GetField(ConfigurationConstants.SERVER_PORT_KEY));

            _clientIpEndPoint = new IPEndPoint(IPAddress.Parse(clientIp), clientPort);
            _serverIpEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
            _tcpClient = new TcpClient(_clientIpEndPoint);
        }

        public bool ConnectToServer()
        {
            try
            {
                _tcpClient.Connect(_serverIpEndPoint);
                _vaporProtocol = new VaporProtocol(new NetworkStreamHandler(_tcpClient.GetStream()));
                //_commandHandler = new ClientCommandHandler();
            }
            catch(Exception e)
            {
                Console.WriteLine($"An unexpected error ocurred {e.Message}");
                return false;
            }
            
            return true;
        }

        public VaporStatusResponse PublishGame(GameNetworkTransferObject game)
        {
            game.OwnerName = _clientSession.Username;
            VaporStatusResponse response = ExecuteCommand(CommandConstants.COMMAND_PUBLISH_GAME_CODE, game);

            // Enviar caratula si corresponde
            _vaporProtocol.SendCover(game.Title, game.CoverPath);
            
            return response;
        }

        public VaporStatusResponse Login(UserNetworkTransferObject user)
        {
            VaporStatusResponse response = ExecuteCommand(CommandConstants.COMMAND_LOGIN_CODE, user);

            if(response.Code == StatusCodeConstants.OK || response.Code == StatusCodeConstants.INFO)
            {
                _clientSession = new ClientSession(user.Username);
            }

            return response;
        }

        public VaporStatusResponse Exit()
        {
            UserNetworkTransferObject user = new UserNetworkTransferObject();
            user.Username = _clientSession.Username;

            VaporStatusResponse response = ExecuteCommand(CommandConstants.COMMAND_EXIT_CODE, user);
            _tcpClient.Close();
            return response;
        }

        private VaporStatusResponse ExecuteCommand(string command, INetworkTransferObject payload)
        {
            _vaporProtocol.SendCommand(ReqResHeader.REQ, command, payload.ToCharacters());
            VaporProcessedPacket vaporProcessedPacket = _vaporProtocol.ReceiveCommand();
            IClientCommandHandler clientCommandHandler = new ClientCommandHandler();
            return clientCommandHandler.ExecuteCommand(vaporProcessedPacket);
        }
    }
}
