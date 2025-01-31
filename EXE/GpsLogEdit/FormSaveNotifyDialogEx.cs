//
// 保存確認画面を表示
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

namespace GpsLogEdit
{
    public partial class FormSaveNotifyDialogEx : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormSaveNotifyDialogEx()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 書き込み確認を表示
        /// </summary>
        /// <param name="info">確認情報</param>
        /// <param name="message">labelに表示するメッセージ</param>
        /// <returns>確認情報</returns>
        public SaveNotifyInfo ShowDialog(SaveNotifyInfo info, string message)
        {
            labelSaveNotifyMsg.Text = message;
            textBoxDataName.Text = info.GetName();
            radioButtonSeparateFile.Checked = info.GetSeparate();
            radioButtonOneFile.Checked = !info.GetSeparate();
            if (info.GetFileType() == FileType.Gpx)
            {
                // GPSファイルへは色を保存できないので指定できないように
                listViewTrackInfo.Columns.RemoveAt(2);
                labelColorChangeMsg.Visible = false;
            }
            for (int i = 0; i < info.GetTrackCount(); i++)
            {
                string[] columnsData = new string[3];
                columnsData[0] = (i + 1).ToString();
                columnsData[1] = String.Format("{0} - {1}", info.GetOneTrack(i).GetStartTime(), info.GetOneTrack(i).GetLastTime());
                columnsData[2] = " ";
                ListViewItem item = new ListViewItem(columnsData);
                item.SubItems[2].BackColor = info.GetColor(i);
                item.UseItemStyleForSubItems = false;
                listViewTrackInfo.Items.Add(item);

            }
            DialogResult result = this.ShowDialog();
            info.SetResult(result);
            for (int i = 0; i < info.GetTrackCount(); i++)
            {
                ListViewItem item = listViewTrackInfo.Items[i];
                info.SetColor(i, item.SubItems[2].BackColor);
            }
            info.SetName(textBoxDataName.Text);
            info.SetSeparate(radioButtonSeparateFile.Checked);
            return info;
        }

        /// <summary>
        /// ESCキーの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formSaveNotifyDialogEx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        /// <summary>
        /// トラックの表示色がクリックされたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewTrackInfo_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listViewTrackInfo.HitTest(e.X, e.Y);
            if ((info != null) && (info.Item != null))
            {
                // 行はinfo.Itemで取得できる
                // 以下は、列の判断
                for (int column = 0; column < info.Item.SubItems.Count; column++)
                {
                    if (info.Item.SubItems[column] == info.SubItem)
                    {
                        // 行と列が分かった
                        if (column == 2)
                        {
                            if (colorDialog1.ShowDialog() == DialogResult.OK)
                            {
                                ListViewItem item = listViewTrackInfo.Items[info.Item.Index];
                                item.SubItems[2].BackColor = colorDialog1.Color;
                            }
                        }
                        break;
                    }
                }
            }
        }

    }
}
