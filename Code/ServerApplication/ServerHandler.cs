using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading;
using Business;
using BusinessInterfaces;
using Common.Commands;
using Common.Configuration;
using Common.Configuration.Interfaces;
using Common.FileSystemUtilities;
using Common.FileSystemUtilities.Interfaces;
using Common.NetworkUtilities;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;
using Exceptions.ConnectionExceptions;
using ServerApplicationInterfaces;
using Exceptions;
using System.Threading.Tasks;

namespace ServerApplication
{
    public class ServerHandler : IServerHandler
    {
        public static IServerHandler Instance   
        {
            get
            {
                return IServerHandler.Instance;
            }
        }
        
        private static int MAX_SECONDS_WASTED = 5000;

        private readonly IConfigurationHandler _configurationHandler;
        private readonly IPEndPoint _serverIpEndPoint;
        private readonly Socket _serverSocket;
        
        private List<Socket> _clientSockets = new List<Socket>();
        private List<ClientCommandExecutionStatus> _clientConnections = new List<ClientCommandExecutionStatus>();
        
        private int _currentTaskId = 0;
        private bool _serverRunning;

        public ServerHandler()
        {
            if(IServerHandler.Instance == null)
            {
                IServerHandler.Instance = this;
            }
            else
            {
                throw new Exception("Singleton already instanced. Do not instance singleton twice!");
            }

            _configurationHandler = new ConfigurationHandler();

            string serverIp = _configurationHandler.GetField(ConfigurationConstants.SERVER_IP_KEY);
            int serverPort = int.Parse(_configurationHandler.GetField(ConfigurationConstants.SERVER_PORT_KEY));
            
            _serverIpEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
            _serverSocket = new Socket(AddressFamily.InterNetwork,
                                       SocketType.Stream,
                                       ProtocolType.Tcp);

            _serverSocket.Bind(_serverIpEndPoint);
        }

        public bool StartServer()
        {
            _serverSocket.Listen(100);

            _serverRunning = true;

            return true; 
        }

        public void CloseServer()
        {
            _serverRunning = false;
            if (!AllClientsFinishedExecuting())
            { 
                Console.WriteLine("Waiting for clients to finish their commands...");
                System.Threading.Thread.Sleep(MAX_SECONDS_WASTED);
            }
            foreach (Socket client in _clientSockets)
            {
                client.Shutdown(SocketShutdown.Both); //Close connection gracefully?
                client.Close();
            }
            FakeTcpConnection();
        }

        public async Task StartClientListeningTask()
        {
            await Task.Run(async() => await ListenForClients().ConfigureAwait(false));
        }

        private async Task ListenForClients()
        {
            while(_serverRunning)
            {
                var foundClient = await _serverSocket.AcceptAsync();

                if(_serverRunning)
                {
                    await Task.Run(async() => await StartClientTask(foundClient).ConfigureAwait(false));

                    _clientSockets.Add(foundClient);
                }
                else
                {
                    foundClient.Shutdown(SocketShutdown.Both); 
                    foundClient.Close();
                }
                
            }
            
            _serverSocket.Close();
            Console.WriteLine("Server closed!");
        }

        private async Task StartClientTask(Socket acceptedClientSocket)
        {
            await Task.Run(async() => await HandleClient(acceptedClientSocket).ConfigureAwait(false));
        }
        
        private async Task HandleClient(Socket acceptedClientSocket)
        {
            int taskId = _currentTaskId;
            _currentTaskId++;
            _clientConnections.Add(new ClientCommandExecutionStatus(taskId));
            string username = "";
            try
            {
                IStreamHandler streamHandler = new SocketStreamHandler(acceptedClientSocket);
                VaporProtocol vp = new VaporProtocol(streamHandler);
                IServerCommandHandler serverCommandHandler = new ServerCommandHandler();
                bool connected = true;

                while (connected && _serverRunning)
                {
                    VaporProcessedPacket processedPacket = await vp.ReceiveCommandAsync();
                    SetStatusOfExecuting(true, taskId);

                    ProcessCommandPair pair = await ProcessCommandAsync(vp, processedPacket, serverCommandHandler, connected, username);

                    connected = pair.Connected;
                    username = pair.Username;

                    SetStatusOfExecuting(false, taskId);
                }
            }
            catch (EndpointClosedSocketException ecsock)
            {
                LogoutUserInException(username);
            }
            catch (EndpointClosedByServerSocketException ecserv)
            {
                LogoutUserInException(username);
            }
            catch (SocketException e)
            {
                LogoutUserInException(username);
                Console.WriteLine($"Something went wrong: {e.Message}");
            }
            finally
            {
                SetStatusOfExecuting(false, taskId);
            }
        }
        

        private string ExtractEncodedGame(string response)
        {
            return response.Substring(VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE,
                response.Length - VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE);
        }

        private bool AllClientsFinishedExecuting()
        {
            return _clientConnections.All(c => !c.ExecutingCommand);
        }

        private void SetStatusOfExecuting(bool isExecuting, int threadId)
        {
            ClientCommandExecutionStatus connection = _clientConnections.First(c => c.ConnectionId == threadId);
            connection.ExecutingCommand = isExecuting;
        }

        private void FakeTcpConnection()
        {
            string serverIp = _configurationHandler.GetField(ConfigurationConstants.SERVER_IP_KEY);
            int serverPort = int.Parse(_configurationHandler.GetField(ConfigurationConstants.SERVER_PORT_KEY));
            
            IPEndPoint clientIpEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), 0);
            IPEndPoint serverIpEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
            
            Socket fakeSocket = new Socket(AddressFamily.InterNetwork,
                                       SocketType.Stream,
                                       ProtocolType.Tcp);
            fakeSocket.Connect(serverIpEndPoint);
        }


        private async Task<ProcessCommandPair> ProcessCommandAsync(VaporProtocol vp,VaporProcessedPacket processedPacket , IServerCommandHandler serverCommandHandler, bool connected, string username)
        {
            CommandResponse response = serverCommandHandler.ExecuteCommand(processedPacket);
            await vp.SendCommandAsync(ReqResHeader.RES, response.Command, response.Response);

            if(response.Command == CommandConstants.COMMAND_PUBLISH_GAME_CODE || response.Command == CommandConstants.COMMAND_MODIFY_GAME_CODE)
            {
                RecieveClientGameCoverAsync(vp);
            }
                    
            if (response.Command == CommandConstants.COMMAND_DOWNLOAD_COVER_CODE)
            {
                SendClientGameCoverAsync(vp, response);
            }
                    
            if (response.Command == CommandConstants.COMMAND_EXIT_CODE)
            {
                connected = false;
            }

            if (response.Command == CommandConstants.COMMAND_LOGIN_CODE)
            {
                string responseWithoutStatusCode = RemoveStatusCode(response.Response);
                username = responseWithoutStatusCode;
            }

            return new ProcessCommandPair(username, connected);
        }
        private string RemoveStatusCode(string response)
        {
            int statusCodeFixedSize = VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE;
            string message = response.Substring(statusCodeFixedSize, response.Length-statusCodeFixedSize);
            return message;
        }
        
        private async Task RecieveClientGameCoverAsync(VaporProtocol vp)
        {
            string path = _configurationHandler.GetPathFromAppSettings();
            try
            {
                await vp.ReceiveCoverAsync(path);
            }
            catch (CoverNotReceivedException cre)
            {
            }
            catch(FileWritingException fwe)
            {
            }
        }

        private async Task SendClientGameCoverAsync(VaporProtocol vp, CommandResponse response)
        {
            string encodedGame = ExtractEncodedGame(response.Response);
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
            Game gameDummy = gameNTO.Decode(encodedGame);

            IPathHandler pathHandler = new PathHandler();
            string path = pathHandler.AppendPath(_configurationHandler.GetPathFromAppSettings(),$"{gameDummy.Id}.png");

            try
            {
                await vp.SendCoverAsync(gameDummy.Title + "-COVER" , path);
            }
            catch(FileReadingException fre)
            {
                await vp.SendCoverFailedAsync();
            }
        }

        private void LogoutUserInException(string username)
        {
            IUserLogic userLogic = new UserLogic();
            User userDummy = new User(username,User.DEFAULT_USER_ID);
            userLogic.Logout(userDummy);
        }

        
    }
}