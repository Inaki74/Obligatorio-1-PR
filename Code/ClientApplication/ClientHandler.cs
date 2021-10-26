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
using System.Threading.Tasks;
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
        private readonly Socket _clientSocket;

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
            _clientSocket = new Socket(AddressFamily.InterNetwork,
                                       SocketType.Stream,
                                       ProtocolType.Tcp);
        }

        public async Task<bool> ConnectToServerAsync()
        {
            try
            {
                await _clientSocket.ConnectAsync(_serverIpEndPoint);
                _vaporProtocol = new VaporProtocol(new SocketStreamHandler(_clientSocket));
            }
            catch(Exception e)
            {
                Console.WriteLine($"Couldn't connect to server: {e.Message}");
                return false;
            }
            
            return true;
        }

        public async Task<VaporStatusResponse> PublishGameAsync(GameNetworkTransferObject game)
        {
            game.OwnerName = _clientSession.Username;

            VaporStatusResponse response = TryCommandExecutionAsync<Game>(CommandConstants.COMMAND_PUBLISH_GAME_CODE, game).Result;

            try
            {
                await _vaporProtocol.SendCoverAsync(response.SelectedGameId.ToString(), game.CoverPath);
            }
            catch(FileReadingException fre)
            {
                await _vaporProtocol.SendCoverFailedAsync();
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

            VaporStatusResponse response = TryCommandExecutionAsync<Game>(CommandConstants.COMMAND_DELETE_GAME_CODE, game).Result;
            
            return response.Message;
        }

        public async Task<string> ModifyGame(GameNetworkTransferObject game)
        {
            game.OwnerName = _clientSession.Username;
            game.ID = _clientSession.GameSelectedId;

            VaporStatusResponse response = TryCommandExecutionAsync<Game>(CommandConstants.COMMAND_MODIFY_GAME_CODE, game).Result;

            try
            {
                await _vaporProtocol.SendCoverAsync(response.SelectedGameId.ToString(), game.CoverPath);
            }
            catch(FileReadingException fre)
            {
                await _vaporProtocol.SendCoverFailedAsync();
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

            VaporStatusResponse response = TryCommandExecutionAsync<Review>(CommandConstants.COMMAND_PUBLISH_REVIEW_CODE, review).Result;
            
            return response.Message;
        }

        public VaporStatusResponse CheckIsOwner()
        {
            GameUserRelationQuery query = new GameUserRelationQuery();
            query.Username = _clientSession.Username;
            query.Gameid = _clientSession.GameSelectedId;

            GameUserRelationQueryNetworkTransferObject queryNTO = new GameUserRelationQueryNetworkTransferObject();
            queryNTO.Load(query);

            VaporStatusResponse response = TryCommandExecutionAsync<GameUserRelationQuery>(CommandConstants.COMMAND_CHECKOWNERSHIP_GAME_CODE, queryNTO).Result;

            return response;
        }

        public VaporStatusResponse GetGames()
        {
            VaporStatusResponse response = TryCommandExecutionAsync<Game>(CommandConstants.COMMAND_GET_GAMES_CODE, null).Result;

            return response;
        }

        public VaporStatusResponse GetGameScore()
        {
            GameNetworkTransferObject game = new GameNetworkTransferObject();

            game.ID = _clientSession.GameSelectedId;

            VaporStatusResponse response = TryCommandExecutionAsync<Game>(CommandConstants.COMMAND_GET_GAME_SCORE_CODE, game).Result;

            return response;
        }

        public VaporStatusResponse GetGameReview(string username)
        {
            ReviewNetworkTransferObject review = new ReviewNetworkTransferObject();
            review.Gameid = _clientSession.GameSelectedId;
            review.Username = username;
            
            VaporStatusResponse response = TryCommandExecutionAsync<Review>(CommandConstants.COMMAND_VIEW_REVIEW_CODE, review).Result;
            
            return response;
        }

        public VaporStatusResponse GetGameDetails()
        {
            GameNetworkTransferObject game = new GameNetworkTransferObject();

            game.ID = _clientSession.GameSelectedId;
            
            VaporStatusResponse response = TryCommandExecutionAsync<Game>(CommandConstants.COMMAND_VIEW_DETAILS_CODE, game).Result;

            return response;
        }

        public async Task<VaporStatusResponse> DownloadGameCover(string path)
        {
            GameNetworkTransferObject game = new GameNetworkTransferObject();

            game.ID = _clientSession.GameSelectedId;
            VaporStatusResponse response = TryCommandExecutionAsync<Game>(CommandConstants.COMMAND_DOWNLOAD_COVER_CODE, game).Result;

            try
            {
                await _vaporProtocol.ReceiveCoverAsync(path);
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
            VaporStatusResponse response = TryCommandExecutionAsync<GameSearchQuery>(CommandConstants.COMMAND_SEARCH_GAMES_CODE, query).Result;

            return response;
        }

        public VaporStatusResponse Login(UserNetworkTransferObject user)
        {
            VaporStatusResponse response = TryCommandExecutionAsync<User>(CommandConstants.COMMAND_LOGIN_CODE, user).Result;

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

            VaporStatusResponse response = TryCommandExecutionAsync<Game>(CommandConstants.COMMAND_SELECT_GAME_CODE, gameDummy).Result;
            
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

            VaporStatusResponse response = TryCommandExecutionAsync<GameUserRelationQuery>(CommandConstants.COMMAND_ACQUIRE_GAME_CODE, query).Result;
            
            return response;
        }

        public async Task<string> ExitAsync()
        {
            UserNetworkTransferObject user = new UserNetworkTransferObject();
            user.Username = _clientSession.Username;

            VaporStatusResponse response = new VaporStatusResponse();
            try
            {
                await SendCommandAsync<User>(CommandConstants.COMMAND_EXIT_CODE, user);
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

            _clientSocket.Shutdown(SocketShutdown.Both); 
            _clientSocket.Close();

            return response.Message;
        }

        private async  Task<VaporStatusResponse> TryCommandExecutionAsync<P>(string command, INetworkTransferObject<P> payload)
        {
            VaporStatusResponse response = new VaporStatusResponse();
            try
            {
                await SendCommandAsync<P>(command, payload);
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
            Task<VaporProcessedPacket> vaporProcessedPacket = _vaporProtocol.ReceiveCommandAsync();
            IClientCommandHandler clientCommandHandler = new ClientCommandHandler();
            return clientCommandHandler.ExecuteCommand(vaporProcessedPacket.Result);
        }

        private async Task SendCommandAsync<P>(string command, INetworkTransferObject<P> payload)
        {
            string payloadString = "";
            if(payload != null)
            {
                payloadString = payload.Encode();
            }

            await _vaporProtocol.SendCommandAsync(ReqResHeader.REQ, command, payloadString);
        }
    }
}
