namespace MCSUI.Authority
{
    partial class PersonalSetting
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label_PersonalSetting_UserID = new System.Windows.Forms.Label();
            this.label_PersonalSetting_Password = new System.Windows.Forms.Label();
            this.label_PersonalSetting_Email = new System.Windows.Forms.Label();
            this.textBox_PersonalSetting_Password = new System.Windows.Forms.TextBox();
            this.textBox_PersonalSetting_Email = new System.Windows.Forms.TextBox();
            this.textBox_PersonalSetting_UserID = new System.Windows.Forms.TextBox();
            this.button_PersonalSetting_Confirn = new System.Windows.Forms.Button();
            this.textBox_PersonalSetting_Password_DoubleConfirm = new System.Windows.Forms.TextBox();
            this.label_PersonalSetting_Password_DoubleConfirm = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_PersonalSetting_UserID
            // 
            this.label_PersonalSetting_UserID.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label_PersonalSetting_UserID.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PersonalSetting_UserID.Location = new System.Drawing.Point(8, 12);
            this.label_PersonalSetting_UserID.Name = "label_PersonalSetting_UserID";
            this.label_PersonalSetting_UserID.Size = new System.Drawing.Size(200, 30);
            this.label_PersonalSetting_UserID.TabIndex = 0;
            this.label_PersonalSetting_UserID.Text = "USER ID";
            this.label_PersonalSetting_UserID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_PersonalSetting_Password
            // 
            this.label_PersonalSetting_Password.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label_PersonalSetting_Password.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PersonalSetting_Password.Location = new System.Drawing.Point(8, 70);
            this.label_PersonalSetting_Password.Name = "label_PersonalSetting_Password";
            this.label_PersonalSetting_Password.Size = new System.Drawing.Size(200, 30);
            this.label_PersonalSetting_Password.TabIndex = 2;
            this.label_PersonalSetting_Password.Text = "PASSWORD";
            this.label_PersonalSetting_Password.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_PersonalSetting_Email
            // 
            this.label_PersonalSetting_Email.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label_PersonalSetting_Email.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PersonalSetting_Email.Location = new System.Drawing.Point(8, 188);
            this.label_PersonalSetting_Email.Name = "label_PersonalSetting_Email";
            this.label_PersonalSetting_Email.Size = new System.Drawing.Size(200, 30);
            this.label_PersonalSetting_Email.TabIndex = 4;
            this.label_PersonalSetting_Email.Text = "EMAIL";
            this.label_PersonalSetting_Email.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_PersonalSetting_Password
            // 
            this.textBox_PersonalSetting_Password.Location = new System.Drawing.Point(217, 73);
            this.textBox_PersonalSetting_Password.Name = "textBox_PersonalSetting_Password";
            this.textBox_PersonalSetting_Password.PasswordChar = '*';
            this.textBox_PersonalSetting_Password.Size = new System.Drawing.Size(680, 25);
            this.textBox_PersonalSetting_Password.TabIndex = 6;
            // 
            // textBox_PersonalSetting_Email
            // 
            this.textBox_PersonalSetting_Email.Location = new System.Drawing.Point(217, 191);
            this.textBox_PersonalSetting_Email.Name = "textBox_PersonalSetting_Email";
            this.textBox_PersonalSetting_Email.Size = new System.Drawing.Size(680, 25);
            this.textBox_PersonalSetting_Email.TabIndex = 7;
            // 
            // textBox_PersonalSetting_UserID
            // 
            this.textBox_PersonalSetting_UserID.Location = new System.Drawing.Point(217, 14);
            this.textBox_PersonalSetting_UserID.Name = "textBox_PersonalSetting_UserID";
            this.textBox_PersonalSetting_UserID.ReadOnly = true;
            this.textBox_PersonalSetting_UserID.Size = new System.Drawing.Size(680, 25);
            this.textBox_PersonalSetting_UserID.TabIndex = 8;
            // 
            // button_PersonalSetting_Confirn
            // 
            this.button_PersonalSetting_Confirn.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_PersonalSetting_Confirn.Location = new System.Drawing.Point(677, 246);
            this.button_PersonalSetting_Confirn.Name = "button_PersonalSetting_Confirn";
            this.button_PersonalSetting_Confirn.Size = new System.Drawing.Size(220, 70);
            this.button_PersonalSetting_Confirn.TabIndex = 9;
            this.button_PersonalSetting_Confirn.Text = "RESET";
            this.button_PersonalSetting_Confirn.UseVisualStyleBackColor = true;
            this.button_PersonalSetting_Confirn.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox_PersonalSetting_Password_DoubleConfirm
            // 
            this.textBox_PersonalSetting_Password_DoubleConfirm.Location = new System.Drawing.Point(217, 132);
            this.textBox_PersonalSetting_Password_DoubleConfirm.Name = "textBox_PersonalSetting_Password_DoubleConfirm";
            this.textBox_PersonalSetting_Password_DoubleConfirm.PasswordChar = '*';
            this.textBox_PersonalSetting_Password_DoubleConfirm.Size = new System.Drawing.Size(680, 25);
            this.textBox_PersonalSetting_Password_DoubleConfirm.TabIndex = 11;
            // 
            // label_PersonalSetting_Password_DoubleConfirm
            // 
            this.label_PersonalSetting_Password_DoubleConfirm.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label_PersonalSetting_Password_DoubleConfirm.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PersonalSetting_Password_DoubleConfirm.Location = new System.Drawing.Point(8, 129);
            this.label_PersonalSetting_Password_DoubleConfirm.Name = "label_PersonalSetting_Password_DoubleConfirm";
            this.label_PersonalSetting_Password_DoubleConfirm.Size = new System.Drawing.Size(200, 30);
            this.label_PersonalSetting_Password_DoubleConfirm.TabIndex = 10;
            this.label_PersonalSetting_Password_DoubleConfirm.Text = "PASSWORD (Confirm Again)";
            this.label_PersonalSetting_Password_DoubleConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PersonalSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_PersonalSetting_Password_DoubleConfirm);
            this.Controls.Add(this.label_PersonalSetting_Password_DoubleConfirm);
            this.Controls.Add(this.button_PersonalSetting_Confirn);
            this.Controls.Add(this.textBox_PersonalSetting_UserID);
            this.Controls.Add(this.textBox_PersonalSetting_Email);
            this.Controls.Add(this.textBox_PersonalSetting_Password);
            this.Controls.Add(this.label_PersonalSetting_Email);
            this.Controls.Add(this.label_PersonalSetting_Password);
            this.Controls.Add(this.label_PersonalSetting_UserID);
            this.Name = "PersonalSetting";
            this.Size = new System.Drawing.Size(1000, 750);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_PersonalSetting_UserID;
        private System.Windows.Forms.Label label_PersonalSetting_Password;
        private System.Windows.Forms.Label label_PersonalSetting_Email;
        private System.Windows.Forms.TextBox textBox_PersonalSetting_Password;
        private System.Windows.Forms.TextBox textBox_PersonalSetting_Email;
        private System.Windows.Forms.TextBox textBox_PersonalSetting_UserID;
        private System.Windows.Forms.Button button_PersonalSetting_Confirn;
        private System.Windows.Forms.TextBox textBox_PersonalSetting_Password_DoubleConfirm;
        private System.Windows.Forms.Label label_PersonalSetting_Password_DoubleConfirm;
    }
}
