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
            this.Text = Properties.Resources.toolFormText;
            lblJson.Text = Properties.Resources.lblJsonText;
            lblSplit.Text = Properties.Resources.lblSplitText;
            lblProcess.Text = Properties.Resources.lblProcessText;
            btnJson.Text = Properties.Resources.browse;
            btnGo.Text = Properties.Resources.btnGoText;
            chkSplit.Text = Properties.Resources.chkSplitText;
            tabCatchData.Text = Properties.Resources.tabCatchDataText;
            tabText.Text = Properties.Resources.tabTextText;
            lblTextFile.Text = Properties.Resources.lblTextFileText;
            btnTextFile.Text =Properties.Resources.browse;
            lblTextOutput.Text = Properties.Resources.lbltextOutputText;
            btnTextOutput.Text = Properties.Resources.browse;
            btnGoText.Text = Properties.Resources.btnGoText;
            tabImage.Text = Properties.Resources.tabImageText;
            lblJson.Text = Properties.Resources.lblJsonText;
            lblImage.Text = Properties.Resources.lblImageText;
            btnImageJson.Text = Properties.Resources.lblProcessText;
            btnImage.Text = Properties.Resources.browse;
            btnGoImage.Text = Properties.Resources.btnGoText;
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
            txtTextFile.Text = Properties.Settings.Default.textFilePath;
            txtTextOutput.Text = Properties.Settings.Default.textOutputPath;
            txtImage.Text = Properties.Settings.Default.ImagePath;
            txtImageJson.Text = Properties.Settings.Default.ImageJsonPath;
        }

        private void SaveToSettings()
        {
            Properties.Settings.Default.jsonPath = txtJson.Text;
            Properties.Settings.Default.splitPath = txtSplit.Text;
            Properties.Settings.Default.processPath = txtProcess.Text;
            Properties.Settings.Default.doSplit = chkSplit.Checked;
            Properties.Settings.Default.textFilePath = txtTextFile.Text;
            Properties.Settings.Default.textOutputPath = txtTextOutput.Text;
            Properties.Settings.Default.ImagePath = txtImage.Text;
            Properties.Settings.Default.ImageJsonPath = txtImageJson.Text;
            Properties.Settings.Default.Save();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            string jsonpath = txtJson.Text;
            string splitpath = txtSplit.Text;
            bool doSplit = chkSplit.Checked;
            string processpath = txtProcess.Text;

            SaveToSettings();

            //try
            //{
                var jsons = GFDecoder.LoadCatchDataJsonFile(new FileStream(jsonpath, FileMode.Open));

                if (doSplit)
                {
                    GFDecoder.SaveSplitedJsonFiles(jsons, splitpath);
                    foreach (var f in Directory.GetFiles(splitpath, "*.json"))
                    {
                        string outputpath = Path.Combine(Path.GetDirectoryName(f), Path.GetFileNameWithoutExtension(f) + ".csv");
                        GFDecoder.Json2Csv(f, outputpath);
                    }
                }

                GFDecoder.ProcessJsonData(jsons, processpath);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
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
            DoOpenFileDialog(txtJson, Properties.Resources.openFileDialogJsonFilter);
        }

        private void ToolForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveToSettings();
        }

        private void btnTextFile_Click(object sender, EventArgs e)
        {
            DoOpenFileDialog(txtTextFile, Properties.Resources.openFileDialogAllFilter);
        }

        private void btnGoText_Click(object sender, EventArgs e)
        {
            string filepath = txtTextFile.Text;
            string outputpath = txtTextOutput.Text;

            SaveToSettings();

            GFDecoder.Avgtext2Js(filepath, outputpath);
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            DoOpenFileDialog(txtImage, Properties.Resources.openFileDialogAllFilter);
        }

        private void btnGoImage_Click(object sender, EventArgs e)
        {

            SaveToSettings();

            //try
            //{
                string jsonpath = txtImageJson.Text;
                string imagepath = txtImage.Text;
                int from = int.Parse(txtImageFrom.Text);
                int to = int.Parse(txtImageTo.Text);
            GFDecoder.ProcessImages(jsonpath, imagepath, imagepath, from, to);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex);
            //}
        }

        private void btnImageJson_Click(object sender, EventArgs e)
        {
            DoOpenFileDialog(txtImageJson, Properties.Resources.openFileDialogJsonFilter);
        }

        private void DoOpenFileDialog(TextBox txtbox, string filter)
        {
            string filepath = txtbox.Text;
            if (File.Exists(filepath))
                openFileDialog.InitialDirectory = Path.GetDirectoryName(filepath);

            openFileDialog.Filter = filter;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtbox.Text = openFileDialog.FileName;
            }
        }

        private void DoSaveFileDialog(TextBox txtbox, string filter)
        {
            string filepath = txtbox.Text;
            if (File.Exists(filepath))
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(filepath);

            saveFileDialog.Filter = filter;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtbox.Text = saveFileDialog.FileName;
            }
        }

        private void txtProcess_TextChanged(object sender, EventArgs e)
        {
            txtImageJson.Text = Path.Combine(txtProcess.Text, typeof(mission_info).Name + ",json");
        }

        private void btnTextOutput_Click(object sender, EventArgs e)
        {
            DoSaveFileDialog(txtTextOutput, Properties.Resources.openFileDialogAllFilter);
        }
    }
}
