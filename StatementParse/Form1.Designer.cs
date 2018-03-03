namespace StatementParse
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
            this.btn_savings1 = new System.Windows.Forms.Button();
            this.pb_import = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btn_savings1
            // 
            this.btn_savings1.Location = new System.Drawing.Point(13, 13);
            this.btn_savings1.Name = "btn_savings1";
            this.btn_savings1.Size = new System.Drawing.Size(114, 38);
            this.btn_savings1.TabIndex = 0;
            this.btn_savings1.Text = "Savings 1";
            this.btn_savings1.UseVisualStyleBackColor = true;
            this.btn_savings1.Click += new System.EventHandler(this.btn_savings1_Click);
            // 
            // pb_import
            // 
            this.pb_import.Location = new System.Drawing.Point(12, 226);
            this.pb_import.Name = "pb_import";
            this.pb_import.Size = new System.Drawing.Size(260, 23);
            this.pb_import.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.pb_import);
            this.Controls.Add(this.btn_savings1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_savings1;
        private System.Windows.Forms.ProgressBar pb_import;
    }
}

