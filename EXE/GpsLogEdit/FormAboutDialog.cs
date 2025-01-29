using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GpsLogEdit
{
    public partial class FormAboutDialog : Form
    {
        private Form ownerForm;

        public FormAboutDialog(Form owner)
        {
            InitializeComponent();
            ownerForm = owner;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://www2.biglobe.ne.jp/~sota/");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://www.openstreetmap.org/");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://opendatacommons.org/");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://mapsui.com/");
        }

        private void OpenUrl(string url)
        {
            try
            {
                var info = new ProcessStartInfo(url)
                {
                    UseShellExecute = true      // ShellExecuteを使わないとうまくいかない
                };
                Process.Start(info);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ownerForm, ex.Message, "プロセス起動エラー", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }
    }
}
