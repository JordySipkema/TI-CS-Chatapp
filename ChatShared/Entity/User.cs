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

        [JsonIgnore]
        private bool _onlineStatus;
        [JsonIgnore]
        public bool OnlineStatus
        {
            get { return _onlineStatus; }
            set { 
                OnOnlineStatusChanged();
                _onlineStatus = value; 
            }
        }

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

        public event EventHandler OnlineStatusChanged;
        private void OnOnlineStatusChanged()
        {
            var handler = OnlineStatusChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
