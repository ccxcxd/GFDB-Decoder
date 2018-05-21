namespace GFDecoder
{
    partial class ToolForm
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
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabCatchData = new System.Windows.Forms.TabPage();
            this.btnGo = new System.Windows.Forms.Button();
            this.txtProcess = new System.Windows.Forms.TextBox();
            this.lblProcess = new System.Windows.Forms.Label();
            this.chkSplit = new System.Windows.Forms.CheckBox();
            this.txtSplit = new System.Windows.Forms.TextBox();
            this.lblSplit = new System.Windows.Forms.Label();
            this.btnJson = new System.Windows.Forms.Button();
            this.txtJson = new System.Windows.Forms.TextBox();
            this.lblJson = new System.Windows.Forms.Label();
            this.tabText = new System.Windows.Forms.TabPage();
            this.btnGoText = new System.Windows.Forms.Button();
            this.btnTextOutput = new System.Windows.Forms.Button();
            this.btnTextFile = new System.Windows.Forms.Button();
            this.txtTextOutput = new System.Windows.Forms.TextBox();
            this.txtTextFile = new System.Windows.Forms.TextBox();
            this.lblTextOutput = new System.Windows.Forms.Label();
            this.lblTextFile = new System.Windows.Forms.Label();
            this.tabImage = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.txtImageTo = new System.Windows.Forms.TextBox();
            this.btnGoImage = new System.Windows.Forms.Button();
            this.txtImageFrom = new System.Windows.Forms.TextBox();
            this.txtImage = new System.Windows.Forms.TextBox();
            this.lblImage = new System.Windows.Forms.Label();
            this.btnImage = new System.Windows.Forms.Button();
            this.btnImageJson = new System.Windows.Forms.Button();
            this.txtImageJson = new System.Windows.Forms.TextBox();
            this.lblImageJson = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabControl.SuspendLayout();
            this.tabCatchData.SuspendLayout();
            this.tabText.SuspendLayout();
            this.tabImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabCatchData);
            this.tabControl.Controls.Add(this.tabText);
            this.tabControl.Controls.Add(this.tabImage);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(801, 320);
            this.tabControl.TabIndex = 0;
            // 
            // tabCatchData
            // 
            this.tabCatchData.Controls.Add(this.btnGo);
            this.tabCatchData.Controls.Add(this.txtProcess);
            this.tabCatchData.Controls.Add(this.lblProcess);
            this.tabCatchData.Controls.Add(this.chkSplit);
            this.tabCatchData.Controls.Add(this.txtSplit);
            this.tabCatchData.Controls.Add(this.lblSplit);
            this.tabCatchData.Controls.Add(this.btnJson);
            this.tabCatchData.Controls.Add(this.txtJson);
            this.tabCatchData.Controls.Add(this.lblJson);
            this.tabCatchData.Location = new System.Drawing.Point(4, 22);
            this.tabCatchData.Name = "tabCatchData";
            this.tabCatchData.Padding = new System.Windows.Forms.Padding(3);
            this.tabCatchData.Size = new System.Drawing.Size(793, 294);
            this.tabCatchData.TabIndex = 0;
            this.tabCatchData.Text = "catchdata";
            this.tabCatchData.UseVisualStyleBackColor = true;
            // 
            // btnGo
            // 
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.Location = new System.Drawing.Point(252, 202);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(283, 60);
            this.btnGo.TabIndex = 17;
            this.btnGo.Text = "执行";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtProcess
            // 
            this.txtProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProcess.Location = new System.Drawing.Point(146, 139);
            this.txtProcess.Name = "txtProcess";
            this.txtProcess.Size = new System.Drawing.Size(497, 26);
            this.txtProcess.TabIndex = 16;
            this.txtProcess.TextChanged += new System.EventHandler(this.txtProcess_TextChanged);
            // 
            // lblProcess
            // 
            this.lblProcess.AutoSize = true;
            this.lblProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcess.Location = new System.Drawing.Point(81, 142);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(41, 20);
            this.lblProcess.TabIndex = 15;
            this.lblProcess.Text = "处理";
            // 
            // chkSplit
            // 
            this.chkSplit.AutoSize = true;
            this.chkSplit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSplit.Location = new System.Drawing.Point(649, 85);
            this.chkSplit.Name = "chkSplit";
            this.chkSplit.Size = new System.Drawing.Size(60, 24);
            this.chkSplit.TabIndex = 14;
            this.chkSplit.Text = "输出";
            this.chkSplit.UseVisualStyleBackColor = true;
            this.chkSplit.CheckedChanged += new System.EventHandler(this.chkSplit_CheckedChanged);
            // 
            // txtSplit
            // 
            this.txtSplit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSplit.Location = new System.Drawing.Point(146, 86);
            this.txtSplit.Name = "txtSplit";
            this.txtSplit.Size = new System.Drawing.Size(497, 26);
            this.txtSplit.TabIndex = 13;
            // 
            // lblSplit
            // 
            this.lblSplit.AutoSize = true;
            this.lblSplit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSplit.Location = new System.Drawing.Point(81, 89);
            this.lblSplit.Name = "lblSplit";
            this.lblSplit.Size = new System.Drawing.Size(41, 20);
            this.lblSplit.TabIndex = 12;
            this.lblSplit.Text = "拆分";
            // 
            // btnJson
            // 
            this.btnJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJson.Location = new System.Drawing.Point(649, 32);
            this.btnJson.Name = "btnJson";
            this.btnJson.Size = new System.Drawing.Size(62, 26);
            this.btnJson.TabIndex = 11;
            this.btnJson.Text = "浏览...";
            this.btnJson.UseVisualStyleBackColor = true;
            this.btnJson.Click += new System.EventHandler(this.btnJson_Click);
            // 
            // txtJson
            // 
            this.txtJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJson.Location = new System.Drawing.Point(146, 32);
            this.txtJson.Name = "txtJson";
            this.txtJson.Size = new System.Drawing.Size(497, 26);
            this.txtJson.TabIndex = 10;
            this.txtJson.TextChanged += new System.EventHandler(this.txtJson_TextChanged);
            // 
            // lblJson
            // 
            this.lblJson.AutoSize = true;
            this.lblJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJson.Location = new System.Drawing.Point(81, 35);
            this.lblJson.Name = "lblJson";
            this.lblJson.Size = new System.Drawing.Size(59, 20);
            this.lblJson.TabIndex = 9;
            this.lblJson.Text = "源Json";
            // 
            // tabText
            // 
            this.tabText.Controls.Add(this.btnGoText);
            this.tabText.Controls.Add(this.btnTextOutput);
            this.tabText.Controls.Add(this.btnTextFile);
            this.tabText.Controls.Add(this.txtTextOutput);
            this.tabText.Controls.Add(this.txtTextFile);
            this.tabText.Controls.Add(this.lblTextOutput);
            this.tabText.Controls.Add(this.lblTextFile);
            this.tabText.Location = new System.Drawing.Point(4, 22);
            this.tabText.Name = "tabText";
            this.tabText.Padding = new System.Windows.Forms.Padding(3);
            this.tabText.Size = new System.Drawing.Size(793, 294);
            this.tabText.TabIndex = 1;
            this.tabText.Text = "文本";
            this.tabText.UseVisualStyleBackColor = true;
            // 
            // btnGoText
            // 
            this.btnGoText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGoText.Location = new System.Drawing.Point(252, 202);
            this.btnGoText.Name = "btnGoText";
            this.btnGoText.Size = new System.Drawing.Size(283, 60);
            this.btnGoText.TabIndex = 18;
            this.btnGoText.Text = "执行";
            this.btnGoText.UseVisualStyleBackColor = true;
            this.btnGoText.Click += new System.EventHandler(this.btnGoText_Click);
            // 
            // btnTextOutput
            // 
            this.btnTextOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTextOutput.Location = new System.Drawing.Point(649, 86);
            this.btnTextOutput.Name = "btnTextOutput";
            this.btnTextOutput.Size = new System.Drawing.Size(62, 26);
            this.btnTextOutput.TabIndex = 14;
            this.btnTextOutput.Text = "浏览...";
            this.btnTextOutput.UseVisualStyleBackColor = true;
            this.btnTextOutput.Click += new System.EventHandler(this.btnTextOutput_Click);
            // 
            // btnTextFile
            // 
            this.btnTextFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTextFile.Location = new System.Drawing.Point(649, 32);
            this.btnTextFile.Name = "btnTextFile";
            this.btnTextFile.Size = new System.Drawing.Size(62, 26);
            this.btnTextFile.TabIndex = 14;
            this.btnTextFile.Text = "浏览...";
            this.btnTextFile.UseVisualStyleBackColor = true;
            this.btnTextFile.Click += new System.EventHandler(this.btnTextFile_Click);
            // 
            // txtTextOutput
            // 
            this.txtTextOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTextOutput.Location = new System.Drawing.Point(146, 86);
            this.txtTextOutput.Name = "txtTextOutput";
            this.txtTextOutput.Size = new System.Drawing.Size(497, 26);
            this.txtTextOutput.TabIndex = 13;
            // 
            // txtTextFile
            // 
            this.txtTextFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTextFile.Location = new System.Drawing.Point(146, 32);
            this.txtTextFile.Name = "txtTextFile";
            this.txtTextFile.Size = new System.Drawing.Size(497, 26);
            this.txtTextFile.TabIndex = 13;
            // 
            // lblTextOutput
            // 
            this.lblTextOutput.AutoSize = true;
            this.lblTextOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextOutput.Location = new System.Drawing.Point(81, 89);
            this.lblTextOutput.Name = "lblTextOutput";
            this.lblTextOutput.Size = new System.Drawing.Size(41, 20);
            this.lblTextOutput.TabIndex = 12;
            this.lblTextOutput.Text = "输出";
            // 
            // lblTextFile
            // 
            this.lblTextFile.AutoSize = true;
            this.lblTextFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextFile.Location = new System.Drawing.Point(81, 35);
            this.lblTextFile.Name = "lblTextFile";
            this.lblTextFile.Size = new System.Drawing.Size(57, 20);
            this.lblTextFile.TabIndex = 12;
            this.lblTextFile.Text = "文件夹";
            // 
            // tabImage
            // 
            this.tabImage.Controls.Add(this.label1);
            this.tabImage.Controls.Add(this.txtImageTo);
            this.tabImage.Controls.Add(this.btnGoImage);
            this.tabImage.Controls.Add(this.txtImageFrom);
            this.tabImage.Controls.Add(this.txtImage);
            this.tabImage.Controls.Add(this.lblImage);
            this.tabImage.Controls.Add(this.btnImage);
            this.tabImage.Controls.Add(this.btnImageJson);
            this.tabImage.Controls.Add(this.txtImageJson);
            this.tabImage.Controls.Add(this.lblImageJson);
            this.tabImage.Location = new System.Drawing.Point(4, 22);
            this.tabImage.Name = "tabImage";
            this.tabImage.Size = new System.Drawing.Size(793, 294);
            this.tabImage.TabIndex = 2;
            this.tabImage.Text = "图片";
            this.tabImage.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(209, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 20);
            this.label1.TabIndex = 28;
            this.label1.Text = "~";
            // 
            // txtImageTo
            // 
            this.txtImageTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImageTo.Location = new System.Drawing.Point(234, 142);
            this.txtImageTo.Name = "txtImageTo";
            this.txtImageTo.Size = new System.Drawing.Size(57, 26);
            this.txtImageTo.TabIndex = 27;
            this.txtImageTo.Text = "99999";
            // 
            // btnGoImage
            // 
            this.btnGoImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGoImage.Location = new System.Drawing.Point(252, 202);
            this.btnGoImage.Name = "btnGoImage";
            this.btnGoImage.Size = new System.Drawing.Size(283, 60);
            this.btnGoImage.TabIndex = 26;
            this.btnGoImage.Text = "执行";
            this.btnGoImage.UseVisualStyleBackColor = true;
            this.btnGoImage.Click += new System.EventHandler(this.btnGoImage_Click);
            // 
            // txtImageFrom
            // 
            this.txtImageFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImageFrom.Location = new System.Drawing.Point(146, 142);
            this.txtImageFrom.Name = "txtImageFrom";
            this.txtImageFrom.Size = new System.Drawing.Size(57, 26);
            this.txtImageFrom.TabIndex = 22;
            this.txtImageFrom.Text = "1";
            // 
            // txtImage
            // 
            this.txtImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImage.Location = new System.Drawing.Point(146, 86);
            this.txtImage.Name = "txtImage";
            this.txtImage.Size = new System.Drawing.Size(497, 26);
            this.txtImage.TabIndex = 22;
            // 
            // lblImage
            // 
            this.lblImage.AutoSize = true;
            this.lblImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImage.Location = new System.Drawing.Point(81, 89);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(41, 20);
            this.lblImage.TabIndex = 21;
            this.lblImage.Text = "图片";
            // 
            // btnImage
            // 
            this.btnImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImage.Location = new System.Drawing.Point(649, 86);
            this.btnImage.Name = "btnImage";
            this.btnImage.Size = new System.Drawing.Size(62, 26);
            this.btnImage.TabIndex = 20;
            this.btnImage.Text = "浏览...";
            this.btnImage.UseVisualStyleBackColor = true;
            this.btnImage.Click += new System.EventHandler(this.btnImage_Click);
            // 
            // btnImageJson
            // 
            this.btnImageJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImageJson.Location = new System.Drawing.Point(649, 32);
            this.btnImageJson.Name = "btnImageJson";
            this.btnImageJson.Size = new System.Drawing.Size(62, 26);
            this.btnImageJson.TabIndex = 20;
            this.btnImageJson.Text = "浏览...";
            this.btnImageJson.UseVisualStyleBackColor = true;
            this.btnImageJson.Click += new System.EventHandler(this.btnImageJson_Click);
            // 
            // txtImageJson
            // 
            this.txtImageJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImageJson.Location = new System.Drawing.Point(146, 32);
            this.txtImageJson.Name = "txtImageJson";
            this.txtImageJson.Size = new System.Drawing.Size(497, 26);
            this.txtImageJson.TabIndex = 19;
            // 
            // lblImageJson
            // 
            this.lblImageJson.AutoSize = true;
            this.lblImageJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImageJson.Location = new System.Drawing.Point(81, 35);
            this.lblImageJson.Name = "lblImageJson";
            this.lblImageJson.Size = new System.Drawing.Size(41, 20);
            this.lblImageJson.TabIndex = 18;
            this.lblImageJson.Text = "处理";
            // 
            // ToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 320);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ToolForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolForm_FormClosing);
            this.Load += new System.EventHandler(this.ToolForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabCatchData.ResumeLayout(false);
            this.tabCatchData.PerformLayout();
            this.tabText.ResumeLayout(false);
            this.tabText.PerformLayout();
            this.tabImage.ResumeLayout(false);
            this.tabImage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabCatchData;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtProcess;
        private System.Windows.Forms.Label lblProcess;
        private System.Windows.Forms.CheckBox chkSplit;
        private System.Windows.Forms.TextBox txtSplit;
        private System.Windows.Forms.Label lblSplit;
        private System.Windows.Forms.Button btnJson;
        private System.Windows.Forms.TextBox txtJson;
        private System.Windows.Forms.Label lblJson;
        private System.Windows.Forms.TabPage tabText;
        private System.Windows.Forms.Button btnGoText;
        private System.Windows.Forms.Button btnTextFile;
        private System.Windows.Forms.TextBox txtTextFile;
        private System.Windows.Forms.Label lblTextFile;
        private System.Windows.Forms.TabPage tabImage;
        private System.Windows.Forms.Button btnGoImage;
        private System.Windows.Forms.TextBox txtImage;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.Button btnImage;
        private System.Windows.Forms.Button btnImageJson;
        private System.Windows.Forms.TextBox txtImageJson;
        private System.Windows.Forms.Label lblImageJson;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtImageTo;
        private System.Windows.Forms.TextBox txtImageFrom;
        private System.Windows.Forms.Button btnTextOutput;
        private System.Windows.Forms.TextBox txtTextOutput;
        private System.Windows.Forms.Label lblTextOutput;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}