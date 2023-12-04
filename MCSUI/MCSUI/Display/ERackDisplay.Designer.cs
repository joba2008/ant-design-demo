namespace MCSUI.Display
{
    partial class ERackDisplay
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
            this.dataGridViewERack = new System.Windows.Forms.DataGridView();
            this.label_ERack_Equipment = new System.Windows.Forms.Label();
            this.button_RecordCurrentBackColor = new System.Windows.Forms.Button();
            this.button_TipColor = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label_TimerStartTime = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewERack)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewERack
            // 
            this.dataGridViewERack.AllowUserToAddRows = false;
            this.dataGridViewERack.AllowUserToResizeColumns = false;
            this.dataGridViewERack.AllowUserToResizeRows = false;
            this.dataGridViewERack.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewERack.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewERack.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewERack.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridViewERack.Location = new System.Drawing.Point(0, 40);
            this.dataGridViewERack.Name = "dataGridViewERack";
            this.dataGridViewERack.ReadOnly = true;
            this.dataGridViewERack.RowHeadersVisible = false;
            this.dataGridViewERack.RowTemplate.Height = 27;
            this.dataGridViewERack.Size = new System.Drawing.Size(575, 138);
            this.dataGridViewERack.TabIndex = 2;
            this.dataGridViewERack.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewERack_CellFormatting);
            this.dataGridViewERack.DoubleClick += new System.EventHandler(this.dataGridViewERack_DoubleClick);
            // 
            // label_ERack_Equipment
            // 
            this.label_ERack_Equipment.BackColor = System.Drawing.SystemColors.Window;
            this.label_ERack_Equipment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ERack_Equipment.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_ERack_Equipment.Font = new System.Drawing.Font("Consolas", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_ERack_Equipment.Location = new System.Drawing.Point(0, 0);
            this.label_ERack_Equipment.Name = "label_ERack_Equipment";
            this.label_ERack_Equipment.Size = new System.Drawing.Size(575, 40);
            this.label_ERack_Equipment.TabIndex = 3;
            this.label_ERack_Equipment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_RecordCurrentBackColor
            // 
            this.button_RecordCurrentBackColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_RecordCurrentBackColor.Location = new System.Drawing.Point(51, 122);
            this.button_RecordCurrentBackColor.Name = "button_RecordCurrentBackColor";
            this.button_RecordCurrentBackColor.Size = new System.Drawing.Size(25, 25);
            this.button_RecordCurrentBackColor.TabIndex = 4;
            this.button_RecordCurrentBackColor.UseVisualStyleBackColor = true;
            this.button_RecordCurrentBackColor.Visible = false;
            // 
            // button_TipColor
            // 
            this.button_TipColor.BackColor = System.Drawing.Color.Yellow;
            this.button_TipColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_TipColor.Location = new System.Drawing.Point(82, 122);
            this.button_TipColor.Name = "button_TipColor";
            this.button_TipColor.Size = new System.Drawing.Size(25, 25);
            this.button_TipColor.TabIndex = 5;
            this.button_TipColor.UseVisualStyleBackColor = false;
            this.button_TipColor.Visible = false;
            // 
            // label_TimerStartTime
            // 
            this.label_TimerStartTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_TimerStartTime.Location = new System.Drawing.Point(113, 122);
            this.label_TimerStartTime.Name = "label_TimerStartTime";
            this.label_TimerStartTime.Size = new System.Drawing.Size(100, 25);
            this.label_TimerStartTime.TabIndex = 6;
            this.label_TimerStartTime.UseVisualStyleBackColor = true;
            this.label_TimerStartTime.Visible = false;
            // 
            // ERackDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.label_TimerStartTime);
            this.Controls.Add(this.button_TipColor);
            this.Controls.Add(this.button_RecordCurrentBackColor);
            this.Controls.Add(this.label_ERack_Equipment);
            this.Controls.Add(this.dataGridViewERack);
            this.Name = "ERackDisplay";
            this.Size = new System.Drawing.Size(575, 178);
            this.Load += new System.EventHandler(this.ERackDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewERack)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewERack;
        private System.Windows.Forms.Label label_ERack_Equipment;
        private System.Windows.Forms.Button button_RecordCurrentBackColor;
        private System.Windows.Forms.Button button_TipColor;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button label_TimerStartTime;
    }
}
