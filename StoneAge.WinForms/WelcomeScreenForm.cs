using System;
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
            Close();
        }

        private void WelcomeScreenForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            Close();
        }
    }
}
