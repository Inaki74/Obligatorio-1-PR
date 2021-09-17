using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common.Commands;
using Common.NetworkUtilities;
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
                NetworkStreamHandler streamHandler = new NetworkStreamHandler(acceptedTcpClient.GetStream());
                VaporProtocol vp = new VaporProtocol(streamHandler);
                VaporProcessedPacket processedPacket = vp.Receive();
                ServerCommandHandler serverCommandHandler = new ServerCommandHandler();
                string response = serverCommandHandler.ExecuteCommand(processedPacket);
                vp.Send(ReqResHeader.RES, CommandConstants.COMMAND_LOGIN_CODE, response.Length, response);
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