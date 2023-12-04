namespace MCSUI.Authority
{
    partial class RoleSetting
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
            this.panel_RoleSetting = new System.Windows.Forms.Panel();
            this.groupBox_RoleSetting_Step4 = new System.Windows.Forms.GroupBox();
            this.button_BindingAuthority = new System.Windows.Forms.Button();
            this.groupBox_RoleSetting_Step3 = new System.Windows.Forms.GroupBox();
            this.checkedListBox_RoleSetting_Step3 = new System.Windows.Forms.CheckedListBox();
            this.groupBox_RoleSetting_Step2 = new System.Windows.Forms.GroupBox();
            this.checkedListBox_RoleSetting_Step2 = new System.Windows.Forms.CheckedListBox();
            this.textBox_RoleSetting_Step2 = new System.Windows.Forms.TextBox();
            this.groupBox_RoleSetting_Step1 = new System.Windows.Forms.GroupBox();
            this.checkedListBox_RoleSetting_Step1 = new System.Windows.Forms.CheckedListBox();
            this.panel_RoleSetting.SuspendLayout();
            this.groupBox_RoleSetting_Step4.SuspendLayout();
            this.groupBox_RoleSetting_Step3.SuspendLayout();
            this.groupBox_RoleSetting_Step2.SuspendLayout();
            this.groupBox_RoleSetting_Step1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_RoleSetting
            // 
            this.panel_RoleSetting.Controls.Add(this.groupBox_RoleSetting_Step4);
            this.panel_RoleSetting.Controls.Add(this.groupBox_RoleSetting_Step3);
            this.panel_RoleSetting.Controls.Add(this.groupBox_RoleSetting_Step2);
            this.panel_RoleSetting.Controls.Add(this.groupBox_RoleSetting_Step1);
            this.panel_RoleSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_RoleSetting.Location = new System.Drawing.Point(0, 0);
            this.panel_RoleSetting.Name = "panel_RoleSetting";
            this.panel_RoleSetting.Size = new System.Drawing.Size(1000, 750);
            this.panel_RoleSetting.TabIndex = 0;
            // 
            // groupBox_RoleSetting_Step4
            // 
            this.groupBox_RoleSetting_Step4.Controls.Add(this.button_BindingAuthority);
            this.groupBox_RoleSetting_Step4.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_RoleSetting_Step4.Enabled = false;
            this.groupBox_RoleSetting_Step4.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_RoleSetting_Step4.Location = new System.Drawing.Point(750, 0);
            this.groupBox_RoleSetting_Step4.Name = "groupBox_RoleSetting_Step4";
            this.groupBox_RoleSetting_Step4.Size = new System.Drawing.Size(250, 750);
            this.groupBox_RoleSetting_Step4.TabIndex = 2;
            this.groupBox_RoleSetting_Step4.TabStop = false;
            this.groupBox_RoleSetting_Step4.Text = "Step 4:";
            // 
            // button_BindingAuthority
            // 
            this.button_BindingAuthority.Location = new System.Drawing.Point(14, 26);
            this.button_BindingAuthority.Name = "button_BindingAuthority";
            this.button_BindingAuthority.Size = new System.Drawing.Size(220, 70);
            this.button_BindingAuthority.TabIndex = 1;
            this.button_BindingAuthority.Text = "CONFIRN";
            this.button_BindingAuthority.UseVisualStyleBackColor = true;
            this.button_BindingAuthority.Click += new System.EventHandler(this.button_BindingAuthority_Click);
            // 
            // groupBox_RoleSetting_Step3
            // 
            this.groupBox_RoleSetting_Step3.Controls.Add(this.checkedListBox_RoleSetting_Step3);
            this.groupBox_RoleSetting_Step3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_RoleSetting_Step3.Enabled = false;
            this.groupBox_RoleSetting_Step3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_RoleSetting_Step3.Location = new System.Drawing.Point(500, 0);
            this.groupBox_RoleSetting_Step3.Name = "groupBox_RoleSetting_Step3";
            this.groupBox_RoleSetting_Step3.Size = new System.Drawing.Size(250, 750);
            this.groupBox_RoleSetting_Step3.TabIndex = 1;
            this.groupBox_RoleSetting_Step3.TabStop = false;
            this.groupBox_RoleSetting_Step3.Text = "Step 3:";
            // 
            // checkedListBox_RoleSetting_Step3
            // 
            this.checkedListBox_RoleSetting_Step3.CheckOnClick = true;
            this.checkedListBox_RoleSetting_Step3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_RoleSetting_Step3.FormattingEnabled = true;
            this.checkedListBox_RoleSetting_Step3.IntegralHeight = false;
            this.checkedListBox_RoleSetting_Step3.Items.AddRange(new object[] {
            "BASIC SETTING",
            "CREATE TRANSFER JOB",
            "EQUIPMENT MANAGEMENT",
            "AUTHORITY MANAGEMENT",
            "PERSONAL SETTING"});
            this.checkedListBox_RoleSetting_Step3.Location = new System.Drawing.Point(3, 21);
            this.checkedListBox_RoleSetting_Step3.Name = "checkedListBox_RoleSetting_Step3";
            this.checkedListBox_RoleSetting_Step3.Size = new System.Drawing.Size(244, 726);
            this.checkedListBox_RoleSetting_Step3.TabIndex = 0;
            this.checkedListBox_RoleSetting_Step3.SelectedIndexChanged += new System.EventHandler(this.checkedListBox_RoleSetting_Step3_SelectedIndexChanged);
            // 
            // groupBox_RoleSetting_Step2
            // 
            this.groupBox_RoleSetting_Step2.Controls.Add(this.checkedListBox_RoleSetting_Step2);
            this.groupBox_RoleSetting_Step2.Controls.Add(this.textBox_RoleSetting_Step2);
            this.groupBox_RoleSetting_Step2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_RoleSetting_Step2.Enabled = false;
            this.groupBox_RoleSetting_Step2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_RoleSetting_Step2.Location = new System.Drawing.Point(250, 0);
            this.groupBox_RoleSetting_Step2.Name = "groupBox_RoleSetting_Step2";
            this.groupBox_RoleSetting_Step2.Size = new System.Drawing.Size(250, 750);
            this.groupBox_RoleSetting_Step2.TabIndex = 0;
            this.groupBox_RoleSetting_Step2.TabStop = false;
            this.groupBox_RoleSetting_Step2.Text = "Step 2:";
            // 
            // checkedListBox_RoleSetting_Step2
            // 
            this.checkedListBox_RoleSetting_Step2.CheckOnClick = true;
            this.checkedListBox_RoleSetting_Step2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_RoleSetting_Step2.FormattingEnabled = true;
            this.checkedListBox_RoleSetting_Step2.IntegralHeight = false;
            this.checkedListBox_RoleSetting_Step2.Location = new System.Drawing.Point(3, 21);
            this.checkedListBox_RoleSetting_Step2.Name = "checkedListBox_RoleSetting_Step2";
            this.checkedListBox_RoleSetting_Step2.Size = new System.Drawing.Size(244, 726);
            this.checkedListBox_RoleSetting_Step2.TabIndex = 1;
            this.checkedListBox_RoleSetting_Step2.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_RoleSetting_Step2_ItemCheck);
            this.checkedListBox_RoleSetting_Step2.SelectedIndexChanged += new System.EventHandler(this.checkedListBox_RoleSetting_Step2_SelectedIndexChanged);
            // 
            // textBox_RoleSetting_Step2
            // 
            this.textBox_RoleSetting_Step2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_RoleSetting_Step2.Location = new System.Drawing.Point(3, 21);
            this.textBox_RoleSetting_Step2.Multiline = true;
            this.textBox_RoleSetting_Step2.Name = "textBox_RoleSetting_Step2";
            this.textBox_RoleSetting_Step2.Size = new System.Drawing.Size(244, 726);
            this.textBox_RoleSetting_Step2.TabIndex = 0;
            this.textBox_RoleSetting_Step2.TextChanged += new System.EventHandler(this.textBox_RoleSetting_Step2_TextChanged);
            // 
            // groupBox_RoleSetting_Step1
            // 
            this.groupBox_RoleSetting_Step1.Controls.Add(this.checkedListBox_RoleSetting_Step1);
            this.groupBox_RoleSetting_Step1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_RoleSetting_Step1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_RoleSetting_Step1.Location = new System.Drawing.Point(0, 0);
            this.groupBox_RoleSetting_Step1.Name = "groupBox_RoleSetting_Step1";
            this.groupBox_RoleSetting_Step1.Size = new System.Drawing.Size(250, 750);
            this.groupBox_RoleSetting_Step1.TabIndex = 0;
            this.groupBox_RoleSetting_Step1.TabStop = false;
            this.groupBox_RoleSetting_Step1.Text = "Step 1:";
            // 
            // checkedListBox_RoleSetting_Step1
            // 
            this.checkedListBox_RoleSetting_Step1.CheckOnClick = true;
            this.checkedListBox_RoleSetting_Step1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_RoleSetting_Step1.FormattingEnabled = true;
            this.checkedListBox_RoleSetting_Step1.IntegralHeight = false;
            this.checkedListBox_RoleSetting_Step1.Items.AddRange(new object[] {
            "ADD ROLE",
            "DELETE ROLE"});
            this.checkedListBox_RoleSetting_Step1.Location = new System.Drawing.Point(3, 21);
            this.checkedListBox_RoleSetting_Step1.Name = "checkedListBox_RoleSetting_Step1";
            this.checkedListBox_RoleSetting_Step1.Size = new System.Drawing.Size(244, 726);
            this.checkedListBox_RoleSetting_Step1.TabIndex = 0;
            this.checkedListBox_RoleSetting_Step1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_RoleSetting_Step1_ItemCheck);
            this.checkedListBox_RoleSetting_Step1.SelectedIndexChanged += new System.EventHandler(this.checkedListBox_RoleSetting_Step1_SelectedIndexChanged);
            // 
            // RoleSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_RoleSetting);
            this.Name = "RoleSetting";
            this.Size = new System.Drawing.Size(1000, 750);
            this.panel_RoleSetting.ResumeLayout(false);
            this.groupBox_RoleSetting_Step4.ResumeLayout(false);
            this.groupBox_RoleSetting_Step3.ResumeLayout(false);
            this.groupBox_RoleSetting_Step2.ResumeLayout(false);
            this.groupBox_RoleSetting_Step2.PerformLayout();
            this.groupBox_RoleSetting_Step1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_RoleSetting;
        private System.Windows.Forms.GroupBox groupBox_RoleSetting_Step4;
        private System.Windows.Forms.GroupBox groupBox_RoleSetting_Step3;
        private System.Windows.Forms.GroupBox groupBox_RoleSetting_Step2;
        private System.Windows.Forms.GroupBox groupBox_RoleSetting_Step1;
        private System.Windows.Forms.CheckedListBox checkedListBox_RoleSetting_Step1;
        private System.Windows.Forms.CheckedListBox checkedListBox_RoleSetting_Step3;
        private System.Windows.Forms.CheckedListBox checkedListBox_RoleSetting_Step2;
        private System.Windows.Forms.TextBox textBox_RoleSetting_Step2;
        private System.Windows.Forms.Button button_BindingAuthority;
    }
}
