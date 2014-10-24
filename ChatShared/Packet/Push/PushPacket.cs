using System;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Push
{
    public class PushPacket : Packet
    {
        //Introduced Fields: CMD

        //ReSharper disable once InconsistentNaming
        public String CMD { get; private set; }

        #region Constructors
        //PushPacket shouldnt be initialized directly.
        //A protected constructor protects initializing 
        //(only derrived classes can call this ctor)
        protected PushPacket(string cmd)
        {
            Initialize(cmd);
        }

        public PushPacket(JObject json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "Pushpacket ctor: json is null!");

            JToken cmd;

            if (!(json.TryGetValue("CMD", StringComparison.CurrentCultureIgnoreCase, out cmd)))
                throw new ArgumentException("CMD is not found in json" + json);

            Initialize((string)cmd);
        }
        #endregion

        #region Initializers
        private void Initialize(String cmd)
        {
            CMD = cmd;
        }
        #endregion


        #region Override methods
        public override JObject ToJsonObject()
        {
            return new JObject(
                     new JProperty("CMD", CMD)
                     );
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
        #endregion
    }
}
