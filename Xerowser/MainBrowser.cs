using System;
using CefSharp;
using CefSharp.WinForms;
using System.Windows.Forms;
using System.Web;
using System.Drawing;

namespace Xerowser
{
    public partial class MainBrowser : Form
    {
        private ChromiumWebBrowser browser;

        string prevURL = "";
        public MainBrowser()
        {
            InitializeComponent();
            var settings = new CefSettings();
            settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\ZEROWSER"; 
            Cef.Initialize(settings);
        }

        delegate void SetTextCallback(string text,string mode);

        private void SetText(string text,string mode)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text, mode });
            }
            else
            {
                this.label1.Text = mode;
                this.textBox1.Text = text;
            }
        }

        delegate void SetTitleA(string text);

        private void SetTitle(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetTitleA d = new SetTitleA(SetTitle);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.Text = "Xerowser - " + text;
            }
        }
        private void MainBrowser_Load(object sender, EventArgs e)
        {

            browser = new ChromiumWebBrowser("https://zerowser.tk/browser.html")
            {
                Dock = DockStyle.Fill,
            };

            browser.AddressChanged += OnBrowserAddressChanged;
            browser.LoadError += ErrorHandle;
            browser.TitleChanged += Title;
            toolStripContainer1.ContentPanel.Controls.Add(browser);
            toolStripContainer1.Size = Screen.PrimaryScreen.WorkingArea.Size;
        }
        private void ErrorHandle(object sender, LoadErrorEventArgs args)
        {
            switch(args.ErrorCode)
            {
                case CefErrorCode.AddressInvalid:
                    MessageBox.Show("That website does not exist!");
                    browser.Load(prevURL);
                    break;
                case CefErrorCode.ConnectionTimedOut:
                    MessageBox.Show("Your connection timed out!");
                    browser.Load(prevURL);
                    break;
            }
        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
        {
            string mode = "";
            if (args.Address.Contains("https://"))
            { label1.ForeColor = Color.Lime;  mode = "Secure";  } else { label1.ForeColor = Color.Red; mode = "Insecure"; }

            SetText(args.Address,mode);
        }
        private void Title(object sender, TitleChangedEventArgs args)
        {
            SetTitle(args.Title);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            prevURL = browser.Address;
            if (!textBox1.Text.Contains("."))
            {
                string text = HttpUtility.UrlEncode(textBox1.Text);
                browser.Load("https://duckduckgo.com/?q=" + text + "&t=h_");
            }
            else
                browser.Load(textBox1.Text);
            
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                prevURL = browser.Address;
                if (!textBox1.Text.Contains("."))
                {
                    string text = HttpUtility.UrlEncode(textBox1.Text);
                    browser.Load("https://duckduckgo.com/?q=" + text + "&t=h_");
                }
                else
                    browser.Load(textBox1.Text);

                e.Handled = true;
                e.SuppressKeyPress = true;

            }
        }

        private void MainBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string add = browser.Address;
            browser.Load(prevURL);
            prevURL = add;
        }

        private void ToolStripContainer1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                browser.ShowDevTools();
            }
        }

        private void MainBrowser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                browser.ShowDevTools();
            }
        }
    }
}

