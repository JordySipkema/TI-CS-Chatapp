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

namespace TI_CS_Chatapp
{
    public class AppGlobal
    {
        public static List<User> Users { get; private set; }
        private readonly TCPController Controller;

        public delegate void ResultDelegate(string status);
        public event ResultDelegate LoginResultEvent;
        public static event ResultDelegate RegisterResultEvent;
        public event ResultDelegate ResultEvent;
        public delegate void ContactDelegate(User user);
        public static event ContactDelegate OnlineStatusOfContactEvent;

        public AppGlobal()
        {
            Controller = TCPController.Instance;
            //debug
            Users = new List<User>
            {
                new User("Jordy", "jordy", "123"),
                new User("Bart", "bart", "456"),
                new User("Klaas", "klaas", "789")
            };
            Controller.OnPacketReceived += PacketReceived;
        }


        
        //return true if succeed
        public void LoginToServer(string username, string password)
        {
            /* ***WIP***
             * hier komt de login code voor het verbinding maken met de server enzovoort.
            */
            Controller.RunClient();
            var passhash = Crypto.CreateSHA256(password);
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
            if (p is LoginResponsePacket)
            {
                var packet = p as LoginResponsePacket;
                OnLoginResultEvent(packet.Status);
            }
            else if (p is RegisterResponsePacket)
            {
                var packet = p as RegisterResponsePacket;
                OnRegisterResultEvent(packet.Status);
            }
            else if (p is ResponsePacket)
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
            foreach (User user in Users)
            {
                OnlineStatusOfContactEventChanged(user);
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

        // hij zou zn chat in cache moeten opslaan, WIP
        public void Exiting()
        {
            Properties.Settings.Default.Save();
        }

    }
}
