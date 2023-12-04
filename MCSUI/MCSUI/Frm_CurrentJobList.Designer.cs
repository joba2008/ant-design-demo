namespace MCSUI
{
    partial class Frm_CurrentJobList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_CurrentJobList));
            this.dataGridView_CurrentJobList = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CurrentJobList)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_CurrentJobList
            // 
            this.dataGridView_CurrentJobList.AllowUserToAddRows = false;
            this.dataGridView_CurrentJobList.AllowUserToDeleteRows = false;
            this.dataGridView_CurrentJobList.AllowUserToResizeColumns = false;
            this.dataGridView_CurrentJobList.AllowUserToResizeRows = false;
            this.dataGridView_CurrentJobList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridView_CurrentJobList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_CurrentJobList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_CurrentJobList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_CurrentJobList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_CurrentJobList.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_CurrentJobList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView_CurrentJobList.Name = "dataGridView_CurrentJobList";
            this.dataGridView_CurrentJobList.ReadOnly = true;
            this.dataGridView_CurrentJobList.RowHeadersVisible = false;
            this.dataGridView_CurrentJobList.RowTemplate.Height = 27;
            this.dataGridView_CurrentJobList.Size = new System.Drawing.Size(782, 553);
            this.dataGridView_CurrentJobList.TabIndex = 1;
            // 
            // Frm_CurrentJobList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.dataGridView_CurrentJobList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Frm_CurrentJobList";
            this.Text = "Frm_CurrentJobList";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CurrentJobList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_CurrentJobList;
    }
}