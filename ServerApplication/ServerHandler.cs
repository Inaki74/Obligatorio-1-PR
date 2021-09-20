using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common.Commands;
using Common.NetworkUtilities;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
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

            //TODO: Create config file with IP and Port 
            _serverIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);
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
                        //vp.ReceiveCover(path);
                        // Images/{nombreJuego}.png
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
    }
}