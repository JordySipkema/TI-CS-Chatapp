﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Chatserver.FileController;
using ChatShared.Entity;
using ChatShared.Packet;
using ChatShared.Packet.Push;
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
                        case RegisterPacket.DefCmd:
                            HandleRegisterPacket(json);
                            break;
                        case PullRequestPacket.DefCmd:
                            HandlePullRequestPacket(json);
                            break;
                    }

                }
                catch (SocketException)
                {
                    Console.WriteLine("Client with IP-address: {0} has been disconnected",
                        _tcpclient.Client.RemoteEndPoint);
                    _thread.Abort();
                }
                catch (Exception e)
                {
                    if (e.InnerException is SocketException)
                    {
                        Console.WriteLine("Client with IP-address: {0} has been disconnected",
                            _tcpclient.Client.RemoteEndPoint);
                        _thread.Abort();
                    }
                    else
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
// ReSharper disable once FunctionNeverReturns
        }

        private void HandlePullRequestPacket(JObject json)
        {
            Console.WriteLine("PullRequestPacket Received");
            var packet = new PullRequestPacket(json);

            var returnPacket = new ResponsePacket(Statuscode.Status.Unauthorized);
            if (Authentication.Authenticate(packet.AuthToken))
            {
                switch (packet.Request)
                {
                    case PullRequestPacket.RequestType.UsersByStatus:
                        returnPacket = HandlePullRequestUsersByStatus();
                        break;
                    case PullRequestPacket.RequestType.ReceivedMessages:
                        returnPacket = HandlePullRequestReceivedMessages(packet);
                        break;
                    case PullRequestPacket.RequestType.MessagesByUser:
                        returnPacket = HandlePullRequestMessagesByUser(packet);
                        break;
                    default:
                        returnPacket = new ResponsePacket(Statuscode.Status.CommandNotImplemented,
                            PullResponsePacket<object>.DefCmd);
                        break;
                }
            }
#if DEBUG
            Console.WriteLine(packet);
            Console.WriteLine(returnPacket);
#endif
            Send(returnPacket);
        }

        private ResponsePacket HandlePullRequestMessagesByUser(PullRequestPacket packet)
        {
            var allMessages = _datastorage.GetMessages(packet.SearchKey);
            return new PullResponsePacket<ChatMessage>(Statuscode.Status.Ok,
                PullResponsePacket<ChatMessage>.DataType.ChatMessage,
                allMessages);
        }

        private ResponsePacket HandlePullRequestReceivedMessages(PullRequestPacket packet)
        {
            var allMessages = _datastorage.GetMessagesSentTo(packet.SearchKey);
            return new PullResponsePacket<ChatMessage>(Statuscode.Status.Ok,
                PullResponsePacket<ChatMessage>.DataType.ChatMessage,
                allMessages);

        }

        private ResponsePacket HandlePullRequestUsersByStatus()
        {
            //Get all users and set their online-status to "false".
            var allUsers = _datastorage.GetUsers().ToList();
            var authenticatedUsers = Authentication.GetAllUsers();
            foreach (var user in allUsers)
            {
                user.OnlineStatus = authenticatedUsers.Contains(user);
            }

            return new PullResponsePacket<User>(Statuscode.Status.Ok,
                PullResponsePacket<User>.DataType.User,
                allUsers);
        }

        private void HandleRegisterPacket(JObject json)
        {
            Console.WriteLine("RegisterPacket Received");

            var packet = new RegisterPacket(json);
            var user = new User(packet.Nickname, packet.Username, packet.Passhash);
            Datastorage.Instance.AddUser(user);

            var returnPacket = new RegisterResponsePacket(Statuscode.Status.Ok);
            Send(returnPacket);

#if DEBUG
            Console.WriteLine(packet.ToString());
            Console.WriteLine(returnPacket.ToString());
#endif
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

#if DEBUG
            Console.WriteLine(packet.ToString());
            Console.WriteLine(returnPacket.ToString());
#endif
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
                var user = _datastorage.GetUser(packet.Username);
                if (user != null)
                {
                    user.OnlineStatus = true;
                    SendToAllOnlineUsers(new UserChangedPacket(user));
                }

            }
            else //If the code reaches this point, the authentification has failed.
            {
                returnJson = new ResponsePacket(Statuscode.Status.InvalidUsernameOrPassword, "RESP-LOGIN");
            }

            //Send the result back to the client.
            Send(returnJson.ToString());
#if DEBUG
            Console.WriteLine(packet.ToString());
            Console.WriteLine(returnJson.ToString());
#endif
        }

        private void HandleChatPacket(JObject json)
        {
            Console.WriteLine("Handle Chat Packet");
            var packet = new ChatPacket(json);

            var returnPacket = new ChatResponsePacket(Statuscode.Status.Unauthorized);

            if (Authentication.Authenticate(packet.AuthToken))
            {
                var usernameSender = Authentication.GetAllUsers()
                    .Where(user => user.AuthToken == packet.AuthToken)
                    .Select(user => user.Username).FirstOrDefault();

                var chatMessage = new ChatMessage(usernameSender,
                    packet.UsernameDestination,
                    packet.Message,
                    packet.Sent);

                _datastorage.AddMessage(chatMessage);

                //Generate ChatPushPacket
                Packet pushPacket = new MessagePushPacket(chatMessage);

                // Determining the socket to send the pushpacket
                var clientHandler = Authentication.GetStream(packet.UsernameDestination);
                if (clientHandler != null) clientHandler.Send(pushPacket);
#if DEBUG
                if (clientHandler != null) Console.WriteLine("Notifing:\n{0}", pushPacket);
#endif

                returnPacket = new ChatResponsePacket(Statuscode.Status.Ok);
            }
#if DEBUG
            Console.WriteLine(packet);
            Console.WriteLine(returnPacket);
#endif
            Send(returnPacket);
        }


        private void SendToAllOnlineUsers(Packet p)
        {
            foreach (var clientHandler in Authentication.GetAllUsers()
                .Select(linqUser => Authentication.GetStream(linqUser.Username))
                .Where(clientHandler => clientHandler != null))
            {
#if DEBUG
                Console.WriteLine("SendToAllUsers: Sending a packet");
#endif
                clientHandler.Send(p);
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


