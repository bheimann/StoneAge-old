using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StoneAge.WinForms
{
    public partial class WelcomeScreenForm : Form
    {
        public WelcomeScreenForm()
        {
            InitializeComponent();
        }

        private void WelcomeScreenForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WelcomeScreenForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Close();
        }
    }
}
