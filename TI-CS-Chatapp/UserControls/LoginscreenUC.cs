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
    public partial class LoginscreenUC : UserControl
    {
        public LoginscreenUC()
        {
            
            InitializeComponent();
        }

        private void lblSignin_Click(object sender, EventArgs e)
        {
            MainForm MyForm = (MainForm)this.Parent;
            tbPassword.Text = tbPassword.Text.Trim();
            MyForm.Login(tbUsername.Text, tbPassword.Text, chkPassword.CheckState.HasFlag(CheckState.Checked));
            tbPassword.Clear();
        }

        private void tbPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                lblSignin_Click(sender, new EventArgs());

            }
        }


    }
}
