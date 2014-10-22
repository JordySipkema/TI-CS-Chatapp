using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatShared;
using System.Net;
using System.Net.Sockets;

namespace TI_CS_Chatapp
{
    public partial class SettingsForm : Form
    {
        private bool changedFlag = false; // form value(s) changed, check it on form close
        

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
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.changedFlag = false;
            this.Close();
        }
        
        private void tbServerIP_TextChanged(object sender, EventArgs e)
        {
            this.changedFlag = true;
        }

        private void tbServerPortNumber_TextChanged(object sender, EventArgs e)
        {
            // this.changedFlag = true; // this is not needed; control is not editable.
        }

        private void tbNickname_TextChanged(object sender, EventArgs e)
        {
            this.changedFlag = true;
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            checkChangedFlag();
        }


        // ** end of events ** //

        //get all settings stuff
        private void LoadSettings()
        {
            tbServerPortNumber.Text = Convert.ToString(AppProperties.PortNumber);
            
            //WIP

            //Code for getting server IP
            var clientIP = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.First(address => address.AddressFamily == AddressFamily.InterNetwork)
                .ToString();
            lblClientIP.Text = clientIP;
        }

        private void SaveAll()
        {
            //WIP

            //on the end
            changedFlag = false;
        }

        // some code for making sure the settings is going to be saved
        private void checkChangedFlag()
        {
            if (this.changedFlag) 
            {
                if (MessageBox.Show("Save your changes before exit?", "Save changes?", MessageBoxButtons.YesNo) == DialogResult.Yes) 
                {
                    this.SaveAll(); 
                }
            }
            
        }

        

        

        

    }
}
