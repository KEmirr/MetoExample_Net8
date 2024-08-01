namespace MetoFirstExample_v4_WinFormMain
{
    partial class MainForm
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
            button_fileOn = new DevExpress.XtraEditors.SimpleButton();
            memoEditLogs = new DevExpress.XtraEditors.MemoEdit();
            button_fileOff = new DevExpress.XtraEditors.SimpleButton();
            textEdit_Counter = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)memoEditLogs.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)textEdit_Counter.Properties).BeginInit();
            SuspendLayout();
            // 
            // button_fileOn
            // 
            button_fileOn.Location = new System.Drawing.Point(148, 119);
            button_fileOn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button_fileOn.Name = "button_fileOn";
            button_fileOn.Size = new System.Drawing.Size(215, 89);
            button_fileOn.TabIndex = 0;
            button_fileOn.Text = "File Watcher On";
            button_fileOn.Click += button_fileOn_Click;
            // 
            // memoEditLogs
            // 
            memoEditLogs.Dock = System.Windows.Forms.DockStyle.Right;
            memoEditLogs.Location = new System.Drawing.Point(665, 0);
            memoEditLogs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            memoEditLogs.Name = "memoEditLogs";
            memoEditLogs.Size = new System.Drawing.Size(405, 617);
            memoEditLogs.TabIndex = 1;
            // 
            // button_fileOff
            // 
            button_fileOff.Location = new System.Drawing.Point(148, 300);
            button_fileOff.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button_fileOff.Name = "button_fileOff";
            button_fileOff.Size = new System.Drawing.Size(215, 89);
            button_fileOff.TabIndex = 2;
            button_fileOff.Text = "File Watcher Off";
            button_fileOff.Click += button_fileOff_Click;
            // 
            // textEdit_Counter
            // 
            textEdit_Counter.Location = new System.Drawing.Point(370, 154);
            textEdit_Counter.Name = "textEdit_Counter";
            textEdit_Counter.Size = new System.Drawing.Size(230, 20);
            textEdit_Counter.TabIndex = 3;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1070, 617);
            Controls.Add(textEdit_Counter);
            Controls.Add(button_fileOff);
            Controls.Add(memoEditLogs);
            Controls.Add(button_fileOn);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MainForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)memoEditLogs.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)textEdit_Counter.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton button_fileOn;
        private DevExpress.XtraEditors.MemoEdit memoEditLogs;
        private DevExpress.XtraEditors.SimpleButton button_fileOff;
        private DevExpress.XtraEditors.TextEdit textEdit_Counter;
    }
}

