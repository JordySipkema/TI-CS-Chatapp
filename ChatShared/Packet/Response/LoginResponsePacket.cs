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
        public LoginResponsePacket(Statuscode.Status status, String authtoken)
            : base(status, DefCmd)
        {
            Initialize(authtoken);
        }

        public LoginResponsePacket(String status, String description, String authtoken) 
            : base(status, description, DefCmd)
        {
            Initialize(authtoken);
        }

        public LoginResponsePacket(JObject json) : base(json)
        {
            if(json["CMD"].ToString() != DefCmd)
                throw new InvalidOperationException("Wrong command type.");

            JToken token;
            JToken userType;

            AuthToken = json.TryGetValue("AUTHTOKEN", out token) ? token.ToString() : null;
        }
        #endregion

        #region Initializers
        private void Initialize(String authtoken)
        {
            AuthToken = authtoken;
        }
        #endregion

        #region Override Methods
        public override JObject ToJsonObject()
        {
            var returnJson = base.ToJsonObject();
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
