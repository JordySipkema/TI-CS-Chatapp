using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using TI_CS_Chatapp.Properties;

namespace TI_CS_Chatapp
{
    public partial class SettingsForm : Form
    {
        private bool ChangedFlag = false; // form value(s) changed, check it on form close


        public SettingsForm()
        {
            InitializeComponent();
        }

        // ***** events ***** //

        private void Settings_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveAll();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangedFlag = false;
            Close();
        }

        private void tbServerIP_TextChanged(object sender, EventArgs e)
        {
            if (Settings.Default.ServerIP != tbServerIP.Text)
            {
                ChangedFlag = true;
            }
        }

        private void tbServerPortNumber_TextChanged(object sender, EventArgs e)
        {
            // this.changedFlag = true; // this is not needed; control is not editable.
        }

        private void tbNickname_TextChanged(object sender, EventArgs e)
        {
            if (Settings.Default.Nickname != tbNickname.Text)
            {
                ChangedFlag = true;
            }
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            CheckChangedFlag();
        }


        // ** end of events ** //

        //get all settings stuff
        private void LoadSettings()
        {
            tbServerPortNumber.Text = ChatShared.Properties.Settings.Default.PortNumber.ToString();
            tbNickname.Text = Settings.Default.Nickname;
            tbServerIP.Text = Settings.Default.ServerIP;
            //Code for getting server IP
            var clientIP = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.First(address => address.AddressFamily == AddressFamily.InterNetwork)
                .ToString();
            lblClientIP.Text = clientIP;
        }

        private void SaveAll()
        {
            Settings.Default.Nickname = tbNickname.Text;
            Settings.Default.ServerIP = tbServerIP.Text;
            //on the end
            ChangedFlag = false;
        }

        // some code for making sure the settings is going to be saved
        private void CheckChangedFlag()
        {
            if (!ChangedFlag) return;

            if (MessageBox.Show("Save your changes before exit?", "Save changes?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SaveAll();
            }
        }
    }
}
