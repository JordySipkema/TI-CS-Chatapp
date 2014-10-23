using System;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Response
{
    public class LoginResponsePacket : ResponsePacket
    {
        public const string DefCmd = "RESP-LOGIN";

        public string Usertype { get; set; }
        public string AuthToken { get; set; }

        #region Constructors
        public LoginResponsePacket(Statuscode.Status status, String usertype, String authtoken)
            : base(status, DefCmd)
        {
            Initialize(usertype, authtoken);
        }

        public LoginResponsePacket(String status, String description, String usertype, String authtoken) 
            : base(status, description, DefCmd)
        {
            Initialize(usertype, authtoken);
        }

        public LoginResponsePacket(JObject json) : base(json)
        {
            if(json["CMD"].ToString() != DefCmd)
                throw new InvalidOperationException("Wrong command type.");

            JToken token;
            JToken userType;

            Usertype = json.TryGetValue("USERTYPE", out userType) ? userType.ToString() : null;
            AuthToken = json.TryGetValue("AUTHTOKEN", out token) ? token.ToString() : null;
        }
        #endregion

        #region Initializers
        private void Initialize(String usertype, String authtoken)
        {
            Usertype = usertype;
            AuthToken = authtoken;
        }
        #endregion

        #region Override Methods
        public override JObject ToJsonObject()
        {
            var returnJson = base.ToJsonObject();
            returnJson.Add("USERTYPE", Usertype);
            returnJson.Add("AUTHTOKEN", AuthToken);

            return returnJson;

        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }

        #endregion

    }
}
