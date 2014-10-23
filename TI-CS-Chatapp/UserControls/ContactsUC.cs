using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TI_CS_Chatapp
{
    public partial class ContactsUserControl : UserControl
    {
        public ContactsUserControl()
        {
            InitializeComponent();

        }

        public void LoadContacts(List<string> contactNames)
        {
            contactNames.Sort();
            foreach (string s in contactNames)
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
