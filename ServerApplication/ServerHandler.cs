using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading;
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
        private readonly TcpListener _tcpServerListener;
        private List<TcpClient> _tcpClients = new List<TcpClient>();
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
            _tcpServerListener = new TcpListener(_serverIpEndPoint);
        }

        public bool StartServer()
        {
            _tcpServerListener.Start(100);

            _serverRunning = true;

            return true; 
        }

        public void CloseServer()
        {
            _serverRunning = false;
            // Wait for all clients to finish their command executions.
            if (!AllClientsFinishedExecuting())
            { 
                Console.WriteLine("Waiting for clients to finish their commands..."); 
                // Give Clients 5 seconds to finish executions
                System.Threading.Thread.Sleep(MAX_SECONDS_WASTED);
            }
            foreach (TcpClient client in _tcpClients)
            {
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
                var foundClient = _tcpServerListener.AcceptTcpClient();
                StartClientThread(foundClient);

                _tcpClients.Add(foundClient);
            }
            
            _tcpServerListener.Stop();
        }

        private void StartClientThread(TcpClient acceptedTcpClient)
        {
            var clientThread = new Thread(() => HandleClient(acceptedTcpClient));
            clientThread.Start();
        }
        
        private void HandleClient(TcpClient acceptedTcpClient)
        {
            int threadId = _currentThreadId;
            _currentThreadId++;
            _clientConnections.Add(new ClientCommandExecutionStatus(threadId));

            try
            {
                INetworkStreamHandler streamHandler = new NetworkStreamHandler(acceptedTcpClient.GetStream());
                VaporProtocol vp = new VaporProtocol(streamHandler);
                IServerCommandHandler serverCommandHandler = new ServerCommandHandler();

                bool connected = true;

                while (connected && _serverRunning)
                {
                    VaporProcessedPacket processedPacket = vp.ReceiveCommand();

                    SetStatusOfExecuting(true, threadId);

                    CommandResponse response = serverCommandHandler.ExecuteCommand(processedPacket);
                    vp.SendCommand(ReqResHeader.RES, response.Command, response.Response);

                    if (response.Command == CommandConstants.COMMAND_PUBLISH_GAME_CODE)
                    {
                        //TODO: Si modificamos el nombre del juego, tiene que cambiar el nombre de la imagen.
                        // Para eso, mejor guardamos la imagen con nombre ID que nunca cambia...
                        string path = GetPathFromAppSettings();
                        vp.ReceiveCover(path);
                    }

                    if (response.Command == CommandConstants.COMMAND_DOWNLOAD_COVER_CODE)
                    {
                        string encodedGame = ExtractEncodedGame(response.Response);
                        GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
                        Game gameDummy = gameNTO.Decode(encodedGame);
                        IPathHandler pathHandler = new PathHandler();
                        string path = pathHandler.AppendPath(GetPathFromAppSettings(), $"{gameDummy.Id}.png");
                        vp.SendCover(gameDummy.Title, path);
                    }

                    if (response.Command == CommandConstants.COMMAND_EXIT_CODE)
                    {
                        connected = false;
                    }

                    SetStatusOfExecuting(false, threadId);
                }
            }
            catch (EndpointClosedSocketException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (EndpointClosedByServerSocketException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(SocketException e)
            {
                Console.WriteLine($"Something went wrong: {e.Message}");
            }
            finally
            {
                SetStatusOfExecuting(false, threadId);
            }
        }

        private string GetPathFromAppSettings()
        {
            string path = "";
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = _configurationHandler.GetField(ConfigurationConstants.WIN_SERVER_IMAGEPATH_KEY);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                path = _configurationHandler.GetField(ConfigurationConstants.OSX_SERVER_IMAGEPATH_KEY);
            }

            return path;
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
            TcpClient fakeTCPClient = new TcpClient(clientIpEndPoint);
            fakeTCPClient.Connect(serverIpEndPoint);
        }
    }
}