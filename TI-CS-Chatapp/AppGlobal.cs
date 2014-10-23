﻿using ChatShared.Entity;
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
        private readonly List<User> Users;
        private readonly TCPController Controller;

        public delegate void ResultDelegate(string status);
        public event ResultDelegate LoginResultEvent;
        public static event ResultDelegate RegisterResultEvent;

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

        void PacketReceived(Packet p)
        {
            if (p is LoginResponsePacket)
            {
                var packet = p as LoginResponsePacket;
                OnLoginResultEvent(packet.Status);
            }
            if (p is RegisterResponsePacket)
            {
                var packet = p as ResponsePacket;
                OnRegisterResultEvent(packet.Status);

            }
        }

        public void SetRememberPassword(bool remember)
        {
            Properties.Settings.Default.RememberPassword = remember;
        }

        public List<string> InitializeContacts()
        {

            /* ***WIP*** 
             * //ik haal hieronder de informatie over het netwerk op van de server OF vanuit de cache
             * Users.Clear(); //important !!!
             * Users = Connection.GetUsers();
            */

            return Users.Select(user => user.Nickname).ToList();

            /* Zo kan het ook:
            var x = from user in Users
                    select user.Nickname;
            return x.ToList();
             */
        }

        // hij zou zn chat in cache moeten opslaan, WIP
        public void Exiting()
        {
            Properties.Settings.Default.Save();
        }

    }
}
