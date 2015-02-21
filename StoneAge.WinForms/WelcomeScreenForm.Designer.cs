namespace StoneAge.WinForms
{
    partial class WelcomeScreenForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeScreenForm));
            this.labelWelcomeToStoneAge = new System.Windows.Forms.Label();
            this.labelCreatedBy = new System.Windows.Forms.Label();
            this.labelAuthor = new System.Windows.Forms.Label();
            this.labelDisclaimer = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelWelcomeToStoneAge
            // 
            this.labelWelcomeToStoneAge.AutoSize = true;
            this.labelWelcomeToStoneAge.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWelcomeToStoneAge.Location = new System.Drawing.Point(75, 105);
            this.labelWelcomeToStoneAge.Name = "labelWelcomeToStoneAge";
            this.labelWelcomeToStoneAge.Size = new System.Drawing.Size(270, 23);
            this.labelWelcomeToStoneAge.TabIndex = 0;
            this.labelWelcomeToStoneAge.Text = "Welcome to Stone Age";
            this.labelWelcomeToStoneAge.Click += new System.EventHandler(this.WelcomeScreenForm_Click);
            // 
            // labelCreatedBy
            // 
            this.labelCreatedBy.AutoSize = true;
            this.labelCreatedBy.Location = new System.Drawing.Point(318, 240);
            this.labelCreatedBy.Name = "labelCreatedBy";
            this.labelCreatedBy.Size = new System.Drawing.Size(62, 13);
            this.labelCreatedBy.TabIndex = 1;
            this.labelCreatedBy.Text = "Created By:";
            this.labelCreatedBy.Click += new System.EventHandler(this.WelcomeScreenForm_Click);
            // 
            // labelAuthor
            // 
            this.labelAuthor.AutoSize = true;
            this.labelAuthor.Location = new System.Drawing.Point(318, 253);
            this.labelAuthor.Name = "labelAuthor";
            this.labelAuthor.Size = new System.Drawing.Size(107, 13);
            this.labelAuthor.TabIndex = 2;
            this.labelAuthor.Text = "Benjamin M Heimann";
            this.labelAuthor.Click += new System.EventHandler(this.WelcomeScreenForm_Click);
            // 
            // labelDisclaimer
            // 
            this.labelDisclaimer.AutoSize = true;
            this.labelDisclaimer.Location = new System.Drawing.Point(12, 253);
            this.labelDisclaimer.Name = "labelDisclaimer";
            this.labelDisclaimer.Size = new System.Drawing.Size(227, 13);
            this.labelDisclaimer.TabIndex = 3;
            this.labelDisclaimer.Text = "(This is no way for distribution, just my own fun)";
            this.labelDisclaimer.Click += new System.EventHandler(this.WelcomeScreenForm_Click);
            // 
            // WelcomeScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 275);
            this.Controls.Add(this.labelDisclaimer);
            this.Controls.Add(this.labelAuthor);
            this.Controls.Add(this.labelCreatedBy);
            this.Controls.Add(this.labelWelcomeToStoneAge);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WelcomeScreenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Welcome to Stone Age";
            this.Click += new System.EventHandler(this.WelcomeScreenForm_Click);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WelcomeScreenForm_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelWelcomeToStoneAge;
        private System.Windows.Forms.Label labelCreatedBy;
        private System.Windows.Forms.Label labelAuthor;
        private System.Windows.Forms.Label labelDisclaimer;
    }
}
