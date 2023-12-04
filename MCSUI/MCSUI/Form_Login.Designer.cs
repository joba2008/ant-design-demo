namespace MCSUI
{
    partial class Form_Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Login));
            this.panel_Login = new System.Windows.Forms.Panel();
            this.button_Login_OK = new System.Windows.Forms.Button();
            this.button_Login_Cancel = new System.Windows.Forms.Button();
            this.textBox_Login_PassWord = new System.Windows.Forms.TextBox();
            this.textBox_Login_UserID = new System.Windows.Forms.TextBox();
            this.label_Login_PassWord = new System.Windows.Forms.Label();
            this.label_Login_UserID = new System.Windows.Forms.Label();
            this.panel_Login.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Login
            // 
            this.panel_Login.Controls.Add(this.button_Login_OK);
            this.panel_Login.Controls.Add(this.button_Login_Cancel);
            this.panel_Login.Controls.Add(this.textBox_Login_PassWord);
            this.panel_Login.Controls.Add(this.textBox_Login_UserID);
            this.panel_Login.Controls.Add(this.label_Login_PassWord);
            this.panel_Login.Controls.Add(this.label_Login_UserID);
            this.panel_Login.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Login.Location = new System.Drawing.Point(0, 0);
            this.panel_Login.Name = "panel_Login";
            this.panel_Login.Size = new System.Drawing.Size(432, 203);
            this.panel_Login.TabIndex = 0;
            // 
            // button_Login_OK
            // 
            this.button_Login_OK.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Login_OK.Location = new System.Drawing.Point(156, 131);
            this.button_Login_OK.Name = "button_Login_OK";
            this.button_Login_OK.Size = new System.Drawing.Size(241, 50);
            this.button_Login_OK.TabIndex = 5;
            this.button_Login_OK.Text = "CONFIRN";
            this.button_Login_OK.UseVisualStyleBackColor = true;
            this.button_Login_OK.Click += new System.EventHandler(this.button_Login_OK_Click);
            // 
            // button_Login_Cancel
            // 
            this.button_Login_Cancel.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Login_Cancel.Location = new System.Drawing.Point(34, 131);
            this.button_Login_Cancel.Name = "button_Login_Cancel";
            this.button_Login_Cancel.Size = new System.Drawing.Size(111, 50);
            this.button_Login_Cancel.TabIndex = 4;
            this.button_Login_Cancel.Text = "CANCEL";
            this.button_Login_Cancel.UseVisualStyleBackColor = true;
            this.button_Login_Cancel.Click += new System.EventHandler(this.button_Login_Cancel_Click);
            // 
            // textBox_Login_PassWord
            // 
            this.textBox_Login_PassWord.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Login_PassWord.Location = new System.Drawing.Point(156, 86);
            this.textBox_Login_PassWord.Name = "textBox_Login_PassWord";
            this.textBox_Login_PassWord.PasswordChar = '*';
            this.textBox_Login_PassWord.Size = new System.Drawing.Size(241, 29);
            this.textBox_Login_PassWord.TabIndex = 3;
            // 
            // textBox_Login_UserID
            // 
            this.textBox_Login_UserID.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Login_UserID.Location = new System.Drawing.Point(156, 36);
            this.textBox_Login_UserID.Name = "textBox_Login_UserID";
            this.textBox_Login_UserID.Size = new System.Drawing.Size(241, 29);
            this.textBox_Login_UserID.TabIndex = 2;
            // 
            // label_Login_PassWord
            // 
            this.label_Login_PassWord.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Login_PassWord.Location = new System.Drawing.Point(30, 87);
            this.label_Login_PassWord.Name = "label_Login_PassWord";
            this.label_Login_PassWord.Size = new System.Drawing.Size(120, 25);
            this.label_Login_PassWord.TabIndex = 1;
            this.label_Login_PassWord.Text = "PASSWORD";
            this.label_Login_PassWord.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Login_UserID
            // 
            this.label_Login_UserID.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Login_UserID.Location = new System.Drawing.Point(30, 37);
            this.label_Login_UserID.Name = "label_Login_UserID";
            this.label_Login_UserID.Size = new System.Drawing.Size(120, 25);
            this.label_Login_UserID.TabIndex = 0;
            this.label_Login_UserID.Text = "USER ID";
            this.label_Login_UserID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 203);
            this.Controls.Add(this.panel_Login);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login # MCS Client";
            this.panel_Login.ResumeLayout(false);
            this.panel_Login.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_Login;
        private System.Windows.Forms.Button button_Login_OK;
        private System.Windows.Forms.Button button_Login_Cancel;
        private System.Windows.Forms.TextBox textBox_Login_PassWord;
        private System.Windows.Forms.TextBox textBox_Login_UserID;
        private System.Windows.Forms.Label label_Login_PassWord;
        private System.Windows.Forms.Label label_Login_UserID;
    }
}