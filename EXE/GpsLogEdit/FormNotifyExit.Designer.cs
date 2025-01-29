namespace GpsLogEdit
{
    partial class FormNotifyExit
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
            label2 = new Label();
            buttonCancel = new Button();
            buttonSaveToGpx = new Button();
            buttonSaveToKml = new Button();
            buttonNoSave = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(111, 9);
            label1.Name = "label1";
            label1.Size = new Size(134, 15);
            label1.TabIndex = 4;
            label1.Text = "データは編集されています。";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(138, 24);
            label2.Name = "label2";
            label2.Size = new Size(81, 15);
            label2.TabIndex = 5;
            label2.Text = "保存しますか？";
            // 
            // buttonCancel
            // 
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Location = new Point(262, 62);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 0;
            buttonCancel.Text = "戻る";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonSaveToGpx
            // 
            buttonSaveToGpx.DialogResult = DialogResult.Yes;
            buttonSaveToGpx.Location = new Point(100, 62);
            buttonSaveToGpx.Name = "buttonSaveToGpx";
            buttonSaveToGpx.Size = new Size(75, 23);
            buttonSaveToGpx.TabIndex = 2;
            buttonSaveToGpx.Text = "GPXに保存";
            buttonSaveToGpx.UseVisualStyleBackColor = true;
            // 
            // buttonSaveToKml
            // 
            buttonSaveToKml.DialogResult = DialogResult.Retry;
            buttonSaveToKml.Location = new Point(19, 62);
            buttonSaveToKml.Name = "buttonSaveToKml";
            buttonSaveToKml.Size = new Size(75, 23);
            buttonSaveToKml.TabIndex = 3;
            buttonSaveToKml.Text = "KMLに保存";
            buttonSaveToKml.UseVisualStyleBackColor = true;
            // 
            // buttonNoSave
            // 
            buttonNoSave.DialogResult = DialogResult.No;
            buttonNoSave.Location = new Point(181, 62);
            buttonNoSave.Name = "buttonNoSave";
            buttonNoSave.Size = new Size(75, 23);
            buttonNoSave.TabIndex = 1;
            buttonNoSave.Text = "保存しない";
            buttonNoSave.UseVisualStyleBackColor = true;
            // 
            // FormNotifyExit
            // 
            AcceptButton = buttonCancel;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(356, 94);
            Controls.Add(buttonNoSave);
            Controls.Add(buttonSaveToKml);
            Controls.Add(buttonSaveToGpx);
            Controls.Add(buttonCancel);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormNotifyExit";
            StartPosition = FormStartPosition.CenterParent;
            Text = "確認";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Button buttonCancel;
        private Button buttonSaveToGpx;
        private Button buttonSaveToKml;
        private Button buttonNoSave;
    }
}