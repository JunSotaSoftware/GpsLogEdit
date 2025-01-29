namespace GpsLogEdit
{
    partial class FormSaveNotifyDialogEx
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelSaveNotifyMsg = new Label();
            listViewTrackInfo = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            label1 = new Label();
            label2 = new Label();
            groupBox1 = new GroupBox();
            radioButtonOneFile = new RadioButton();
            radioButtonSeparateFile = new RadioButton();
            textBoxDataName = new TextBox();
            buttonSaveOk = new Button();
            buttonSaveCancel = new Button();
            colorDialog1 = new ColorDialog();
            labelColorChangeMsg = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            //
            // labelSaveNotifyMsg
            //
            labelSaveNotifyMsg.AutoSize = true;
            labelSaveNotifyMsg.Location = new Point(12, 9);
            labelSaveNotifyMsg.Name = "labelSaveNotifyMsg";
            labelSaveNotifyMsg.Size = new Size(41, 15);
            labelSaveNotifyMsg.TabIndex = 2;
            labelSaveNotifyMsg.Text = "label1";
            //
            // listViewTrackInfo
            //
            listViewTrackInfo.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            listViewTrackInfo.FullRowSelect = true;
            listViewTrackInfo.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listViewTrackInfo.Location = new Point(12, 27);
            listViewTrackInfo.MultiSelect = false;
            listViewTrackInfo.Name = "listViewTrackInfo";
            listViewTrackInfo.Size = new Size(427, 161);
            listViewTrackInfo.TabIndex = 3;
            listViewTrackInfo.UseCompatibleStateImageBehavior = false;
            listViewTrackInfo.View = View.Details;
            listViewTrackInfo.MouseClick += listViewTrackInfo_MouseClick;
            //
            // columnHeader1
            //
            columnHeader1.Text = "Track";
            columnHeader1.Width = 50;
            //
            // columnHeader2
            //
            columnHeader2.Text = "開始／終了時刻";
            columnHeader2.Width = 300;
            //
            // columnHeader3
            //
            columnHeader3.Text = "線色";
            columnHeader3.Width = 50;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(12, 303);
            label1.Name = "label1";
            label1.Size = new Size(117, 15);
            label1.TabIndex = 5;
            label1.Text = "ファイルに記録する情報";
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Location = new Point(36, 331);
            label2.Name = "label2";
            label2.Size = new Size(66, 15);
            label2.TabIndex = 6;
            label2.Text = "データ名(&N)";
            //
            // groupBox1
            //
            groupBox1.Controls.Add(radioButtonOneFile);
            groupBox1.Controls.Add(radioButtonSeparateFile);
            groupBox1.Location = new Point(13, 214);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(278, 76);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "分割したデータの記録方法";
            //
            // radioButtonOneFile
            //
            radioButtonOneFile.AutoSize = true;
            radioButtonOneFile.Location = new Point(23, 47);
            radioButtonOneFile.Name = "radioButtonOneFile";
            radioButtonOneFile.Size = new Size(224, 19);
            radioButtonOneFile.TabIndex = 1;
            radioButtonOneFile.TabStop = true;
            radioButtonOneFile.Text = "ファイルは1つでファイル内でトラック分割(&T)";
            radioButtonOneFile.UseVisualStyleBackColor = true;
            //
            // radioButtonSeparateFile
            //
            radioButtonSeparateFile.AutoSize = true;
            radioButtonSeparateFile.Location = new Point(23, 22);
            radioButtonSeparateFile.Name = "radioButtonSeparateFile";
            radioButtonSeparateFile.Size = new Size(177, 19);
            radioButtonSeparateFile.TabIndex = 0;
            radioButtonSeparateFile.TabStop = true;
            radioButtonSeparateFile.Text = "複数のファイルに分けて保存(&M)";
            radioButtonSeparateFile.UseVisualStyleBackColor = true;
            //
            // textBoxDataName
            //
            textBoxDataName.Location = new Point(108, 328);
            textBoxDataName.Name = "textBoxDataName";
            textBoxDataName.Size = new Size(331, 23);
            textBoxDataName.TabIndex = 7;
            //
            // buttonSaveOk
            //
            buttonSaveOk.DialogResult = DialogResult.OK;
            buttonSaveOk.Location = new Point(283, 372);
            buttonSaveOk.Name = "buttonSaveOk";
            buttonSaveOk.Size = new Size(75, 23);
            buttonSaveOk.TabIndex = 0;
            buttonSaveOk.Text = "保存";
            buttonSaveOk.UseVisualStyleBackColor = true;
            //
            // buttonSaveCancel
            //
            buttonSaveCancel.DialogResult = DialogResult.Cancel;
            buttonSaveCancel.Location = new Point(364, 372);
            buttonSaveCancel.Name = "buttonSaveCancel";
            buttonSaveCancel.Size = new Size(75, 23);
            buttonSaveCancel.TabIndex = 1;
            buttonSaveCancel.Text = "キャンセル";
            buttonSaveCancel.UseVisualStyleBackColor = true;
            //
            // labelColorChangeMsg
            //
            labelColorChangeMsg.AutoSize = true;
            labelColorChangeMsg.Location = new Point(202, 191);
            labelColorChangeMsg.Name = "labelColorChangeMsg";
            labelColorChangeMsg.Size = new Size(237, 15);
            labelColorChangeMsg.TabIndex = 8;
            labelColorChangeMsg.Text = "(線色は上の色部分をクリックすると変更できます)";
            //
            // FormSaveNotifyDialogEx
            //
            AcceptButton = buttonSaveOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(451, 407);
            Controls.Add(labelColorChangeMsg);
            Controls.Add(buttonSaveCancel);
            Controls.Add(buttonSaveOk);
            Controls.Add(textBoxDataName);
            Controls.Add(groupBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listViewTrackInfo);
            Controls.Add(labelSaveNotifyMsg);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSaveNotifyDialogEx";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ファイルの保存確認";
            KeyPress += formSaveNotifyDialogEx_KeyPress;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelSaveNotifyMsg;
        private ListView listViewTrackInfo;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private Label label1;
        private Label label2;
        private GroupBox groupBox1;
        private RadioButton radioButtonOneFile;
        private RadioButton radioButtonSeparateFile;
        private TextBox textBoxDataName;
        private Button buttonSaveOk;
        private Button buttonSaveCancel;
        private ColorDialog colorDialog1;
        private Label labelColorChangeMsg;
    }
}
