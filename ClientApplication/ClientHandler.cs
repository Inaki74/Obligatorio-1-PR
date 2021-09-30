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
using Domain.BusinessObjects;
using Domain.HelperObjects;
using Common.Protocol.Interfaces;
using Common.Protocol.NTOs;
using Common.Configuration.Interfaces;
using Common.Configuration;
using System.Collections.Generic;
using Exceptions;

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
            }
            //TODO: ACTUALLY HANDLE EXCEPTIONS!!!
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

            SendCommand<Game>(CommandConstants.COMMAND_PUBLISH_GAME_CODE, game);
            VaporStatusResponse response = ExecuteCommand();

            try
            {
                _vaporProtocol.SendCover(response.SelectedGameId.ToString(), game.CoverPath);
            }
            catch(FileReadingException fre)
            {
                response.Message = fre.Message;
                response.Code = StatusCodeConstants.ERROR_CLIENT;
            }
            
            return response;
        }

        public string DeleteGame()
        {
            GameNetworkTransferObject game = new GameNetworkTransferObject();

            game.ID = _clientSession.GameSelectedId;

            SendCommand<Game>(CommandConstants.COMMAND_DELETE_GAME_CODE, game);
            VaporStatusResponse response = ExecuteCommand();
            
            return response.Message;
        }

        public string ModifyGame(GameNetworkTransferObject game)
        {
            game.OwnerName = _clientSession.Username;
            game.ID = _clientSession.GameSelectedId;

            SendCommand<Game>(CommandConstants.COMMAND_MODIFY_GAME_CODE, game);
            VaporStatusResponse response = ExecuteCommand();

            try
            {
                _vaporProtocol.SendCover(response.SelectedGameId.ToString(), game.CoverPath);
            }
            catch(FileReadingException fre)
            {
                response.Message = fre.Message;
                response.Code = StatusCodeConstants.ERROR_CLIENT;
            }
            
            return response.Message;
        }

        public string PublishReview(ReviewNetworkTransferObject review)
        {
            review.Username = _clientSession.Username;
            review.Gameid = _clientSession.GameSelectedId;

            SendCommand<Review>(CommandConstants.COMMAND_PUBLISH_REVIEW_CODE, review);
            VaporStatusResponse response = ExecuteCommand();
            
            return response.Message;
        }

        public VaporStatusResponse CheckIsOwner()
        {
            GameUserRelationQuery query = new GameUserRelationQuery();
            query.Username = _clientSession.Username;
            query.Gameid = _clientSession.GameSelectedId;

            GameUserRelationQueryNetworkTransferObject queryNTO = new GameUserRelationQueryNetworkTransferObject();
            queryNTO.Load(query);

            SendCommand<GameUserRelationQuery>(CommandConstants.COMMAND_CHECKOWNERSHIP_GAME_CODE, queryNTO);
            VaporStatusResponse response = ExecuteCommand();

            return response;
        }

        public VaporStatusResponse GetGames()
        {
            SendCommand<Game>(CommandConstants.COMMAND_GET_GAMES_CODE, null);
            VaporStatusResponse response = ExecuteCommand();

            return response;
        }

        public VaporStatusResponse GetGameScore()
        {
            GameNetworkTransferObject game = new GameNetworkTransferObject();

            game.ID = _clientSession.GameSelectedId;

            SendCommand<Game>(CommandConstants.COMMAND_GET_GAME_SCORE_CODE, game);
            VaporStatusResponse response = ExecuteCommand();

            return response;
        }

        public VaporStatusResponse GetGameReview(string username)
        {
            ReviewNetworkTransferObject review = new ReviewNetworkTransferObject();
            review.Gameid = _clientSession.GameSelectedId;
            review.Username = username;
            
            SendCommand<Review>(CommandConstants.COMMAND_VIEW_REVIEW_CODE, review);
            VaporStatusResponse response = ExecuteCommand();
            
            return response;
        }

        public VaporStatusResponse GetGameDetails()
        {
            GameNetworkTransferObject game = new GameNetworkTransferObject();

            game.ID = _clientSession.GameSelectedId;
            
            SendCommand<Game>(CommandConstants.COMMAND_VIEW_DETAILS_CODE, game);
            VaporStatusResponse response = ExecuteCommand();

            return response;
        }

        public VaporStatusResponse DownloadGameCover(string path)
        {
            GameNetworkTransferObject game = new GameNetworkTransferObject();

            game.ID = _clientSession.GameSelectedId;
            SendCommand<Game>(CommandConstants.COMMAND_DOWNLOAD_COVER_CODE, game);
            VaporStatusResponse response = ExecuteCommand();

            try
            {
                _vaporProtocol.ReceiveCover(path);
            }
            catch(FileWritingException fwe)
            {
                response.Message = fwe.Message;
                response.Code = StatusCodeConstants.ERROR_CLIENT;
            }
            
            return response;
        }

        public VaporStatusResponse SearchGames(GameSearchQueryNetworkTransferObject query)
        {
            SendCommand<GameSearchQuery>(CommandConstants.COMMAND_SEARCH_GAMES_CODE, query);
            VaporStatusResponse response = ExecuteCommand();

            return response;
        }

        public VaporStatusResponse Login(UserNetworkTransferObject user)
        {
            SendCommand<User>(CommandConstants.COMMAND_LOGIN_CODE, user);
            VaporStatusResponse response = ExecuteCommand();

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

            SendCommand<Game>(CommandConstants.COMMAND_SELECT_GAME_CODE, gameDummy);
            VaporStatusResponse response = ExecuteCommand();
            
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

            SendCommand<GameUserRelationQuery>(CommandConstants.COMMAND_ACQUIRE_GAME_CODE, query);
            VaporStatusResponse response = ExecuteCommand();
            
            return response;
        }

        public string Exit()
        {
            UserNetworkTransferObject user = new UserNetworkTransferObject();
            user.Username = _clientSession.Username;

            SendCommand<User>(CommandConstants.COMMAND_EXIT_CODE, user);
            VaporStatusResponse response = ExecuteCommand();

            _tcpClient.Close();
            return response.Message;
        }

        // Send information to the Server and execute command when we receive a response.
        // command is the command key.
        // payload is what to send wrapped in a NTO.
        // P is the type of payload the NTO brings.
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

        private List<string> GetOnlyTitles(List<Game> games)
        {
            List<string> ret = new List<string>();
            foreach(Game game in games)
            {
                ret.Add(game.Title);
            }

            return ret;
        }
    }
}
