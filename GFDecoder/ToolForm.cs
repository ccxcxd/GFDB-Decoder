using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GFDecoder
{
    public partial class ToolForm : Form
    {
        public ToolForm()
        {
            InitializeComponent();
        }

        private void ToolForm_Load(object sender, EventArgs e)
        {
            #region String Resource Assignement
            this.Text = Properties.Resources.ToolFormText;
            lblJson.Text = Properties.Resources.lblJsonText;
            lblSplit.Text = Properties.Resources.lblSplitText;
            lblProcess.Text = Properties.Resources.lblProcessText;
            btnJson.Text = Properties.Resources.btnJsonText;
            btnGo.Text = Properties.Resources.btnGoText;
            chkSplit.Text = Properties.Resources.chkSplitText;
            #endregion

            UpdateFromSettings();
        }

        private void UpdateFromSettings()
        {
            txtJson.Text = Properties.Settings.Default.jsonPath;
            txtSplit.Text = Properties.Settings.Default.splitPath;
            txtProcess.Text = Properties.Settings.Default.processPath;
            chkSplit.Checked = Properties.Settings.Default.doSplit;
            chkSplit_CheckedChanged(null,null);
        }

        private void SaveToSettings()
        {
            Properties.Settings.Default.jsonPath = txtJson.Text;
            Properties.Settings.Default.splitPath = txtSplit.Text;
            Properties.Settings.Default.processPath = txtProcess.Text;
            Properties.Settings.Default.doSplit = chkSplit.Checked;
            Properties.Settings.Default.Save();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            string jsonpath = txtJson.Text;
            string splitpath = txtSplit.Text;
            bool doSplit = chkSplit.Checked;
            string processpath = txtProcess.Text;

            SaveToSettings();

            try
            {
                var jsons = GFDecoder.LoadCatchDataJsonFile(new FileStream(jsonpath, FileMode.Open));

                if (doSplit)
                {
                    GFDecoder.SaveSplitedJsonFiles(jsons, splitpath);
                }

                GFDecoder.ProcessJsonData(jsons, processpath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtJson_TextChanged(object sender, EventArgs e)
        {
            string jsonpath = txtJson.Text;
            string dir = Path.GetDirectoryName(jsonpath);
            if (Properties.Settings.Default.splitDirName != "")
                txtSplit.Text = Path.Combine(dir, Properties.Settings.Default.splitDirName);
            if (Properties.Settings.Default.processDirName != "")
                txtProcess.Text = Path.Combine(dir, Properties.Settings.Default.processDirName);
        }

        private void chkSplit_CheckedChanged(object sender, EventArgs e)
        {
            txtSplit.Enabled = chkSplit.Checked;
        }

        private void btnJson_Click(object sender, EventArgs e)
        {
            string jsonpath = txtJson.Text;
            if (File.Exists(jsonpath))
                openFileDialog.InitialDirectory = Path.GetDirectoryName(jsonpath);

            openFileDialog.Filter = Properties.Resources.openFileDialogFilter;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtJson.Text = openFileDialog.FileName;
            }
        }

        private void ToolForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveToSettings();
        }
    }
}
