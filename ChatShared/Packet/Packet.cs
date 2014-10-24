using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatShared.Entity;
using ChatShared.Packet.Request;
using ChatShared.Packet.Response;
using Newtonsoft.Json.Linq;
using PullResponsePacket = ChatShared.Packet.Response.PullResponsePacket<ChatShared.Entity.User>;
using PullResponseEnum = ChatShared.Packet.Response.PullResponsePacket<ChatShared.Entity.User>.DataType;

namespace ChatShared.Packet
{
    public abstract class Packet
    {

        public static int GetLengthOfPacket(string buffer)
        {
            if (buffer.Length < 4) return -1;
            //Continue means: if _totalBuffer.Lenght < 4, DO NOT PROCEED
            return int.Parse(buffer.Substring(0, 4));
        }

        public static int GetLengthOfPacket(List<byte> buffer)
        {
            if (buffer.Count < 4) return -1;
            var t = BitConverter.ToInt32(buffer.ToArray(), 0);
            return t;
        }

        /// <summary>
        ///  Tries to retrieve exactly one packet as a JSON object from a byte list.
        /// </summary>
        public static JObject RetrieveJson(int packetSize, ref List<byte> buffer)
        {
            if (buffer.Count < packetSize + 4) return null;
            return JObject.Parse(Encoding.UTF8.GetString(GetPacketBytes(packetSize, ref buffer).ToArray()));
        }

        private static List<byte> GetPacketBytes(int packetSize, ref List<byte> buffer)
        {
            var jsonData = buffer.GetRange(4, packetSize);
            buffer.RemoveRange(0, packetSize + 4);
            return jsonData;
        }

        /// <summary>
        ///  Creates a byte array from the specified string. First four bytes contains the length the data. The remainder of the bytes is the data bytes created from the given string.
        /// </summary>
        public static byte[] CreateByteData(string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            var length = BitConverter.GetBytes(bytes.Length);
            var data = length.Concat(bytes).ToArray();
            return data;
        }

        public static Packet RetrievePacket(int packetSize, ref List<byte> buffer)
        {
            var json = RetrieveJson(packetSize, ref buffer);
            return json == null ? null : GetPacketFromJson(json);
        }

        public static Packet GetPacketFromJson(JObject json)
        {
            if (json == null) return null;


            Packet p = null;
            switch ((string)json.GetValue("CMD", StringComparison.CurrentCultureIgnoreCase))
            {
                case LoginPacket.DefCmd:
                    p = new LoginPacket(json);
                    break;
                case LoginResponsePacket.DefCmd:
                    p = new LoginResponsePacket(json);
                    break;
                case ChatPacket.DefCmd:
                    p = new ChatPacket(json);
                    break;
                case RegisterPacket.DefCmd:
                    p = new RegisterPacket(json);
                    break;
                case RegisterResponsePacket.DefCmd:
                    p = new RegisterResponsePacket(json);
                    break;
                case DisconnectPacket.DefCmd:
                    p = new DisconnectPacket(json);
                    break;
                case PullResponsePacket.DefCmd:
                    switch ((PullResponseEnum)Enum.Parse(typeof(PullResponseEnum), 
                        (string)json.GetValue("DataType", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        case PullResponseEnum.User:
                            p = new PullResponsePacket<User>(json);
                            break;
                        case PullResponseEnum.ChatMessage:
                            p = new PullResponsePacket<ChatMessage>(json);
                            break;
                    }
                    break;
                default:
                    try
                    {
                        p = new ResponsePacket(json);
                    }
                    catch (ArgumentException) { }
                    break;
            }
            return p;
        }

        public abstract JObject ToJsonObject();

        public static implicit operator JObject(Packet packet)
        {
            return packet.ToJsonObject();
        }

        public static implicit operator String(Packet packet)
        {
            return packet.ToString();
        }
    }
}
