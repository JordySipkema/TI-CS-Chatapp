﻿using System;
using Newtonsoft.Json.Linq;

namespace ChatShared.Packet.Response
{
    public class ResponsePacket : Packet
    {
        public string Status { get; private set; }
        public string Description { get; private set; }
        // ReSharper disable once InconsistentNaming
        public string CMD { get; private set; }

        #region Constructors
        public ResponsePacket(String cmd)
        {
            Initialize(cmd);
        }
        
        public ResponsePacket(Statuscode.Status status, string cmd = null)
        {
            Initialize(
                // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                Statuscode.GetCode(status).ToString(),
                Statuscode.GetDescription(status),
                cmd
                );
        }

        public ResponsePacket(JObject json)
        {
            if (json == null)
                throw new ArgumentNullException("json", "Requestpacket ctor: json is null!");

            JToken status;
            JToken description;
            JToken cmd;

            if (!(json.TryGetValue("STATUS", StringComparison.CurrentCultureIgnoreCase, out status) 
                && json.TryGetValue("DESCRIPTION", StringComparison.CurrentCultureIgnoreCase, out description)))
                throw new ArgumentException("STATUS or DESCRIPTION is not found in json" + json);

            var cmdS = json.TryGetValue("CMD", out cmd) ? cmd.ToString() : null;
            var statusS = status.ToString();
            var descS = description.ToString();

            Initialize(statusS, descS, cmdS);
        }

        public ResponsePacket(string status, string description, string cmd)
        {
            Initialize(status, description, cmd);
        }
        #endregion

        #region Initializers
        private void Initialize(string cmd)
        {
            CMD = cmd;
        }

        private void Initialize(string status, string description, string cmd = null)
        {
            Status = status;
            Description = description;
            CMD = cmd;
        }
        #endregion

        #region Override methods
        public override JObject ToJsonObject()
        {
            if (CMD == null)
            {
                return new JObject(
                        new JProperty("STATUS", Status),
                        new JProperty("DESCRIPTION", Description)
                        );
            }
            //else
            return new JObject(
                    new JProperty("CMD", CMD),
                    new JProperty("STATUS", Status),
                    new JProperty("DESCRIPTION", Description)
                    );
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }
        #endregion
    }
}
