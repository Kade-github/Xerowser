using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xerowser
{
    public partial class About : Form
    {

        public About()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            new MainBrowser().Show(); Hide();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void About_Load(object sender, EventArgs e)
        {
            try
            {
                if ("1.0" != new WebClient().DownloadString("https://zerowser.tk/version.txt"))
                    MessageBox.Show("You should all ways check our website for updates!", "Update Notice");
            }
            catch
            {
                MessageBox.Show("Unable to connect to the Servers, to check the version!", "Update Notice FAILED!");
            }
            label4.Text = "Version 1.0";
        }
    }
}
