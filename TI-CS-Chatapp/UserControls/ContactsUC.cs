using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatShared.Entity;

namespace TI_CS_Chatapp
{
    public partial class ContactsUserControl : UserControl
    {
        private const string _online = " (online)";
        private const string _offline = " (offline)";

        public ContactsUserControl()
        {
            InitializeComponent();
            MainForm MyForm = (MainForm)this.Parent;
            AppGlobal.OnlineStatusOfContactEvent += HandleOnlineStatusOfContact;
        }

        private void HandleOnlineStatusOfContact(User user)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((new Action(() => HandleOnlineStatusOfContact(user))));
                return;
            }
            /*
            if (_userDict.ContainsKey(username))
            {
                _userDict[username] = onlinestatus;
            }
            else
            {
                _userDict.Add(username, onlinestatus);
            }
            */
            var nicknames =
                from nn in AppGlobal.Users
                orderby nn.Nickname
                select String.Format("{0} {1}", nn.Nickname, boolToOnline(nn.OnlineStatus));

            RemoveContactsFromListBox();
            LoadContacts(nicknames.ToList());
        }


        private string boolToOnline(bool online)
        {
            return online ? _online : _offline;
        }

        public void LoadContacts(List<string> nicknames)
        {
            nicknames.Sort();
            foreach (string s in nicknames)
            {
                lboxContacts.Items.Add(s);
            }

        }

        public void RemoveContactsFromListBox()
        {
            lboxContacts.Items.Clear();
        }
    }
}
