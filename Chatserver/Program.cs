using Chatserver.Server;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using ChatShared.Entity;
using ChatShared.Packet;
using ChatShared.Packet.Push;
using System.Threading;
using Chatserver.FileController;
using Chatserver.Properties;

// ReSharper disable ObjectCreationAsStatement

namespace Chatserver
{
    public class Program
    {
        private readonly Thread _thread;
        public static void Main(string[] args)
        {
            new Program();
            string userInput;
            userInput = Console.ReadLine();
            while (userInput != "exit")
            {
                switch(userInput)
                {
                    case "save":
                        Datastorage.Instance.SaveToFile();
                        break;
                    case "exit":
                        return;
                    default:

                        break;
                }
                userInput = Console.ReadLine();
            }
            Environment.Exit(0);
        }

        public Program()
        {
            _thread = new Thread(RunServer);
            _thread.Start();
            //RunServer();
        }

        public void RunServer()
        {
            Console.WriteLine("ChatServer Status: Initializing");
            var serverListener = new TcpListener(IPAddress.Any, ChatShared.Properties.Settings.Default.PortNumber);

            //Code for getting server IP
            var serverip = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.First(address => address.AddressFamily == AddressFamily.InterNetwork)
                .ToString();
            //Display server IP:
            Console.WriteLine("ChatServer IP: {0}", serverip);

            //Start the server listener
            serverListener.Start();
            Console.WriteLine("ChatServer Status: Listening");

            //Console.WriteLine("Example json: Errorpacket");
            //Console.WriteLine(new LoginResponsePacket(Statuscode.Status.Ok, "Client", "Ahsdha7w27%^hsdja^&"));

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
