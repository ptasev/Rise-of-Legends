using BigHugeEngineLibrary.Archive.Big;
using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoLBigArchiver
{
    public partial class Form1 : Form
    {
        private BigFile file;
        private ProgressDialog progDiag;

        public Form1()
        {
            InitializeComponent();
            this.Text = Properties.Resources.AppTitleLong;
            this.ShowIcon = false;

            this.openFileDialog.Filter = "Big files|*.big|All files|*.*";
            this.saveFileDialog.Filter = "Big files|*.big|All files|*.*";

            this.entriesObjectListView.ShowGroups = false;
            this.entriesObjectListView.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;

            OLVColumn nameCol = new OLVColumn("Name", "Name");
            nameCol.Width = 225;
            nameCol.IsEditable = false;
            this.entriesObjectListView.Columns.Add(nameCol);

            OLVColumn fileTypeCol = new OLVColumn("File Type", "Type");
            fileTypeCol.Width = 150;
            this.entriesObjectListView.Columns.Add(fileTypeCol);

            OLVColumn sizeCol = new OLVColumn("Size", "Size");
            sizeCol.Width = 100;
            sizeCol.IsEditable = false;
            this.entriesObjectListView.Columns.Add(sizeCol);

            OLVColumn pSizeCol = new OLVColumn("Packed Size", "CompressedSize");
            pSizeCol.Width = 100;
            pSizeCol.IsEditable = false;
            this.entriesObjectListView.Columns.Add(pSizeCol);

            OLVColumn fullPathCol = new OLVColumn("Full Path", "FileName");
            //fullPathCol.Width = 500;
            fullPathCol.FillsFreeSpace = true;
            fullPathCol.IsEditable = false;
            this.entriesObjectListView.Columns.Add(fullPathCol);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.file = new BigFile();
                this.file.Read(File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read));

                this.entriesObjectListView.SetObjects(this.file.Entries);
                this.Text = Properties.Resources.AppTitleShort + " - " + Path.GetFileName(openFileDialog.FileName);
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (file == null || file.Entries.Count == 0)
            {
                return;
            }

            folderBrowserDialog.Description = "Select a folder to export the files:";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    progDiag = new ProgressDialog(this.file);
                    Thread exportThread = new Thread(() => file.Export(folderBrowserDialog.SelectedPath));
                    exportThread.IsBackground = true;
                    exportThread.Start();
                    progDiag.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed Exporting!" + Environment.NewLine + Environment.NewLine +
                        ex.Message, "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.Description = "Select a folder to import the files from:";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    progDiag = new ProgressDialog(this.file);
                    Thread importThread = new Thread(() => file.Import(folderBrowserDialog.SelectedPath));
                    importThread.IsBackground = true;
                    importThread.Start();
                    progDiag.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed Importing!" + Environment.NewLine + Environment.NewLine +
                        ex.Message, "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (file == null || file.Entries.Count == 0)
            {
                return;
            }

            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.file.Write(File.Open(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.Read));
                this.Text = Properties.Resources.AppTitleShort + " - " + Path.GetFileName(saveFileDialog.FileName);
            }
        }
    }
}
