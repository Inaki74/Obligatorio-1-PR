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
        
        private int _currentThreadId = 0;
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
                client.Shutdown(SocketShutdown.Both); 
                client.Close();
            }
            FakeTcpConnection();
        }

        public void StartClientListeningThread()
        {
            var clientListeningThread = new Thread(() => ListenForClients());
            clientListeningThread.Start();
        }

        private void ListenForClients()
        {
            while(_serverRunning)
            {
                var foundClient = _serverSocket.Accept();

                if(_serverRunning)
                {
                    StartClientThread(foundClient);

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

        private void StartClientThread(Socket acceptedClientSocket)
        {
            var clientThread = new Thread(() => HandleClient(acceptedClientSocket));
            clientThread.Start();
        }
        
        private void HandleClient(Socket acceptedClientSocket)
        {
            int threadId = _currentThreadId;
            _currentThreadId++;
            _clientConnections.Add(new ClientCommandExecutionStatus(threadId));
            string username = "";
            try
            {
                IStreamHandler streamHandler = new SocketStreamHandler(acceptedClientSocket);
                VaporProtocol vp = new VaporProtocol(streamHandler);
                IServerCommandHandler serverCommandHandler = new ServerCommandHandler();
                bool connected = true;

                while (connected && _serverRunning)
                {
                    VaporProcessedPacket processedPacket = vp.ReceiveCommand();
                    SetStatusOfExecuting(true, threadId);

                    ProcessCommand(vp, processedPacket, serverCommandHandler, ref connected, ref username);

                    SetStatusOfExecuting(false, threadId);
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
                SetStatusOfExecuting(false, threadId);
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


        private void ProcessCommand(VaporProtocol vp,VaporProcessedPacket processedPacket , IServerCommandHandler serverCommandHandler, ref bool connected, ref string username)
        {
            CommandResponse response = serverCommandHandler.ExecuteCommand(processedPacket);
            vp.SendCommand(ReqResHeader.RES, response.Command, response.Response);

            if(response.Command == CommandConstants.COMMAND_PUBLISH_GAME_CODE || response.Command == CommandConstants.COMMAND_MODIFY_GAME_CODE)
            {
                RecieveClientGameCover(vp);
            }
                    
            if (response.Command == CommandConstants.COMMAND_DOWNLOAD_COVER_CODE)
            {
                SendClientGameCover(vp, response);
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
        }
        private string RemoveStatusCode(string response)
        {
            int statusCodeFixedSize = VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE;
            string message = response.Substring(statusCodeFixedSize, response.Length-statusCodeFixedSize);
            return message;
        }
        
        private void RecieveClientGameCover(VaporProtocol vp)
        {
            string path = _configurationHandler.GetPathFromAppSettings();
            try
            {
                vp.ReceiveCover(path);
            }
            catch (CoverNotReceivedException cre)
            {
            }
            catch(FileWritingException fwe)
            {
            }
        }

        private void SendClientGameCover(VaporProtocol vp, CommandResponse response)
        {
            string encodedGame = ExtractEncodedGame(response.Response);
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
            Game gameDummy = gameNTO.Decode(encodedGame);

            IPathHandler pathHandler = new PathHandler();
            string path = pathHandler.AppendPath(_configurationHandler.GetPathFromAppSettings(),$"{gameDummy.Id}.png");

            try
            {
                vp.SendCover(gameDummy.Title + "-COVER" , path);
            }
            catch(FileReadingException fre)
            {
                vp.SendCoverFailed();
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