//
// 統計情報ウインドウ
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

namespace GpsLogEdit
{
    public partial class FormStatisticsWindow : Form
    {
        public event Action? FormClosedEvent;
        private bool restoreWindowPos;

        /// <summary>
        /// 統計情報ウインドウのコンストラクタ
        /// </summary>
        /// <param name="restoreWindowPos"></param>
        public FormStatisticsWindow(bool restoreWindowPos)
        {
            this.restoreWindowPos = restoreWindowPos;
            InitializeComponent();
        }

        /// <summary>
        /// 統計情報ウインドウがロードされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form2_Load(object sender, EventArgs e)
        {
            if ((Properties.Settings.Default.F2Size.Width != 0) && (Properties.Settings.Default.F2Size.Height != 0))
            {
                // ウインドウの位置を復元
                if (restoreWindowPos)
                {
                    this.Location = Properties.Settings.Default.F2Location;
                }
                // ウインドウのサイズを復元
                this.Size = Properties.Settings.Default.F2Size;
            }
        }

        /// <summary>
        /// 統計情報ウインドウが閉じられた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formStatisticWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormClosedEvent?.Invoke();

            // 閉じるときはウインドのサイズと位置を保存
            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.F2Location = this.Location;
                Properties.Settings.Default.F2Size = this.Size;
            }
            else
            {
                Properties.Settings.Default.F2Location = this.RestoreBounds.Location;
                Properties.Settings.Default.F2Size = this.RestoreBounds.Size;
            }
            Properties.Settings.Default.Save();
        }

    }
}
