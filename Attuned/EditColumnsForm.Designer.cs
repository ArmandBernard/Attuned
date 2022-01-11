
namespace Attuned
{
    partial class EditColumnsForm
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
            this.ListCLB = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // ListCLB
            // 
            this.ListCLB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListCLB.BackColor = System.Drawing.SystemColors.Control;
            this.ListCLB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ListCLB.CheckOnClick = true;
            this.ListCLB.FormattingEnabled = true;
            this.ListCLB.IntegralHeight = false;
            this.ListCLB.Location = new System.Drawing.Point(10, 10);
            this.ListCLB.Margin = new System.Windows.Forms.Padding(10);
            this.ListCLB.Name = "ListCLB";
            this.ListCLB.Size = new System.Drawing.Size(241, 295);
            this.ListCLB.TabIndex = 3;
            // 
            // EditColumnsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 316);
            this.Controls.Add(this.ListCLB);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EditColumnsForm";
            this.Text = "EditColumnsForm";
            this.Deactivate += new System.EventHandler(this.EditColumnsForm_Deactivate);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox ListCLB;
    }
}