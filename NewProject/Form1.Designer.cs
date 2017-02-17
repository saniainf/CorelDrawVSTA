namespace NewProject1
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
            this.button1 = new System.Windows.Forms.Button();
            this.listFindItem = new System.Windows.Forms.ListBox();
            this.listSelectItem = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 603);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listFindItem
            // 
            this.listFindItem.FormattingEnabled = true;
            this.listFindItem.Location = new System.Drawing.Point(12, 11);
            this.listFindItem.Name = "listFindItem";
            this.listFindItem.Size = new System.Drawing.Size(251, 303);
            this.listFindItem.TabIndex = 1;
            this.listFindItem.SelectedIndexChanged += new System.EventHandler(this.listFindItem_SelectedIndexChanged);
            // 
            // listSelectItem
            // 
            this.listSelectItem.FormattingEnabled = true;
            this.listSelectItem.Location = new System.Drawing.Point(12, 320);
            this.listSelectItem.Name = "listSelectItem";
            this.listSelectItem.Size = new System.Drawing.Size(251, 277);
            this.listSelectItem.TabIndex = 2;
            this.listSelectItem.SelectedIndexChanged += new System.EventHandler(this.listSelectItem_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 643);
            this.Controls.Add(this.listSelectItem);
            this.Controls.Add(this.listFindItem);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listFindItem;
        private System.Windows.Forms.ListBox listSelectItem;
    }
}