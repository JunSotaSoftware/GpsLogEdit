using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GpsLogEdit
{
    public partial class FormShowReadFileList : Form
    {
        public FormShowReadFileList()
        {
            InitializeComponent();
        }

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

        private void formShowReadFileList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
