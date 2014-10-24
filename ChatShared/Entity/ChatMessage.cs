using System;

namespace ChatShared.Entity
{
    public class ChatMessage
    {
        public String Sender { get; private set; }
        public String Recipient { get; private set; }
        public String Message { get; private set; }
        public DateTime Timestamp { get; private set; }

        public ChatMessage(string sender, string recipient, string message, DateTime timestamp)
        {
            Sender = sender;
            Recipient = recipient;
            Message = message;
            Timestamp = timestamp;
        }




    }
}
