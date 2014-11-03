using ChatShared.Entity;
using System.Collections.Generic;
using System.Linq;
using ChatShared.Properties;
using TI_CS_Chatapp.Controller;
using ChatShared.Packet;
using ChatShared.Packet.Request;
using ChatShared.Packet.Response;
using ChatShared;
using ChatShared.Utilities;
using System.Threading;
using System;
using TI_CS_Chatapp.UserControls;
using ChatShared.Packet.Push;

namespace TI_CS_Chatapp
{
    public class AppGlobal
    {
        public static List<User> Users { get; private set; }
        public static List<ChatMessage> ChatMessages { get; private set; }

        public static User SelectedUser { get; private set; }

        public string AuthToken;

        private readonly TCPController Controller;

        public delegate void ResultDelegate(string status);
        public event ResultDelegate LoginResultEvent;
        public static event ResultDelegate RegisterResultEvent;
        public event ResultDelegate ResultEvent;
        public delegate void ContactDelegate(User user);
        public static event ContactDelegate OnlineStatusOfContactEvent;
        public delegate void MessageDelegate(ChatMessage message, bool selectedContactChanged, User selectedContact);
        public static event MessageDelegate IncomingMessageEvent;
        


        public AppGlobal()
        {
            Users = new List<User>();
            Controller = TCPController.Instance;
            //debug
            Users = new List<User>();

            ChatMessages = new List<ChatMessage>();
            Controller.OnPacketReceived += PacketReceived;
            AppGlobal.IncomingMessageEvent += HandleIncomingChatMessageEvent;
            ChatSessionUC.OutgoingMessageEvent += HandleOutgoingChatMessageEvent;
        }
        
        public void LoginToServer(string username, string password)
        {
            Controller.RunClient();

            Properties.Settings.Default.Username = username;
            var packet = new LoginPacket(username, Properties.Settings.Default.Password);
            if (!Properties.Settings.Default.RememberPassword)
            {
                var passhash = Crypto.CreateSHA256(password);
                Properties.Settings.Default.Password = passhash;
                packet = new LoginPacket(username, passhash);
            }

            Controller.SendAsync(packet);
            Controller.ReceiveTransmissionAsync();
        }

        public async void LogoutFromServer()
        {
            if (AuthToken == null || AuthToken.Length <= 20) return;

            var disconnectPacket = new DisconnectPacket(AuthToken);
            try
            {
                await Controller.SendAsync(disconnectPacket);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("An exception occured in the AppGlobal.LogoutFromServer function: " +
                    e.Message);
            }
            
            Users.Clear();
        }
        
        void PacketReceived(Packet p)
        {
            if (p is MessagePushPacket) 
            {
                var packet = p as MessagePushPacket;
                Console.WriteLine("push packet received!");
                IncomingMessageEvent(packet.Message, false, SelectedUser);
            }
            else if (p is UserChangedPacket)
            {
                var packet = p as UserChangedPacket;
                if (packet.Username == Properties.Settings.Default.Username)
                    return;
                User x = Users.FirstOrDefault(u => u.Username == packet.Username);
                if (x == null)
                {
                    x = new User(packet.Nickname, packet.Username, null);
                    Users.Add(x);
                }
                x.OnlineStatus = packet.Status;
                OnlineStatusOfContactEventChanged(x);
                
            }
            else if (p is LoginResponsePacket)
            {
                var packet = p as LoginResponsePacket;
                AuthToken = packet.AuthToken;
                Console.WriteLine("We are logged in!");
                Console.WriteLine("authtoken: " + AuthToken);
                OnLoginResultEvent(packet.Status);
            }
            else if (p is RegisterResponsePacket)
            {
                var packet = p as RegisterResponsePacket;
                OnRegisterResultEvent(packet.Status);
            }
            else if (p is PullResponsePacket<ChatMessage>)
            {
                var packet = p as PullResponsePacket<ChatMessage>;
                FillChatMessageList(packet.Data.ToList());
                Console.WriteLine("PullResponsePacket<ChatMessage> received!");
            }
            else if (p is PullResponsePacket<User>)
            {
                var packet = p as PullResponsePacket<User>;
                foreach (User u in packet.Data.ToList()) {
                    Users.Add(u);
                }
                InitializeContacts();
            }
            else if (p is ResponsePacket) //this one should be last!
            {
                var packet = p as ResponsePacket;
                OnResultEvent(packet);
            }
        }

        private void SelectedContactChanged(string SelectedContact)
        {
            var username = Users.Where(user => SelectedContact.Contains(user.Nickname)).Select(user => user.Username).FirstOrDefault();
            if (username == null)
                return;
            AppGlobal.SelectedUser = Users.Where(user => username == user.Username).ToList().FirstOrDefault();
            foreach (ChatMessage message in GetMessages(username))
            {
                IncomingMessageEvent(message, true, SelectedUser);
            }
        }

        public void SetRememberPassword(bool remember)
        {
            Properties.Settings.Default.RememberPassword = remember;
        }

        public void InitializeContacts()
        {
            ContactsUserControl.SelectedContactEvent += SelectedContactChanged;
            foreach (User user in Users)
            {
                OnlineStatusOfContactEventChanged(user);
            }
            
        }

        private void OnlineStatusOfContactEventChanged(User user)
        {
            ContactDelegate handler = OnlineStatusOfContactEvent;
            if (handler != null) handler(user);
        }

        private void OnLoginResultEvent(string status)
        {
            ResultDelegate handler = LoginResultEvent;
            if (handler != null) handler(status);
        }

        private void OnRegisterResultEvent(string status)
        {
            ResultDelegate handler = RegisterResultEvent;
            if (handler != null) handler(status);
        }

        private void OnResultEvent(Packet packet)
        {
            ResultDelegate handler = ResultEvent;
            if (handler != null) handler(packet);
        }


        private void HandleIncomingChatMessageEvent(ChatMessage message, bool selectedContactChanged, User selectedUser)
        {
            if (!selectedContactChanged)
                ChatMessages.Add(message);
        }

        private void HandleOutgoingChatMessageEvent(ChatMessage message)
        {
            Controller.RunClient();
            var packet = new ChatPacket(message, AuthToken);
            Controller.SendAsync(packet);
            Controller.ReceiveTransmissionAsync();
            IncomingMessageEvent(message, false, SelectedUser);
        }

        private void OnIncomingMessageEvent(ChatMessage message)
        {
            MessageDelegate handler = IncomingMessageEvent;
            if (handler != null) handler(message, true, SelectedUser);
        }


        public IEnumerable<ChatMessage> GetMessages(string username)
        {
            var x =
                from message in ChatMessages
                where message.Recipient == username || message.Sender == username
                orderby message.Timestamp ascending
                select message;

            return x;
        }
        public IEnumerable<ChatMessage> GetMessagesSentTo(string username)
        {
            var x =
                from message in ChatMessages
                where message.Recipient == username
                orderby message.Timestamp ascending
                select message;

            return x;
        }
        public IEnumerable<ChatMessage> GetMessagesFrom(string username)
        {
            var x =
                from message in ChatMessages
                where message.Sender == username
                orderby message.Timestamp ascending
                select message;
            return x;
        }

        public void GetAllMessagesAndContactsFromServer()
        {
            Controller.RunClient();

            var UBSpacket = new PullRequestPacket(PullRequestPacket.RequestType.UsersByStatus, AuthToken);
            Controller.SendAsync(UBSpacket);
            // Controller.ReceiveTransmissionAsync(); //not needed?
            // the messagesByUser packet
            var MBUpacket = new PullRequestPacket(PullRequestPacket.RequestType.MessagesByUser, Properties.Settings.Default.Username, AuthToken);
            Controller.SendAsync(MBUpacket);
        }

        private void FillChatMessageList(List<ChatMessage> list)
        {
            ChatMessages = new List<ChatMessage>(list);
        }

        

        public void Exiting()
        {
            Properties.Settings.Default.Save();

            // hij zou zn chat in cache moeten opslaan, WIP
        }

    }
}
