namespace GpsLogEdit
{
    partial class FormAboutDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAboutDialog));
            label1 = new Label();
            buttonAboutOk = new Button();
            pictureBox1 = new PictureBox();
            linkLabel2 = new LinkLabel();
            linkLabel1 = new LinkLabel();
            linkLabel4 = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(122, 29);
            label1.Name = "label1";
            label1.Size = new Size(106, 15);
            label1.TabIndex = 0;
            label1.Text = "GpsLogEdit Ver.1.0.0";
            //
            // buttonAboutOk
            //
            buttonAboutOk.DialogResult = DialogResult.OK;
            buttonAboutOk.Location = new Point(263, 147);
            buttonAboutOk.Name = "buttonAboutOk";
            buttonAboutOk.Size = new Size(75, 23);
            buttonAboutOk.TabIndex = 1;
            buttonAboutOk.Text = "OK";
            buttonAboutOk.UseVisualStyleBackColor = true;
            //
            // pictureBox1
            //
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(32, 32);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            //
            // linkLabel2
            //
            linkLabel2.AutoSize = true;
            linkLabel2.LinkArea = new LinkArea(8, 13);
            linkLabel2.Location = new Point(39, 84);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new Size(273, 20);
            linkLabel2.TabIndex = 4;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "表示する地図は OpenStreetMap を使用しています.";
            linkLabel2.UseCompatibleTextRendering = true;
            linkLabel2.LinkClicked += linkLabel2_LinkClicked;
            //
            // linkLabel1
            //
            linkLabel1.AutoSize = true;
            linkLabel1.LinkArea = new LinkArea(23, 4);
            linkLabel1.Location = new Point(100, 55);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(188, 20);
            linkLabel1.TabIndex = 5;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Copyright(C) 2024-2025 Sota.";
            linkLabel1.UseCompatibleTextRendering = true;
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            //
            // linkLabel4
            //
            linkLabel4.AutoSize = true;
            linkLabel4.LinkArea = new LinkArea(7, 6);
            linkLabel4.Location = new Point(48, 113);
            linkLabel4.Name = "linkLabel4";
            linkLabel4.Size = new Size(254, 20);
            linkLabel4.TabIndex = 6;
            linkLabel4.TabStop = true;
            linkLabel4.Text = "地図の表示は Mapsui ライブラリを使用しています.";
            linkLabel4.UseCompatibleTextRendering = true;
            linkLabel4.LinkClicked += linkLabel4_LinkClicked;
            //
            // FormAboutDialog
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(350, 178);
            Controls.Add(linkLabel4);
            Controls.Add(linkLabel1);
            Controls.Add(linkLabel2);
            Controls.Add(pictureBox1);
            Controls.Add(buttonAboutOk);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAboutDialog";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "GpsLogEditについて";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button buttonAboutOk;
        private PictureBox pictureBox1;
        private LinkLabel linkLabel2;
        private LinkLabel linkLabel1;
        private LinkLabel linkLabel4;
    }
}
