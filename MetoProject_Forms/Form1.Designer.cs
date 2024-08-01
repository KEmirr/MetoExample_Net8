namespace MetoProject_Forms
{
    partial class Form1
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
            memoEditLogs = new DevExpress.XtraEditors.MemoEdit();
            button_fileOn = new DevExpress.XtraEditors.SimpleButton();
            button_fileOff = new DevExpress.XtraEditors.SimpleButton();
            textEdit_Counter = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)memoEditLogs.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)textEdit_Counter.Properties).BeginInit();
            SuspendLayout();
            // 
            // memoEditLogs
            // 
            memoEditLogs.Dock = System.Windows.Forms.DockStyle.Right;
            memoEditLogs.Location = new System.Drawing.Point(244, 0);
            memoEditLogs.Name = "memoEditLogs";
            memoEditLogs.Size = new System.Drawing.Size(378, 533);
            memoEditLogs.TabIndex = 0;
            // 
            // button_fileOn
            // 
            button_fileOn.Location = new System.Drawing.Point(12, 88);
            button_fileOn.Name = "button_fileOn";
            button_fileOn.Size = new System.Drawing.Size(199, 112);
            button_fileOn.TabIndex = 1;
            button_fileOn.Text = "File Wacther On";
            button_fileOn.Click += button_fileOn_Click;
            // 
            // button_fileOff
            // 
            button_fileOff.Location = new System.Drawing.Point(12, 257);
            button_fileOff.Name = "button_fileOff";
            button_fileOff.Size = new System.Drawing.Size(199, 112);
            button_fileOff.TabIndex = 2;
            button_fileOff.Text = "File Wacther Off";
            button_fileOff.Click += button_fileOff_Click;
            // 
            // textEdit_Counter
            // 
            textEdit_Counter.Location = new System.Drawing.Point(12, 218);
            textEdit_Counter.Name = "textEdit_Counter";
            textEdit_Counter.Size = new System.Drawing.Size(199, 20);
            textEdit_Counter.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(622, 533);
            Controls.Add(textEdit_Counter);
            Controls.Add(button_fileOff);
            Controls.Add(button_fileOn);
            Controls.Add(memoEditLogs);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)memoEditLogs.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)textEdit_Counter.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit memoEditLogs;
        private DevExpress.XtraEditors.SimpleButton button_fileOn;
        private DevExpress.XtraEditors.SimpleButton button_fileOff;
        private DevExpress.XtraEditors.TextEdit textEdit_Counter;
    }
}

