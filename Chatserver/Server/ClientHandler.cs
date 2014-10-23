using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using ChatShared.Packet;
using ChatShared.Packet.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chatserver.Server
{
    internal class ClientHandler : IClientHandler
    {
        private readonly byte[] _buffer = new byte[1024];
        private const int BufferSize = 1024;
        private readonly TcpClient _tcpclient;
        private readonly NetworkStream _networkStream;
        private List<byte> _totalBuffer;

        public ClientHandler(TcpClient client)
        {
            _tcpclient = client;

            _networkStream = _tcpclient.GetStream();
            _totalBuffer = new List<byte>();
            var thread = new Thread(ThreadLoop);
            thread.Start();

        }

        private void ThreadLoop()
        {
            while (true)
            {
                try
                {
                    //Is the client connected?
                    if (!_tcpclient.Connected)
                        throw new SocketException(0xDC);

                    //Recieve the data from the networkStream
                    var receiveCount = _networkStream.Read(_buffer, 0, BufferSize);

                    var rawData = new byte[receiveCount];
                    Array.Copy(_buffer, 0, rawData, 0, receiveCount);
                    _totalBuffer = _totalBuffer.Concat(rawData).ToList();

                    //Check the packetsize, did we recieve anything?
                    var packetSize = Packet.GetLengthOfPacket(_totalBuffer);
                    if (packetSize == -1)
                        continue;

                    //Retrieve the Json out of the recieved packet.
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
                        //continue;
                    }

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

        public void Send(String s)
        {
            var dataArray = Packet.CreateByteData(s);
            _networkStream.Write(dataArray, 0, dataArray.Length);
            _networkStream.Flush();
        }

        public void Send(Packet s)
        {
            Send(s.ToString());
        }
    }
}


