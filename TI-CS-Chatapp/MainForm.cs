using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TI_CS_Chatapp
{
    public partial class MainForm : Form
    {

        // hoogte zonder menustrip en menubalk (-/+/x) is 700 - (24 + 39) = 637
        // user control width/height voor login screen: 484 en 629
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //loginscreenUC1.Visible = true;
        }



    }
}
