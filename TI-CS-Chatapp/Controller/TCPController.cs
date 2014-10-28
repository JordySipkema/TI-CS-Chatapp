using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ChatShared.Packet;

namespace TI_CS_Chatapp.Controller
{
    // ReSharper disable once InconsistentNaming
    public class TCPController
    {
        private static TCPController _instance;
        public static TCPController Instance
        {
            get { return _instance ?? (_instance = new TCPController()); }
        }

        private TcpClient _client;
        public Boolean IsConnected { get; private set; }
        public delegate void ReceivedPacket(Packet p);
        public event ReceivedPacket OnPacketReceived;

        public bool IsReading { get; private set; }
        private List<byte> _totalBuffer = new List<byte>();

        private TCPController()
        {
            //ReceiveTransmissionAsync();
        }

        public void RunClient()
        {
            if (Properties.Settings.Default.ServerIP.Length < 7)
                throw new ArgumentNullException("ServerIP", "ServerIP is not set or invalid");

            RunClient(IPAddress.Parse(Properties.Settings.Default.ServerIP));
        }

        public void RunClient(IPAddress IP)
        {
            if (IsConnected) return;

            IsReading = false;
            _client = new TcpClient();
            _client.Connect(IP, ChatShared.Properties.Settings.Default.PortNumber);
            IsConnected = true;

            _totalBuffer = new List<byte>();

            // Signal that connected
            Console.WriteLine("TCPController: Connection active");
        }

        public void StopClient()
        {
            IsConnected = false;

            if (_client == null) return;
            _client.Close();
            _client = null;
            Console.WriteLine("Client closed...");
        }



        public async Task SendAsync(String data)
        {
            if (_client == null)
                return;

            var bytes = Packet.CreateByteData(data);
            await _client.GetStream().WriteAsync(bytes, 0, bytes.Length);
        }

        public async void ReceiveTransmissionAsync()
        {
            while (IsConnected)
            {
                var buffer = new byte[1024];
                var bytesRead = await _client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    try
                    {
                        var rawData = new byte[bytesRead];
                        Array.Copy(buffer, 0, rawData, 0, bytesRead);
                        _totalBuffer = _totalBuffer.Concat(rawData).ToList();

                        var packetSize = Packet.GetLengthOfPacket(_totalBuffer);
                        if (packetSize == -1)
                            continue;

                        var p = Packet.RetrievePacket(packetSize, ref _totalBuffer);
                        if (p == null)
                            continue;


                        foreach (var @delegate in OnPacketReceived.GetInvocationList())
                        {
                                var deleg = (ReceivedPacket) @delegate;
                                deleg.BeginInvoke(p, null, null);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An exception occured in the TCPController.ReceiveTransmissionAsync function: " +
                                          e.Message);
                    }
                }
                else
                {
                    Console.WriteLine("No bytes received. Connection has probably been closed.");
                    return;
                }
            }
        }
    }
}
