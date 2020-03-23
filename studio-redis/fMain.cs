using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace studio_redis
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void Form_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnCleanLog_Click(object sender, EventArgs e)
        {
            labelMessage.Text = string.Empty;
            try
            {
                //var r = new Redis(txtIP.Text.Trim(), Convert.ToInt16(txtPort.Text.Trim()));
                //r.FlushDb();
                //r.Dispose();
                //labelMessage.Text = "OK";
            }
            catch (Exception err) {
                labelMessage.Text = "FAIL";
            }
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            labelMessage.MouseDown += Form_MouseDown;
            this.Shown += (s1, e1) => {
                this.Width = 98;
                this.Top = 45;
                this.Left = 45;
            };
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fMain_MouseClick(object sender, MouseEventArgs e)
        {
            this.TopMost = true;
            labelMessage.Text = string.Empty;
        }
    }
}
