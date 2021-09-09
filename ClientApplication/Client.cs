using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientApplication
{
    class Client
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client starting...");
           
            var clientIpEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.77"), 0); // Puerto 0 -> usa el primer puerto disponible
            var serverIpEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.77"), 6000);
            
            var tcpClient = new TcpClient(clientIpEndPoint);
            
            Console.WriteLine("Trying to connect to server");
            tcpClient.Connect(serverIpEndPoint);

            Console.WriteLine("Found server!");

            tcpClient.Close();

            //SendMessage(tcpClient);
        }

        static void SendMessage(TcpClient tcpClient)
        {
            using (var networkStream = tcpClient.GetStream())
            {
                var word = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(word);

                networkStream.Write(data, 0, data.Length);
            }

            tcpClient.Close();
        }
    }
}
