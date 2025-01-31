//
// GpsLogEditについて画面を表示
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

using System.Diagnostics;

namespace GpsLogEdit
{
    public partial class FormAboutDialog : Form
    {
        private Form ownerForm;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="owner">オーナーフォーム</param>
        public FormAboutDialog(Form owner)
        {
            InitializeComponent();
            ownerForm = owner;
        }

        /// <summary>
        /// リンクがクリックされたときの処理（1）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://www2.biglobe.ne.jp/~sota/");
        }

        /// <summary>
        /// リンクがクリックされたときの処理（2）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://www.openstreetmap.org/");
        }

        /// <summary>
        /// リンクがクリックされたときの処理（3）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://opendatacommons.org/");
        }

        /// <summary>
        /// リンクがクリックされたときの処理（4）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://mapsui.com/");
        }

        /// <summary>
        /// ブラウザでURLをオープン
        /// </summary>
        /// <param name="url">URL</param>
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
