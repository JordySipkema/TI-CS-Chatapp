using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatShared.Enitity
{
    public class User
    {
        public string Nickname { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public List<ChatMessage> Messages { get; set; }
        
        public User(string nickname, string username, string password)
        {
            this.Nickname = nickname;
            this.Username = username;
            this.Password = password;
        }

        public void changeNickname(string nickname)
        {
            this.Nickname = nickname;
        }

        public void ChangePassword()
        {


        }

    }
}
