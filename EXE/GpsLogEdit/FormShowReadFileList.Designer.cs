namespace GpsLogEdit
{
    partial class FormShowReadFileList
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
            label1 = new Label();
            listReadFileList = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            button1 = new Button();
            SuspendLayout();
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(168, 15);
            label1.TabIndex = 2;
            label1.Text = "以下のファイルを読み込んでいます";
            //
            // listReadFileList
            //
            listReadFileList.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            listReadFileList.FullRowSelect = true;
            listReadFileList.Location = new Point(31, 27);
            listReadFileList.Name = "listReadFileList";
            listReadFileList.Size = new Size(688, 162);
            listReadFileList.TabIndex = 1;
            listReadFileList.UseCompatibleStateImageBehavior = false;
            listReadFileList.View = View.Details;
            //
            // columnHeader1
            //
            columnHeader1.Text = "ファイル＃";
            //
            // columnHeader2
            //
            columnHeader2.Text = "ファイル名";
            columnHeader2.Width = 600;
            //
            // button1
            //
            button1.DialogResult = DialogResult.OK;
            button1.Location = new Point(644, 205);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            //
            // FormShowReadFileList
            //
            AcceptButton = button1;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(731, 240);
            Controls.Add(button1);
            Controls.Add(listReadFileList);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormShowReadFileList";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "読み込んでいるファイルの一覧";
            KeyPress += formShowReadFileList_KeyPress;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ListView listReadFileList;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Button button1;
    }
}
