using BigHugeEngineLibrary.Archive.Big;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoLBigArchiver
{
    public partial class ProgressDialog : Form
    {
        public ProgressDialog(BigFile file)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.progressBar.Style = ProgressBarStyle.Blocks;
            this.progressStatusRichTextBox.HideSelection = false;
            file.ImportProgressChanged += file_ImportProgressChanged;
            file.ExportProgressChanged += file_ExportProgressChanged;
        }

        public void SetProgressText(string text)
        {
            if (progressStatusRichTextBox.InvokeRequired)
            {
                progressStatusRichTextBox.BeginInvoke(new Action(() =>
                {
                    progressStatusRichTextBox.AppendText(text);
                }));
            }
            else
            {
                progressStatusRichTextBox.AppendText(text);
            }
        }

        public void SetProgressValue(int value)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.BeginInvoke(new Action(() =>
                {
                        progressBar.Value = value;
                }));
            }
            else
            {
                progressBar.Value = value;
            }

            if (percentageLabel.InvokeRequired)
            {
                percentageLabel.BeginInvoke(new Action(() =>
                {
                    percentageLabel.Text = string.Format("{0}%", value);
                }));
            }
            else
            {
                percentageLabel.Text = string.Format("{0}%", value);
            }
        }

        private void file_ImportProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.SetProgressValue(e.ProgressPercentage);
            this.SetProgressText((string)e.UserState);
        }

        private void file_ExportProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.SetProgressValue(e.ProgressPercentage);
            this.SetProgressText((string)e.UserState);
        }
    }
}
