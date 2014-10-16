using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chatserver
{
    internal class ClientHandler
    {
        private readonly byte[] _buffer = new byte[1024];
        private const int BufferSize = 1024;
        private readonly TcpClient _tcpclient;
        private readonly SslStream _sslStream;
        private List<byte> _totalBuffer;

        //private string username;
        //private Boolean isLoggedIn;

        public ClientHandler(TcpClient client)
        {
            _tcpclient = client;
            //var certificate = new X509Certificate2(Resources.invalid_certificate, "apeture");

            _sslStream = new SslStream(_tcpclient.GetStream());
            _totalBuffer = new List<byte>();
            var thread = new Thread(ThreadLoop);
            thread.Start();

        }

        private void ThreadLoop()
        {
            while (true)
            {
                if (!_tcpclient.Connected)
                    break;
                try
                {
                    //new Socket().Receive(Buffer);
                    var receiveCount = _sslStream.Read(_buffer, 0, BufferSize);

                    var rawData = new byte[receiveCount];
                    Array.Copy(_buffer, 0, rawData, 0, receiveCount);
                    _totalBuffer = _totalBuffer.Concat(rawData).ToList();


                    var packetSize = Packet.GetLengthOfPacket(_totalBuffer);
                    if (packetSize == -1)
                        continue;

                    JObject json = null;
                    try
                    {
                        json = Packet.RetrieveJson(packetSize, ref _totalBuffer);
                    }
                    catch (JsonReaderException)
                    {
                        Console.WriteLine("Sending SyntaxError-packet to {0}", _tcpclient.Client.RemoteEndPoint);
                        Send(new ResponsePacket(Statuscode.Status.SyntaxError));
                    }

                    if (json == null)
                        continue;

                    JToken cmd;
                    JToken authToken = null;
                    if (!json.TryGetValue("CMD", out cmd))
                    {
                        Console.WriteLine("Got JSON that does not define a command.");
                        continue;
                    }

                    //_totalBuffer = _totalBuffer.Substring(packetSize + 4);
                    //_totalBuffer = String.Empty;
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Client with IP-address: {0} has been disconnected",
                        _tcpclient.Client.LocalEndPoint);
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            }
        }

        private void Send(String s)
        {
            //byte[] data = Encoding.UTF8.GetBytes(s.Length.ToString("0000") + s).ToArray();

            _sslStream.Write(Packet.CreateByteData(s));
            _sslStream.Flush();
        }

        private void Send(Packet s)
        {
            Send(s.ToString());
        }
    }
}


