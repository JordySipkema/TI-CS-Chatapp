using ChatShared.Entity;
using System.Collections.Generic;
using System.Linq;
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
        //depricated
        //public static string SelectedContact { get; set; }

        private readonly TCPController Controller;

        public delegate void ResultDelegate(string status);
        public event ResultDelegate LoginResultEvent;
        public static event ResultDelegate RegisterResultEvent;
        public event ResultDelegate ResultEvent;
        public delegate void ContactDelegate(User user);
        public static event ContactDelegate OnlineStatusOfContactEvent;
        public delegate void MessageDelegate(ChatMessage message, bool selectedContactChanged);
        public static event MessageDelegate IncomingMessageEvent;
        


        public AppGlobal()
        {
            Controller = TCPController.Instance;
            //debug
            Users = new List<User>
            {
                //new User("Jordy", "jordy", "123"),
                //new User("Bart", "bart", "456"),
                //new User("Klaas", "klaas", "789")
            };

            ChatMessages = new List<ChatMessage>
            {
                new ChatMessage("jordy", "bart", "hallo bart", DateTime.Now),
                new ChatMessage("bart", "jordy", "hallo jordy", DateTime.Now),
                new ChatMessage("jordy", "bart", "hoe is het?", DateTime.Now),
                new ChatMessage("bart", "jordy", "goed!", DateTime.Now),
                new ChatMessage("jordy", "bart", "fijn!", DateTime.Now),
            };

            Controller.OnPacketReceived += PacketReceived;
            AppGlobal.IncomingMessageEvent += HandleIncomingChatMessageEvent;
            ChatSessionUC.OutgoingMessageEvent += HandleOutgoingChatMessageEvent;
        }
        
        //return true if succeed
        public void LoginToServer(string username, string password)
        {
            /* ***WIP***
             * hier komt de login code voor het verbinding maken met de server enzovoort.
            */
            Controller.RunClient();
            var passhash = Crypto.CreateSHA256(password);
            Properties.Settings.Default.Username = username;
            Properties.Settings.Default.Password = passhash;
            var packet = new LoginPacket(username, passhash);

            Controller.SendAsync(packet);
            Controller.ReceiveTransmissionAsync();
            
        }

        public void LogoutFromServer()
        {
            

        }
        
        void PacketReceived(Packet p)
        {
            if (p is MessagePushPacket) 
            {
                var packet = p as MessagePushPacket;
                Console.WriteLine("push packet received!");
                IncomingMessageEvent(packet.Message, false);
            }
            else if (p is UserChangedPacket)
            {
                var packet = p as UserChangedPacket;
                var user = new User(packet.Nickname, packet.Username, null);
                Users.Add(user);
                OnlineStatusOfContactEventChanged(user);
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
                Console.WriteLine("PullResponsePacket received!");
            }
            else if (p is ResponsePacket) //this one should be last!
            {
                var packet = p as ResponsePacket;
                OnResultEvent(packet);
            }
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

        private void SelectedContactChanged(string SelectedContact)
        {
            var username = Users.Where(user => SelectedContact.Contains(user.Nickname)).Select(user => user.Username).FirstOrDefault();
            if (username == null)
                return;
            AppGlobal.SelectedUser = Users.Where(user => username == user.Username).ToList().FirstOrDefault();
            foreach (ChatMessage message in GetMessages(username))
            {
                IncomingMessageEvent(message, true);
            }
        }

        //depricated
        public List<string> GetAllNicknames()
        {
            return Users.Select(user => user.Nickname).ToList();
            /* Zo kan het ook:
            var x = from user in Users
                    select user.Nickname;
            return x.ToList();
             */
        }

        private void OnlineStatusOfContactEventChanged(User user)
        {
            ContactDelegate handler = OnlineStatusOfContactEvent;
            if (handler != null) handler(user);
        }

        private void OnIncomingMessageEvent(ChatMessage message)
        {
            MessageDelegate handler = IncomingMessageEvent;
            if (handler != null) handler(message, true);
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

        public void GetAllMessagesFromServer()
        {
            Controller.RunClient();
            var packet = new PullRequestPacket(PullRequestPacket.RequestType.MessagesByUser, Properties.Settings.Default.Username, AuthToken);
            Controller.SendAsync(packet);
            Controller.ReceiveTransmissionAsync();
        }

        private void FillChatMessageList(List<ChatMessage> list)
        {
            ChatMessages = new List<ChatMessage>(list);
        }

        private void HandleIncomingChatMessageEvent(ChatMessage message, bool selectedContactChanged)
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
            IncomingMessageEvent(message, false);
        }

        public void Exiting()
        {
            Properties.Settings.Default.Save();

            // hij zou zn chat in cache moeten opslaan, WIP
        }

    }
}
