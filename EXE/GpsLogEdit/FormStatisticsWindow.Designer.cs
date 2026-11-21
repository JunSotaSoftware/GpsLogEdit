namespace GpsLogEdit
{
    partial class FormStatisticsWindow
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
            listViewStatistics = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            label1 = new Label();
            SuspendLayout();
            // 
            // listViewStatistics
            // 
            listViewStatistics.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            listViewStatistics.Location = new Point(48, 76);
            listViewStatistics.MultiSelect = false;
            listViewStatistics.Name = "listViewStatistics";
            listViewStatistics.Size = new Size(693, 241);
            listViewStatistics.TabIndex = 0;
            listViewStatistics.UseCompatibleStateImageBehavior = false;
            listViewStatistics.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "トラック";
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "開始時刻";
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "終了時刻";
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "経過時間";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(325, 27);
            label1.Name = "label1";
            label1.Size = new Size(115, 15);
            label1.TabIndex = 1;
            label1.Text = "この画面は未実装です";
            // 
            // FormStatisticsWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(listViewStatistics);
            Name = "FormStatisticsWindow";
            Text = "統計情報";
            FormClosed += formStatisticWindow_FormClosed;
            Load += form2_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView listViewStatistics;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private Label label1;
    }
}