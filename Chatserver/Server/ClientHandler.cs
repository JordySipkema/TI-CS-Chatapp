using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Chatserver.FileController;
using ChatShared.Packet;
using ChatShared.Packet.Request;
using ChatShared.Packet.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chatserver.Server
{
    internal class ClientHandler : IClientHandler
    {
        private readonly Thread _thread;
        private readonly byte[] _buffer = new byte[1024];
        private const int BufferSize = 1024;
        private readonly TcpClient _tcpclient;
        private readonly NetworkStream _networkStream;
        private List<byte> _totalBuffer;
        private readonly Datastorage _datastorage;

        public ClientHandler(TcpClient client)
        {
            _datastorage = Datastorage.Instance;
            _tcpclient = client;

            _networkStream = _tcpclient.GetStream();
            _totalBuffer = new List<byte>();
            _thread = new Thread(ThreadLoop);
            _thread.Start();

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

                    //Retrieve the Json out of the received packet.
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

                    if (!json.TryGetValue("CMD", out cmd))
                    {
                        Console.WriteLine("Got JSON that does not define a command.");
                        continue;
                    }

                    switch ((string)cmd)
                    {
                        case ChatPacket.DefCmd:
                            HandleChatPacket(json);
                            break;
                        case LoginPacket.DefCmd:
                            HandleLoginPacket(json);
                            break;
                        case DisconnectPacket.DefCmd:
                            HandleDisconnectPacket(json);
                            break;
                    }

                }
                catch (Exception e)
                {
                    if (e.InnerException is SocketException)
                    {
                        Console.WriteLine("Client with IP-address: {0} has been disconnected",
                            _tcpclient.Client.LocalEndPoint);
                        _thread.Abort();
                    }
                    Console.WriteLine(e.Message);
                    break;
                }
            }
            Console.WriteLine("ClientThread stopped");
        }

        private void HandleDisconnectPacket(JObject json)
        {
            Console.WriteLine("DisconnectPacket Received");

            var packet = new DisconnectPacket(json);

            var returnPacket = new ResponsePacket(Statuscode.Status.Unauthorized);
            if (Authentication.Authenticate(packet.AuthToken))
            {
                Authentication.ReleaseAuthToken(packet.AuthToken);
                returnPacket = new ResponsePacket(Statuscode.Status.Ok);
            }

            Send(returnPacket);
        }

        private void HandleLoginPacket(JObject json)
        {
            Console.WriteLine("Login packet received");
            //Recieve the username and password from json.
            var packet = new LoginPacket(json);

            JObject returnJson;
            //Code to check User/pass here
            if (Authentication.Authenticate(packet.Username, packet.Passhash, this))
            {
                returnJson = new LoginResponsePacket(
                    Statuscode.Status.Ok,
                    Authentication.GetUser(packet.Username).AuthToken
                    );

            }
            else //If the code reaches this point, the authentification has failed.
            {
                returnJson = new ResponsePacket(Statuscode.Status.InvalidUsernameOrPassword, "RESP-LOGIN");
            }

            //Send the result back to the client.
            Console.WriteLine(returnJson.ToString());
            Send(returnJson.ToString());
        }

        private void HandleChatPacket(JObject json)
        {
            Console.WriteLine("Handle Chat Packet");
            var packet = new ChatPacket(json);

            
            throw new NotImplementedException();
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


