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

        public delegate void SelectedContactDelegate(string SelectedContact);
        public static event SelectedContactDelegate SelectedContactEvent;
        

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
            RemoveContactsFromListBox();
            LoadContacts(AppGlobal.Users);
        }


        private string boolToOnline(bool online)
        {
            return online ? _online : _offline;
        }

        public void LoadContacts(List<User> nicknames)
        {
            foreach (User u in nicknames)
            {
                lboxContacts.Items.Add(u);
            }
        }

        public void RemoveContactsFromListBox()
        {
            lboxContacts.Items.Clear();
        }

        private void lboxContacts_SelectedValueChanged(object sender, EventArgs e)
        {
            // depricated
            // AppGlobal.SelectedContact = lboxContacts.SelectedItem.ToString();
            if (lboxContacts.SelectedItem == null) 
                return;
            MainForm MyForm = (MainForm)this.Parent;
            MyForm.ClearChatHistory();
            OnSelectedContactEvent(lboxContacts.SelectedItem.ToString());
            
        }

        private void OnSelectedContactEvent(string contact)
        {
            SelectedContactDelegate handler = SelectedContactEvent;
            if (handler != null) handler(contact);
        }
    }
}
