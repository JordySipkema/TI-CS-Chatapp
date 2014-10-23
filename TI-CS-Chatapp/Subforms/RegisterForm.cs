using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatShared.Packet.Request;
using ChatShared.Packet.Response;
using ChatShared.Packet;
using ChatShared.Utilities;
using TI_CS_Chatapp.Controller;

namespace TI_CS_Chatapp.Subforms
{
    public partial class RegisterForm : Form
    {
        private string Status = "";
        private bool ChangedFlag = false; // form value(s) changed, check it on form close

        public RegisterForm()
        {
            InitializeComponent();
            AppGlobal.RegisterResultEvent += HandleStatus;
        }

        private void tbNickname_TextChanged(object sender, EventArgs e)
        {
            ChangedFlag = true;
        }

        private void tbUsername_TextChanged(object sender, EventArgs e)
        {
            ChangedFlag = true;
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            ChangedFlag = true;
        }

        private void CheckChangedFlag()
        {
            CheckChangedFlag(null);
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            Register();
            ChangedFlag = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangedFlag = false;
            this.Close();
        }

        private void RegisterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CheckChangedFlag(e);
        }

        private void CheckChangedFlag(FormClosingEventArgs args)
        {
            if (this.ChangedFlag)
            {
                var b = MessageBox.Show("Save your changes before exit?", "Save changes?", MessageBoxButtons.YesNoCancel);
                if (b == DialogResult.Yes)
                {
                    Register();
                    if (Status != "200")
                    {
                        args.Cancel = true;
                    }
                }
                else if (b == DialogResult.Cancel)
                {
                    args.Cancel = true;
                }
            }

        }

        private void Register()
        {
            var packet = new RegisterPacket(tbNickname.Text, tbUsername.Text, Crypto.CreateSHA256(tbPassword.Text));
            Controller.TCPController.Instance.RunClient();
            Controller.TCPController.Instance.SendAsync(packet);
            Controller.TCPController.Instance.ReceiveTransmissionAsync();
        }

        private void HandleStatus(string status)
        {
            Status = status;
            if (status == "200") // OK
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((new Action(() => HandleStatus(status))));
                    return;
                }
                MessageBox.Show("Thank you for your registration!", "Registration succeed", MessageBoxButtons.OK);
                ChangedFlag = false;
                this.Close();
                
            }
            else
            {
                MessageBox.Show("Unhandled error occured", "Error", MessageBoxButtons.OK);
            }
        }
    }
}
