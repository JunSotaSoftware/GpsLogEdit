//
// 読み込んでいるファイル一覧を表示
//
// MIT License
// Copyright(c) 2024-2025 Sota. 

namespace GpsLogEdit
{
    public partial class FormShowReadFileList : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormShowReadFileList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ファイル一覧を表示
        /// </summary>
        /// <param name="fileList">ファイル一覧</param>
        /// <returns>ダイアログの戻り値</returns>
        public DialogResult ShowList(List<string> fileList)
        {
            string[] columnsData = new string[2];
            for (int i = 0; i < fileList.Count; i++)
            {
                columnsData[0] = (i + 1).ToString();
                columnsData[1] = fileList[i].ToString();
                ListViewItem item = new ListViewItem(columnsData);
                listReadFileList.Items.Add(item);
            }
            return this.ShowDialog();
        }

        /// <summary>
        /// ESCキーの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formShowReadFileList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
