// ReSharper disable ObjectCreationAsStatement
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using ChatShared;

namespace Chatserver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            RunServer();
        }

        public void RunServer()
        {
            Console.WriteLine("ChatServer Status: Initializing");
            var serverListener = new TcpListener(IPAddress.Any, AppProperties.PortNumber);

            //Code for getting server IP
            var serverip = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.First(address => address.AddressFamily == AddressFamily.InterNetwork)
                .ToString();
            //Display server IP:
            Console.WriteLine("ChatServer IP: {0}", serverip);
            Console.WriteLine("ChatServer Status: Listening");
            serverListener.Start();
            while (true)
            {
                var tcpclient = serverListener.AcceptTcpClient();
                Console.WriteLine("ChatServer: Accepted new client");
                new ClientHandler(tcpclient);
            }
        // ReSharper disable once FunctionNeverReturns
        }
    }
}
