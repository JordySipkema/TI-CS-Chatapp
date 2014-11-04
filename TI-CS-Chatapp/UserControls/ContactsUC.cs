using System;
using System.Collections.Generic;
using System.Linq;
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
            var MyForm = (MainForm)Parent;
            AppGlobal.OnlineStatusOfContactEvent += HandleOnlineStatusOfContact;
        }

        private void HandleOnlineStatusOfContact(User user)
        {
            if (InvokeRequired)
            {
                Invoke((new Action(() => HandleOnlineStatusOfContact(user))));
                return;
            }
            RemoveContactsFromListBox();
            LoadContacts(AppGlobal.Users.Where(x => x.Username != Properties.Settings.Default.Username).ToList());
        }


        private string boolToOnline(bool online)
        {
            return online ? _online : _offline;
        }

        public void LoadContacts(List<User> nicknames)
        {
            foreach (var u in nicknames)
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
            var MyForm = (MainForm)this.Parent;
            MyForm.ClearChatHistory();
            OnSelectedContactEvent(lboxContacts.SelectedItem.ToString());
            
        }

        private void OnSelectedContactEvent(string contact)
        {
            var handler = SelectedContactEvent;
            if (handler != null) handler(contact);
        }
    }
}
