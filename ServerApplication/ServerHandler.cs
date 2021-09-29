using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Common.Commands;
using Common.Configuration;
using Common.Configuration.Interfaces;
using Common.NetworkUtilities;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;
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

        private readonly IConfigurationHandler _configurationHandler;
        private TcpClient _currentFoundClient;
        private readonly IPEndPoint _serverIpEndPoint;
        private readonly TcpListener _tcpServerListener;
        //private tcpClient[] _tcpCLients;

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

            return true; 
        }

        public void ListenForClients()
        {
            _currentFoundClient = _tcpServerListener.AcceptTcpClient();
        }

        public void StartClientThread()
        {
            var clientThread = new Thread(() => HandleClient(_currentFoundClient));
            clientThread.Start();
        }
        
        private void HandleClient(TcpClient acceptedTcpClient)
        {
            try
            {
                INetworkStreamHandler streamHandler = new NetworkStreamHandler(acceptedTcpClient.GetStream());
                VaporProtocol vp = new VaporProtocol(streamHandler);
                IServerCommandHandler serverCommandHandler = new ServerCommandHandler();

                bool connected = true;

                while(connected)
                {
                    VaporProcessedPacket processedPacket = vp.ReceiveCommand();
                    CommandResponse response = serverCommandHandler.ExecuteCommand(processedPacket);
                    vp.SendCommand(ReqResHeader.RES, response.Command, response.Response);

                    if(response.Command == CommandConstants.COMMAND_PUBLISH_GAME_CODE)
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
                        string path = GetPathFromAppSettings() + $"{gameDummy.Id}.png";
                        vp.SendCover(gameDummy.Title, path);
                    }

                    if(response.Command == CommandConstants.COMMAND_EXIT_CODE)
                    {
                        connected = false;
                    }
                }
            }
            catch(SocketException e)
            {
                Console.WriteLine($"Something went wrong: {e.Message}");
            }
            finally
            {
                Console.WriteLine("Goodbye client!");
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
    }
}