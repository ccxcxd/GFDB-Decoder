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
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabCatchData = new System.Windows.Forms.TabPage();
            this.lblJson = new System.Windows.Forms.Label();
            this.txtJson = new System.Windows.Forms.TextBox();
            this.btnJson = new System.Windows.Forms.Button();
            this.lblSplit = new System.Windows.Forms.Label();
            this.txtSplit = new System.Windows.Forms.TextBox();
            this.chkSplit = new System.Windows.Forms.CheckBox();
            this.lblProcess = new System.Windows.Forms.Label();
            this.txtProcess = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabCatchData.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // tabCatchData
            // 
            this.tabCatchData.Controls.Add(this.btnGo);
            this.tabCatchData.Controls.Add(this.txtProcess);
            this.tabCatchData.Controls.Add(this.txtSplit);
            this.tabCatchData.Controls.Add(this.txtJson);
            this.tabCatchData.Controls.Add(this.lblProcess);
            this.tabCatchData.Controls.Add(this.chkSplit);
            this.tabCatchData.Controls.Add(this.lblSplit);
            this.tabCatchData.Controls.Add(this.btnJson);
            this.tabCatchData.Controls.Add(this.lblJson);
            this.tabCatchData.Location = new System.Drawing.Point(4, 22);
            this.tabCatchData.Name = "tabCatchData";
            this.tabCatchData.Padding = new System.Windows.Forms.Padding(3);
            this.tabCatchData.Size = new System.Drawing.Size(793, 294);
            this.tabCatchData.TabIndex = 0;
            this.tabCatchData.Text = "catchdata";
            this.tabCatchData.UseVisualStyleBackColor = true;
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
            // txtJson
            // 
            this.txtJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJson.Location = new System.Drawing.Point(146, 32);
            this.txtJson.Name = "txtJson";
            this.txtJson.Size = new System.Drawing.Size(497, 26);
            this.txtJson.TabIndex = 10;
            this.txtJson.TextChanged += new System.EventHandler(this.txtJson_TextChanged);
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
            // txtSplit
            // 
            this.txtSplit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSplit.Location = new System.Drawing.Point(146, 86);
            this.txtSplit.Name = "txtSplit";
            this.txtSplit.Size = new System.Drawing.Size(497, 26);
            this.txtSplit.TabIndex = 13;
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
            // txtProcess
            // 
            this.txtProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProcess.Location = new System.Drawing.Point(146, 139);
            this.txtProcess.Name = "txtProcess";
            this.txtProcess.Size = new System.Drawing.Size(497, 26);
            this.txtProcess.TabIndex = 16;
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
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabCatchData);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(801, 320);
            this.tabControl.TabIndex = 0;
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
            this.tabCatchData.ResumeLayout(false);
            this.tabCatchData.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.TabPage tabCatchData;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtProcess;
        private System.Windows.Forms.TextBox txtSplit;
        private System.Windows.Forms.TextBox txtJson;
        private System.Windows.Forms.Label lblProcess;
        private System.Windows.Forms.CheckBox chkSplit;
        private System.Windows.Forms.Label lblSplit;
        private System.Windows.Forms.Button btnJson;
        private System.Windows.Forms.Label lblJson;
        private System.Windows.Forms.TabControl tabControl;
    }
}