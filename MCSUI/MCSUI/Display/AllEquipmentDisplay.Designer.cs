namespace MCSUI.Display
{
    partial class AllEquipmentDisplay
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewAllEQPStatus = new System.Windows.Forms.DataGridView();
            this.Column_EQPType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_EQPSubtype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_EQPName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_EQPCapacity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_CountofFOUP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_PercentageofUsage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAllEQPStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewAllEQPStatus
            // 
            this.dataGridViewAllEQPStatus.AllowUserToAddRows = false;
            this.dataGridViewAllEQPStatus.AllowUserToDeleteRows = false;
            this.dataGridViewAllEQPStatus.AllowUserToResizeColumns = false;
            this.dataGridViewAllEQPStatus.AllowUserToResizeRows = false;
            this.dataGridViewAllEQPStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridViewAllEQPStatus.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAllEQPStatus.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewAllEQPStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_EQPType,
            this.Column_EQPSubtype,
            this.Column_EQPName,
            this.Column_EQPCapacity,
            this.Column_CountofFOUP,
            this.Column_PercentageofUsage});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewAllEQPStatus.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewAllEQPStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAllEQPStatus.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewAllEQPStatus.Name = "dataGridViewAllEQPStatus";
            this.dataGridViewAllEQPStatus.ReadOnly = true;
            this.dataGridViewAllEQPStatus.RowHeadersVisible = false;
            this.dataGridViewAllEQPStatus.RowTemplate.Height = 27;
            this.dataGridViewAllEQPStatus.Size = new System.Drawing.Size(1700, 800);
            this.dataGridViewAllEQPStatus.TabIndex = 0;
            // 
            // Column_EQPType
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.Column_EQPType.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column_EQPType.HeaderText = "Equipment Type";
            this.Column_EQPType.Name = "Column_EQPType";
            this.Column_EQPType.ReadOnly = true;
            this.Column_EQPType.Width = 131;
            // 
            // Column_EQPSubtype
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.Column_EQPSubtype.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column_EQPSubtype.HeaderText = "Equipment Subtype";
            this.Column_EQPSubtype.Name = "Column_EQPSubtype";
            this.Column_EQPSubtype.ReadOnly = true;
            this.Column_EQPSubtype.Width = 148;
            // 
            // Column_EQPName
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.Column_EQPName.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column_EQPName.HeaderText = "Equipment Name";
            this.Column_EQPName.Name = "Column_EQPName";
            this.Column_EQPName.ReadOnly = true;
            this.Column_EQPName.Width = 135;
            // 
            // Column_EQPCapacity
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            this.Column_EQPCapacity.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column_EQPCapacity.HeaderText = "Equipment Capacity";
            this.Column_EQPCapacity.Name = "Column_EQPCapacity";
            this.Column_EQPCapacity.ReadOnly = true;
            this.Column_EQPCapacity.Width = 151;
            // 
            // Column_CountofFOUP
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            this.Column_CountofFOUP.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column_CountofFOUP.HeaderText = "The Count of FOUP in Equipment";
            this.Column_CountofFOUP.Name = "Column_CountofFOUP";
            this.Column_CountofFOUP.ReadOnly = true;
            this.Column_CountofFOUP.Width = 233;
            // 
            // Column_PercentageofUsage
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            this.Column_PercentageofUsage.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column_PercentageofUsage.HeaderText = "Utilization";
            this.Column_PercentageofUsage.Name = "Column_PercentageofUsage";
            this.Column_PercentageofUsage.ReadOnly = true;
            this.Column_PercentageofUsage.Width = 96;
            // 
            // AllEquipmentDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridViewAllEQPStatus);
            this.Name = "AllEquipmentDisplay";
            this.Size = new System.Drawing.Size(1700, 800);
            this.Load += new System.EventHandler(this.AllEquipmentDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAllEQPStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewAllEQPStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_EQPType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_EQPSubtype;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_EQPName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_EQPCapacity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_CountofFOUP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PercentageofUsage;
    }
}
