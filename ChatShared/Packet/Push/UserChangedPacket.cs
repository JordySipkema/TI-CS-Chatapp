using System;
using ChatShared.Entity;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Push
{
    public class UserChangedPacket : PushPacket
    {
        //Inherited Fields: CMD
        //Introduced Fields: Username, Nickname, Status

        public String Username { get; set; }
        public String Nickname { get; set; }
        public bool Status { get; set; }

        public const string DefCmd = "PUSH-UC";

        public UserChangedPacket(User user) : base(DefCmd)
        {
            Initialize(user.Username, user.Nickname, user.OnlineStatus);
        }

        public UserChangedPacket(String username, string nickname, bool status) : base(DefCmd)
        {
            Initialize(username, nickname, status);
        }


        public UserChangedPacket(JObject json) : base(json)
        {
        }

        private void Initialize(String username, string nickname, bool status)
        {
            Username = username;
            Nickname = nickname;
            Status = status;
        }

        public override JObject ToJsonObject()
        {
            return base.ToJsonObject();
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
    }
}
