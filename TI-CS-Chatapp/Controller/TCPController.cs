using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using ChatShared.Packet;

namespace TI_CS_Chatapp.Controller
{
    // State object for receiving data from remote device.
    public class StateObject
    {
        // Size of receive buffer.
        public const int BufferSize = 256;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();



    }

    // ReSharper disable once InconsistentNaming
    public class TCPController
    {
        private static TcpClient _client;
        public static Boolean Busy { get; private set; }
        private static string _response = String.Empty;

        public delegate void ReceivedPacket(Packet p);

        public static event ReceivedPacket OnPacketReceived;

        public static bool IsReading { get; private set; }
        private static List<byte> _totalBuffer = new List<byte>();

        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);

        public static void RunClient()
        {
            IsReading = false;
            _client = new TcpClient();
            _client.Connect(new IPAddress(new byte[]{127,0,0,1}), ChatShared.Properties.Settings.Default.PortNumber);

            _totalBuffer = new List<byte>();

            // Signal that connected
            Console.WriteLine("TCPController: Connection active");
        }

        public static void StopClient()
        {
            if (_client == null) return;
            _client.Close();
            _client = null;
            Console.WriteLine("Client closed...");
        }



        public async static void SendAsync(String data)
        {
            if (_client == null)
                return;
            Busy = true;

            var bytes = Packet.CreateByteData(data);
            await _client.GetStream().WriteAsync(bytes, 0, bytes.Length);

        }

        public async static void ReceiveTransmissionAsync()
        {
            while (true)
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
