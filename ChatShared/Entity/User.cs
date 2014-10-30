using System;
using Newtonsoft.Json;

namespace ChatShared.Entity
{
    public class User
    {
        public string Nickname { get; set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string AuthToken { get; set; }

        public bool OnlineStatus { get; set; }

        public User(string nickname, string username, string password)
        {
            Nickname = nickname;
            Username = username;
            Password = password;
        }

        public void ChangeNickname(string nickname)
        {
            Nickname = nickname;
        }

        public void ChangePassword()
        {
        }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Nickname, OnlineStatus ? "Online" : "Offline");
        }
    }
}
