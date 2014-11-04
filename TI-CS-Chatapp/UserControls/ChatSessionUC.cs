using System;
using System.Windows.Forms;
using ChatShared.Entity;

namespace TI_CS_Chatapp.UserControls
{
    public partial class ChatSessionUC : UserControl
    {

        public delegate void MessageDelegate(ChatMessage message);
        public static event MessageDelegate OutgoingMessageEvent;

        private bool _outgoingmsgFlag = false;

        public ChatSessionUC()
        {
            InitializeComponent();
            AppGlobal.IncomingMessageEvent += HandleIncomingChatMessageEvent;
            
            
        }

        private void HandleIncomingChatMessageEvent(ChatShared.Entity.ChatMessage message, bool contactChanged, User selectedUser)
        {
            if (selectedUser == null) return;
            if (selectedUser.Nickname.Equals(message.Sender, StringComparison.CurrentCultureIgnoreCase) || _outgoingmsgFlag)
            {
                if (InvokeRequired)
                {
                    Invoke((new Action(() => HandleIncomingChatMessageEvent(message, contactChanged, selectedUser))));
                    return;
                }
                tbMsgHistory.AppendText((message.Timestamp.ToShortTimeString() + " " + message.Sender + ": " + message.Message + "\r\n"));
            }
            _outgoingmsgFlag = false;
        }

        private void OnOutgoingMessageEvent(ChatMessage message)
        {
            MessageDelegate handler = OutgoingMessageEvent;
            if (handler != null) handler(message);
        }
        
        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void SendMessage()
        {

            if (String.IsNullOrEmpty(Properties.Settings.Default.Username) || String.IsNullOrEmpty(AppGlobal.SelectedUser.Username))
                return;
            tbMessage.Text = tbMessage.Text.Trim();
            _outgoingmsgFlag = true;
            OnOutgoingMessageEvent(new ChatMessage(Properties.Settings.Default.Username, AppGlobal.SelectedUser.Username, tbMessage.Text, DateTime.Now));
            tbMessage.Clear();
        }

        public void ClearHistory()
        {
            tbMsgHistory.Text = String.Empty;
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return && AppGlobal.SelectedUser != null)
            {
                SendMessage();
            }
        }


    }
}
