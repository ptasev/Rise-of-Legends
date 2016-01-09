namespace RoLBigArchiver
{
    partial class ProgressDialog
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
            this.percentageLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressStatusRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // percentageLabel
            // 
            this.percentageLabel.AutoSize = true;
            this.percentageLabel.Location = new System.Drawing.Point(419, 11);
            this.percentageLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.percentageLabel.Name = "percentageLabel";
            this.percentageLabel.Size = new System.Drawing.Size(33, 13);
            this.percentageLabel.TabIndex = 5;
            this.percentageLabel.Text = "100%";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 11);
            this.progressBar.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(403, 13);
            this.progressBar.TabIndex = 4;
            // 
            // progressStatusRichTextBox
            // 
            this.progressStatusRichTextBox.Location = new System.Drawing.Point(12, 29);
            this.progressStatusRichTextBox.Name = "progressStatusRichTextBox";
            this.progressStatusRichTextBox.Size = new System.Drawing.Size(440, 240);
            this.progressStatusRichTextBox.TabIndex = 6;
            this.progressStatusRichTextBox.Text = "";
            // 
            // ProgressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 281);
            this.Controls.Add(this.progressStatusRichTextBox);
            this.Controls.Add(this.percentageLabel);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressDialog";
            this.Text = "ProgressDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label percentageLabel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.RichTextBox progressStatusRichTextBox;
    }
}