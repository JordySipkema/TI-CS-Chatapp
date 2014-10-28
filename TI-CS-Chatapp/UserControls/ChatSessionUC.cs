﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatShared.Entity;

namespace TI_CS_Chatapp.UserControls
{
    public partial class ChatSessionUC : UserControl
    {

        public delegate void MessageDelegate(ChatMessage message);
        public static event MessageDelegate OutgoingMessageEvent;

        public ChatSessionUC()
        {
            InitializeComponent();
            AppGlobal.IncomingMessageEvent += HandleIncomingChatMessageEvent;
            
            
        }

        private void HandleIncomingChatMessageEvent(ChatShared.Entity.ChatMessage message, bool contactChanged)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((new Action(() => HandleIncomingChatMessageEvent(message, contactChanged))));
                return;
            }
            tbMsgHistory.AppendText((message.Timestamp.ToShortTimeString() + " " + message.Sender + ": " + message.Message + "\r\n"));
            //tbMsgHistory.Text += 
            
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

            if ((Properties.Settings.Default.Username == null) || (AppGlobal.SelectedUser.Username == null))
                return;
            tbMessage.Text = tbMessage.Text.Trim();
            OnOutgoingMessageEvent(new ChatMessage(Properties.Settings.Default.Username, AppGlobal.SelectedUser.Username, tbMessage.Text, DateTime.Now));
            tbMessage.Clear();
        }

        public void ClearHistory()
        {
            tbMsgHistory.Text = "";
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SendMessage();
            }
        }


    }
}
