namespace MCSUI
{
    partial class Form_BatchTransferOut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_BatchTransferOut));
            this.panel_batchTransferOut_Info = new System.Windows.Forms.Panel();
            this.panel_BatchTransferOut_xxx = new System.Windows.Forms.Panel();
            this.button_BatchTransferOut_Search = new System.Windows.Forms.Button();
            this.textBox_BatchTransferOut_FOUPID = new System.Windows.Forms.TextBox();
            this.panel_BatchTransferOut_BTN = new System.Windows.Forms.Panel();
            this.button_BatchTransferOut = new System.Windows.Forms.Button();
            this.panel_BatchTransferOut_EQPID = new System.Windows.Forms.Panel();
            this.label_BatchTransferOut_EQPID = new System.Windows.Forms.Label();
            this.panel_batchTransferOut_DataGridView = new System.Windows.Forms.Panel();
            this.label_BatchTransferOut_UserName = new System.Windows.Forms.Label();
            this.dataGridView_batchTransferOut = new System.Windows.Forms.DataGridView();
            this.panel_BatchTransferOut_Tip = new System.Windows.Forms.Panel();
            this.label_BatchTransferOut_Tip = new System.Windows.Forms.Label();
            this.panel_batchTransferOut_Info.SuspendLayout();
            this.panel_BatchTransferOut_xxx.SuspendLayout();
            this.panel_BatchTransferOut_BTN.SuspendLayout();
            this.panel_BatchTransferOut_EQPID.SuspendLayout();
            this.panel_batchTransferOut_DataGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_batchTransferOut)).BeginInit();
            this.panel_BatchTransferOut_Tip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_batchTransferOut_Info
            // 
            this.panel_batchTransferOut_Info.Controls.Add(this.panel_BatchTransferOut_xxx);
            this.panel_batchTransferOut_Info.Controls.Add(this.panel_BatchTransferOut_BTN);
            this.panel_batchTransferOut_Info.Controls.Add(this.panel_BatchTransferOut_EQPID);
            this.panel_batchTransferOut_Info.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_batchTransferOut_Info.Location = new System.Drawing.Point(0, 0);
            this.panel_batchTransferOut_Info.Name = "panel_batchTransferOut_Info";
            this.panel_batchTransferOut_Info.Size = new System.Drawing.Size(582, 50);
            this.panel_batchTransferOut_Info.TabIndex = 0;
            // 
            // panel_BatchTransferOut_xxx
            // 
            this.panel_BatchTransferOut_xxx.Controls.Add(this.button_BatchTransferOut_Search);
            this.panel_BatchTransferOut_xxx.Controls.Add(this.textBox_BatchTransferOut_FOUPID);
            this.panel_BatchTransferOut_xxx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_BatchTransferOut_xxx.Location = new System.Drawing.Point(150, 0);
            this.panel_BatchTransferOut_xxx.Name = "panel_BatchTransferOut_xxx";
            this.panel_BatchTransferOut_xxx.Size = new System.Drawing.Size(282, 50);
            this.panel_BatchTransferOut_xxx.TabIndex = 2;
            // 
            // button_BatchTransferOut_Search
            // 
            this.button_BatchTransferOut_Search.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_BatchTransferOut_Search.Location = new System.Drawing.Point(166, 10);
            this.button_BatchTransferOut_Search.Name = "button_BatchTransferOut_Search";
            this.button_BatchTransferOut_Search.Size = new System.Drawing.Size(110, 29);
            this.button_BatchTransferOut_Search.TabIndex = 1;
            this.button_BatchTransferOut_Search.Text = "Search";
            this.button_BatchTransferOut_Search.UseVisualStyleBackColor = true;
            this.button_BatchTransferOut_Search.Click += new System.EventHandler(this.button_BatchTransferOut_Search_Click);
            // 
            // textBox_BatchTransferOut_FOUPID
            // 
            this.textBox_BatchTransferOut_FOUPID.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_BatchTransferOut_FOUPID.Location = new System.Drawing.Point(6, 10);
            this.textBox_BatchTransferOut_FOUPID.Name = "textBox_BatchTransferOut_FOUPID";
            this.textBox_BatchTransferOut_FOUPID.Size = new System.Drawing.Size(154, 29);
            this.textBox_BatchTransferOut_FOUPID.TabIndex = 0;
            this.textBox_BatchTransferOut_FOUPID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_BatchTransferOut_FOUPID_KeyDown);
            // 
            // panel_BatchTransferOut_BTN
            // 
            this.panel_BatchTransferOut_BTN.Controls.Add(this.button_BatchTransferOut);
            this.panel_BatchTransferOut_BTN.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_BatchTransferOut_BTN.Location = new System.Drawing.Point(432, 0);
            this.panel_BatchTransferOut_BTN.Name = "panel_BatchTransferOut_BTN";
            this.panel_BatchTransferOut_BTN.Size = new System.Drawing.Size(150, 50);
            this.panel_BatchTransferOut_BTN.TabIndex = 1;
            // 
            // button_BatchTransferOut
            // 
            this.button_BatchTransferOut.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_BatchTransferOut.Location = new System.Drawing.Point(8, 10);
            this.button_BatchTransferOut.Name = "button_BatchTransferOut";
            this.button_BatchTransferOut.Size = new System.Drawing.Size(130, 29);
            this.button_BatchTransferOut.TabIndex = 0;
            this.button_BatchTransferOut.Text = "Transfer Out";
            this.button_BatchTransferOut.UseVisualStyleBackColor = true;
            this.button_BatchTransferOut.Click += new System.EventHandler(this.button_BatchTransferOut_Click);
            // 
            // panel_BatchTransferOut_EQPID
            // 
            this.panel_BatchTransferOut_EQPID.Controls.Add(this.label_BatchTransferOut_EQPID);
            this.panel_BatchTransferOut_EQPID.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_BatchTransferOut_EQPID.Location = new System.Drawing.Point(0, 0);
            this.panel_BatchTransferOut_EQPID.Name = "panel_BatchTransferOut_EQPID";
            this.panel_BatchTransferOut_EQPID.Size = new System.Drawing.Size(150, 50);
            this.panel_BatchTransferOut_EQPID.TabIndex = 0;
            // 
            // label_BatchTransferOut_EQPID
            // 
            this.label_BatchTransferOut_EQPID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_BatchTransferOut_EQPID.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_BatchTransferOut_EQPID.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label_BatchTransferOut_EQPID.Location = new System.Drawing.Point(0, 0);
            this.label_BatchTransferOut_EQPID.Name = "label_BatchTransferOut_EQPID";
            this.label_BatchTransferOut_EQPID.Size = new System.Drawing.Size(150, 50);
            this.label_BatchTransferOut_EQPID.TabIndex = 0;
            this.label_BatchTransferOut_EQPID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel_batchTransferOut_DataGridView
            // 
            this.panel_batchTransferOut_DataGridView.Controls.Add(this.label_BatchTransferOut_UserName);
            this.panel_batchTransferOut_DataGridView.Controls.Add(this.dataGridView_batchTransferOut);
            this.panel_batchTransferOut_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_batchTransferOut_DataGridView.Location = new System.Drawing.Point(0, 50);
            this.panel_batchTransferOut_DataGridView.Name = "panel_batchTransferOut_DataGridView";
            this.panel_batchTransferOut_DataGridView.Size = new System.Drawing.Size(582, 703);
            this.panel_batchTransferOut_DataGridView.TabIndex = 1;
            // 
            // label_BatchTransferOut_UserName
            // 
            this.label_BatchTransferOut_UserName.AutoSize = true;
            this.label_BatchTransferOut_UserName.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_BatchTransferOut_UserName.Location = new System.Drawing.Point(498, 676);
            this.label_BatchTransferOut_UserName.Name = "label_BatchTransferOut_UserName";
            this.label_BatchTransferOut_UserName.Size = new System.Drawing.Size(72, 18);
            this.label_BatchTransferOut_UserName.TabIndex = 1;
            this.label_BatchTransferOut_UserName.Text = "username";
            this.label_BatchTransferOut_UserName.Visible = false;
            // 
            // dataGridView_batchTransferOut
            // 
            this.dataGridView_batchTransferOut.AllowUserToAddRows = false;
            this.dataGridView_batchTransferOut.AllowUserToDeleteRows = false;
            this.dataGridView_batchTransferOut.AllowUserToResizeColumns = false;
            this.dataGridView_batchTransferOut.AllowUserToResizeRows = false;
            this.dataGridView_batchTransferOut.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView_batchTransferOut.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_batchTransferOut.ColumnHeadersVisible = false;
            this.dataGridView_batchTransferOut.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView_batchTransferOut.Location = new System.Drawing.Point(0, 53);
            this.dataGridView_batchTransferOut.Name = "dataGridView_batchTransferOut";
            this.dataGridView_batchTransferOut.ReadOnly = true;
            this.dataGridView_batchTransferOut.RowHeadersVisible = false;
            this.dataGridView_batchTransferOut.RowTemplate.Height = 27;
            this.dataGridView_batchTransferOut.Size = new System.Drawing.Size(582, 650);
            this.dataGridView_batchTransferOut.TabIndex = 0;
            this.dataGridView_batchTransferOut.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_batchTransferOut_CellClick);
            // 
            // panel_BatchTransferOut_Tip
            // 
            this.panel_BatchTransferOut_Tip.Controls.Add(this.label_BatchTransferOut_Tip);
            this.panel_BatchTransferOut_Tip.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_BatchTransferOut_Tip.Location = new System.Drawing.Point(0, 50);
            this.panel_BatchTransferOut_Tip.Name = "panel_BatchTransferOut_Tip";
            this.panel_BatchTransferOut_Tip.Size = new System.Drawing.Size(582, 50);
            this.panel_BatchTransferOut_Tip.TabIndex = 2;
            // 
            // label_BatchTransferOut_Tip
            // 
            this.label_BatchTransferOut_Tip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_BatchTransferOut_Tip.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_BatchTransferOut_Tip.ForeColor = System.Drawing.Color.Blue;
            this.label_BatchTransferOut_Tip.Location = new System.Drawing.Point(0, 0);
            this.label_BatchTransferOut_Tip.Name = "label_BatchTransferOut_Tip";
            this.label_BatchTransferOut_Tip.Size = new System.Drawing.Size(582, 50);
            this.label_BatchTransferOut_Tip.TabIndex = 1;
            this.label_BatchTransferOut_Tip.Text = "  TIP: You Can Click Multiple Grids To Choose Multiple Items";
            this.label_BatchTransferOut_Tip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Form_BatchTransferOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 753);
            this.Controls.Add(this.panel_BatchTransferOut_Tip);
            this.Controls.Add(this.panel_batchTransferOut_DataGridView);
            this.Controls.Add(this.panel_batchTransferOut_Info);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_BatchTransferOut";
            this.Text = "TransferOut";
            this.panel_batchTransferOut_Info.ResumeLayout(false);
            this.panel_BatchTransferOut_xxx.ResumeLayout(false);
            this.panel_BatchTransferOut_xxx.PerformLayout();
            this.panel_BatchTransferOut_BTN.ResumeLayout(false);
            this.panel_BatchTransferOut_EQPID.ResumeLayout(false);
            this.panel_batchTransferOut_DataGridView.ResumeLayout(false);
            this.panel_batchTransferOut_DataGridView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_batchTransferOut)).EndInit();
            this.panel_BatchTransferOut_Tip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_batchTransferOut_Info;
        private System.Windows.Forms.Panel panel_batchTransferOut_DataGridView;
        private System.Windows.Forms.DataGridView dataGridView_batchTransferOut;
        private System.Windows.Forms.Panel panel_BatchTransferOut_EQPID;
        private System.Windows.Forms.Label label_BatchTransferOut_EQPID;
        private System.Windows.Forms.Panel panel_BatchTransferOut_BTN;
        private System.Windows.Forms.Button button_BatchTransferOut;
        private System.Windows.Forms.Panel panel_BatchTransferOut_xxx;
        private System.Windows.Forms.Button button_BatchTransferOut_Search;
        private System.Windows.Forms.TextBox textBox_BatchTransferOut_FOUPID;
        private System.Windows.Forms.Label label_BatchTransferOut_UserName;
        private System.Windows.Forms.Panel panel_BatchTransferOut_Tip;
        private System.Windows.Forms.Label label_BatchTransferOut_Tip;
    }
}