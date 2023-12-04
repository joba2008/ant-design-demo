namespace MCSUI.Display
{
    partial class StockerDisplay
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewStocker = new System.Windows.Forms.DataGridView();
            this.button_RecordCurrentBackColor = new System.Windows.Forms.Button();
            this.timer_ChangeBackColor = new System.Windows.Forms.Timer(this.components);
            this.button_TipColor = new System.Windows.Forms.Button();
            this.label_TimerStartTime = new System.Windows.Forms.Label();
            this.button_TransferTimerColor = new System.Windows.Forms.Button();
            this.label_TransferTimerStartTime = new System.Windows.Forms.Label();
            this.button_RecordCurrentBackColorTransfer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStocker)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewStocker
            // 
            this.dataGridViewStocker.AllowUserToAddRows = false;
            this.dataGridViewStocker.AllowUserToResizeColumns = false;
            this.dataGridViewStocker.AllowUserToResizeRows = false;
            this.dataGridViewStocker.ColumnHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewStocker.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewStocker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewStocker.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewStocker.Name = "dataGridViewStocker";
            this.dataGridViewStocker.ReadOnly = true;
            this.dataGridViewStocker.RowHeadersVisible = false;
            this.dataGridViewStocker.RowHeadersWidth = 20;
            this.dataGridViewStocker.RowTemplate.Height = 27;
            this.dataGridViewStocker.Size = new System.Drawing.Size(1700, 800);
            this.dataGridViewStocker.TabIndex = 1;
            this.dataGridViewStocker.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewStocker_CellClick);
            this.dataGridViewStocker.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewStocker_CellFormatting);
            this.dataGridViewStocker.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewStocker_CellMouseDoubleClick);
            this.dataGridViewStocker.Paint += new System.Windows.Forms.PaintEventHandler(this.dataGridViewStocker_Paint);
            // 
            // button_RecordCurrentBackColor
            // 
            this.button_RecordCurrentBackColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_RecordCurrentBackColor.Location = new System.Drawing.Point(16, 754);
            this.button_RecordCurrentBackColor.Name = "button_RecordCurrentBackColor";
            this.button_RecordCurrentBackColor.Size = new System.Drawing.Size(25, 25);
            this.button_RecordCurrentBackColor.TabIndex = 2;
            this.button_RecordCurrentBackColor.UseVisualStyleBackColor = true;
            this.button_RecordCurrentBackColor.Visible = false;
            // 
            // button_TipColor
            // 
            this.button_TipColor.BackColor = System.Drawing.Color.Yellow;
            this.button_TipColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_TipColor.Location = new System.Drawing.Point(48, 754);
            this.button_TipColor.Name = "button_TipColor";
            this.button_TipColor.Size = new System.Drawing.Size(25, 25);
            this.button_TipColor.TabIndex = 3;
            this.button_TipColor.UseVisualStyleBackColor = false;
            this.button_TipColor.Visible = false;
            // 
            // label_TimerStartTime
            // 
            this.label_TimerStartTime.Location = new System.Drawing.Point(79, 754);
            this.label_TimerStartTime.Name = "label_TimerStartTime";
            this.label_TimerStartTime.Size = new System.Drawing.Size(100, 25);
            this.label_TimerStartTime.TabIndex = 4;
            this.label_TimerStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label_TimerStartTime.Visible = false;
            // 
            // button_TransferTimerColor
            // 
            this.button_TransferTimerColor.BackColor = System.Drawing.Color.DarkGreen;
            this.button_TransferTimerColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_TransferTimerColor.Location = new System.Drawing.Point(1100, 754);
            this.button_TransferTimerColor.Name = "button_TransferTimerColor";
            this.button_TransferTimerColor.Size = new System.Drawing.Size(25, 25);
            this.button_TransferTimerColor.TabIndex = 5;
            this.button_TransferTimerColor.UseVisualStyleBackColor = false;
            this.button_TransferTimerColor.Visible = false;
            // 
            // label_TransferTimerStartTime
            // 
            this.label_TransferTimerStartTime.Location = new System.Drawing.Point(1131, 754);
            this.label_TransferTimerStartTime.Name = "label_TransferTimerStartTime";
            this.label_TransferTimerStartTime.Size = new System.Drawing.Size(100, 25);
            this.label_TransferTimerStartTime.TabIndex = 6;
            this.label_TransferTimerStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label_TransferTimerStartTime.Visible = false;
            // 
            // button_RecordCurrentBackColorTransfer
            // 
            this.button_RecordCurrentBackColorTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_RecordCurrentBackColorTransfer.Location = new System.Drawing.Point(1069, 754);
            this.button_RecordCurrentBackColorTransfer.Name = "button_RecordCurrentBackColorTransfer";
            this.button_RecordCurrentBackColorTransfer.Size = new System.Drawing.Size(25, 25);
            this.button_RecordCurrentBackColorTransfer.TabIndex = 7;
            this.button_RecordCurrentBackColorTransfer.UseVisualStyleBackColor = true;
            this.button_RecordCurrentBackColorTransfer.Visible = false;
            // 
            // StockerDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.button_RecordCurrentBackColorTransfer);
            this.Controls.Add(this.label_TransferTimerStartTime);
            this.Controls.Add(this.button_TransferTimerColor);
            this.Controls.Add(this.label_TimerStartTime);
            this.Controls.Add(this.button_TipColor);
            this.Controls.Add(this.button_RecordCurrentBackColor);
            this.Controls.Add(this.dataGridViewStocker);
            this.Name = "StockerDisplay";
            this.Size = new System.Drawing.Size(1700, 800);
            this.Load += new System.EventHandler(this.StockerDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStocker)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewStocker;
        private System.Windows.Forms.Button button_RecordCurrentBackColor;
        private System.Windows.Forms.Timer timer_ChangeBackColor;
        private System.Windows.Forms.Button button_TipColor;
        private System.Windows.Forms.Label label_TimerStartTime;
        private System.Windows.Forms.Button button_TransferTimerColor;
        private System.Windows.Forms.Label label_TransferTimerStartTime;
        private System.Windows.Forms.Button button_RecordCurrentBackColorTransfer;
    }
}
