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
using Domain.BusinessObjects;
using Domain.HelperObjects;
using Common.Protocol.Interfaces;
using Common.Protocol.NTOs;
using Common.Configuration.Interfaces;
using Common.Configuration;
using System.Collections.Generic;
using Exceptions;
using Exceptions.ConnectionExceptions;
using Exceptions.BusinessExceptions;

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
            }
            catch(Exception e)
            {
                Console.WriteLine($"Couldn't connecto to server: {e.Message}");
                return false;
            }
            
            return true;
        }

        public VaporStatusResponse PublishGame(GameNetworkTransferObject game)
        {
            game.OwnerName = _clientSession.Username;

            VaporStatusResponse response = TryCommandExecution<Game>(CommandConstants.COMMAND_PUBLISH_GAME_CODE, game);

            try
            {
                _vaporProtocol.SendCover(response.SelectedGameId.ToString(), game.CoverPath);
            }
            catch(FileReadingException fre)
            {
                _vaporProtocol.SendCoverFailed();
                response.Message = fre.Message;
                response.Code = StatusCodeConstants.ERROR_CLIENT;
            }
            catch(EndpointClosedByServerSocketException ecserv)
            {
                response.Code = StatusCodeConstants.ERROR_SERVER;
                response.Message = ecserv.Message;
                throw new ExitException();
            }
            
            
            return response;
        }

        public string DeleteGame()
        {
            GameNetworkTransferObject game = new GameNetworkTransferObject();

            game.ID = _clientSession.GameSelectedId;

            VaporStatusResponse response = TryCommandExecution<Game>(CommandConstants.COMMAND_DELETE_GAME_CODE, game);
            
            return response.Message;
        }

        public string ModifyGame(GameNetworkTransferObject game)
        {
            game.OwnerName = _clientSession.Username;
            game.ID = _clientSession.GameSelectedId;

            VaporStatusResponse response = TryCommandExecution<Game>(CommandConstants.COMMAND_MODIFY_GAME_CODE, game);

            try
            {
                _vaporProtocol.SendCover(response.SelectedGameId.ToString(), game.CoverPath);
            }
            catch(FileReadingException fre)
            {
                _vaporProtocol.SendCoverFailed();
                response.Message = fre.Message;
                response.Code = StatusCodeConstants.ERROR_CLIENT;
            }
            catch(EndpointClosedByServerSocketException ecserv)
            {
                response.Code = StatusCodeConstants.ERROR_SERVER;
                response.Message = ecserv.Message;
                throw new ExitException();
            }
            
            return response.Message;
        }

        public string PublishReview(ReviewNetworkTransferObject review)
        {
            review.Username = _clientSession.Username;
            review.Gameid = _clientSession.GameSelectedId;

            VaporStatusResponse response = TryCommandExecution<Review>(CommandConstants.COMMAND_PUBLISH_REVIEW_CODE, review);
            
            return response.Message;
        }

        public VaporStatusResponse CheckIsOwner()
        {
            GameUserRelationQuery query = new GameUserRelationQuery();
            query.Username = _clientSession.Username;
            query.Gameid = _clientSession.GameSelectedId;

            GameUserRelationQueryNetworkTransferObject queryNTO = new GameUserRelationQueryNetworkTransferObject();
            queryNTO.Load(query);

            VaporStatusResponse response = TryCommandExecution<GameUserRelationQuery>(CommandConstants.COMMAND_CHECKOWNERSHIP_GAME_CODE, queryNTO);

            return response;
        }

        public VaporStatusResponse GetGames()
        {
            VaporStatusResponse response = TryCommandExecution<Game>(CommandConstants.COMMAND_GET_GAMES_CODE, null);

            return response;
        }

        public VaporStatusResponse GetGameScore()
        {
            GameNetworkTransferObject game = new GameNetworkTransferObject();

            game.ID = _clientSession.GameSelectedId;

            VaporStatusResponse response = TryCommandExecution<Game>(CommandConstants.COMMAND_GET_GAME_SCORE_CODE, game);

            return response;
        }

        public VaporStatusResponse GetGameReview(string username)
        {
            ReviewNetworkTransferObject review = new ReviewNetworkTransferObject();
            review.Gameid = _clientSession.GameSelectedId;
            review.Username = username;
            
            VaporStatusResponse response = TryCommandExecution<Review>(CommandConstants.COMMAND_VIEW_REVIEW_CODE, review);
            
            return response;
        }

        public VaporStatusResponse GetGameDetails()
        {
            GameNetworkTransferObject game = new GameNetworkTransferObject();

            game.ID = _clientSession.GameSelectedId;
            
            VaporStatusResponse response = TryCommandExecution<Game>(CommandConstants.COMMAND_VIEW_DETAILS_CODE, game);

            return response;
        }

        public VaporStatusResponse DownloadGameCover(string path)
        {
            GameNetworkTransferObject game = new GameNetworkTransferObject();

            game.ID = _clientSession.GameSelectedId;
            VaporStatusResponse response = TryCommandExecution<Game>(CommandConstants.COMMAND_DOWNLOAD_COVER_CODE, game);

            try
            {
                _vaporProtocol.ReceiveCover(path);
            }
            catch (CoverNotReceivedException cre)
            {
                response.Message = cre.Message;
                response.Code = StatusCodeConstants.ERROR_STREAM;
            }
            catch(FileWritingException fwe)
            {
                response.Message = fwe.Message;
                response.Code = StatusCodeConstants.ERROR_CLIENT;
            }
            catch(NetworkReadException nre)
            {
                response.Code = StatusCodeConstants.ERROR_STREAM;
                response.Message = nre.Message;
            }
            
            return response;
        }

        public VaporStatusResponse SearchGames(GameSearchQueryNetworkTransferObject query)
        {
            VaporStatusResponse response = TryCommandExecution<GameSearchQuery>(CommandConstants.COMMAND_SEARCH_GAMES_CODE, query);

            return response;
        }

        public VaporStatusResponse Login(UserNetworkTransferObject user)
        {
            VaporStatusResponse response = TryCommandExecution<User>(CommandConstants.COMMAND_LOGIN_CODE, user);

            if(response.Code == StatusCodeConstants.OK || response.Code == StatusCodeConstants.INFO)
            {
                _clientSession = new ClientSession(user.Username);
            }

            return response;
        }

        public VaporStatusResponse SelectGame(string game)
        {
            GameNetworkTransferObject gameDummy = new GameNetworkTransferObject();
            gameDummy.Title = game;

            VaporStatusResponse response = TryCommandExecution<Game>(CommandConstants.COMMAND_SELECT_GAME_CODE, gameDummy);
            
            if (response.Code == StatusCodeConstants.OK)
            {
                _clientSession.GameSelectedId = response.SelectedGameId;
            }

            return response;
        }

        public VaporStatusResponse AcquireGame()
        {
            GameUserRelationQueryNetworkTransferObject query = new GameUserRelationQueryNetworkTransferObject();
            query.Gameid = _clientSession.GameSelectedId;
            query.Username = _clientSession.Username;

            VaporStatusResponse response = TryCommandExecution<GameUserRelationQuery>(CommandConstants.COMMAND_ACQUIRE_GAME_CODE, query);
            
            return response;
        }

        public string Exit()
        {
            UserNetworkTransferObject user = new UserNetworkTransferObject();
            user.Username = _clientSession.Username;

            VaporStatusResponse response = new VaporStatusResponse();
            try
            {
                SendCommand<User>(CommandConstants.COMMAND_EXIT_CODE, user);
                response = ExecuteCommand();
            }
            catch(NetworkReadException nre)
            {
                response.Code = StatusCodeConstants.ERROR_STREAM;
                response.Message = nre.Message + "Exited application anyways.";
            }
            catch(EndpointClosedByServerSocketException ecserv)
            {
                response.Code = StatusCodeConstants.ERROR_SERVER;
                response.Message = ecserv.Message + "Lowered connection.";
            }

            _tcpClient.Close();

            return response.Message;
        }

        private VaporStatusResponse TryCommandExecution<P>(string command, INetworkTransferObject<P> payload)
        {
            VaporStatusResponse response = new VaporStatusResponse();
            try
            {
                SendCommand<P>(command, payload);
                response = ExecuteCommand();
            }
            catch(NetworkReadException nre)
            {
                response.Code = StatusCodeConstants.ERROR_STREAM;
                response.Message = nre.Message;
            }
            catch(EndpointClosedByServerSocketException ecserv)
            {
                response.Code = StatusCodeConstants.ERROR_SERVER;
                response.Message = ecserv.Message;
                throw new ExitException();
            }

            return response;
        }
        private VaporStatusResponse ExecuteCommand()
        {
            VaporProcessedPacket vaporProcessedPacket = _vaporProtocol.ReceiveCommand();
            IClientCommandHandler clientCommandHandler = new ClientCommandHandler();
            return clientCommandHandler.ExecuteCommand(vaporProcessedPacket);
        }

        private void SendCommand<P>(string command, INetworkTransferObject<P> payload)
        {
            string payloadString = "";
            if(payload != null)
            {
                payloadString = payload.Encode();
            }

            _vaporProtocol.SendCommand(ReqResHeader.REQ, command, payloadString);
        }
    }
}