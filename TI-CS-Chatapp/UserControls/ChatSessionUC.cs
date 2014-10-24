using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TI_CS_Chatapp.UserControls
{
    public partial class ChatSessionUC : UserControl
    {
        public ChatSessionUC()
        {
            InitializeComponent();
            AppGlobal.IncomingMessageEvent += HandleIncomingChatMessageEvent;
        }

        private void HandleIncomingChatMessageEvent(ChatShared.Entity.ChatMessage message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((new Action(() => HandleIncomingChatMessageEvent(message))));
                return;
            }

            tbMsgHistory.Text += (message.Timestamp.ToShortTimeString() + " " + message.Sender + ": " + message.Message + "\r\n");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

        }

        public void ClearHistory()
        {
            tbMsgHistory.Text = "";
        }


    }
}
