namespace MCSUI
{
    partial class Form_Edit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Edit));
            this.label_Edit_ItemName_Item = new System.Windows.Forms.Label();
            this.label_Edit_EQPID_Item = new System.Windows.Forms.Label();
            this.label_Edit_NewValue = new System.Windows.Forms.Label();
            this.button_Edit = new System.Windows.Forms.Button();
            this.textBox_Edit_NewValue = new System.Windows.Forms.TextBox();
            this.label_Edit_EQPID_Value = new System.Windows.Forms.Label();
            this.label_Edit_ItemName_Value = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_Edit_ItemName_Item
            // 
            this.label_Edit_ItemName_Item.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Edit_ItemName_Item.Location = new System.Drawing.Point(12, 52);
            this.label_Edit_ItemName_Item.Name = "label_Edit_ItemName_Item";
            this.label_Edit_ItemName_Item.Size = new System.Drawing.Size(140, 23);
            this.label_Edit_ItemName_Item.TabIndex = 0;
            this.label_Edit_ItemName_Item.Text = "Item Name:";
            this.label_Edit_ItemName_Item.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Edit_EQPID_Item
            // 
            this.label_Edit_EQPID_Item.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Edit_EQPID_Item.Location = new System.Drawing.Point(12, 18);
            this.label_Edit_EQPID_Item.Name = "label_Edit_EQPID_Item";
            this.label_Edit_EQPID_Item.Size = new System.Drawing.Size(140, 23);
            this.label_Edit_EQPID_Item.TabIndex = 1;
            this.label_Edit_EQPID_Item.Text = "Equipment ID: ";
            this.label_Edit_EQPID_Item.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Edit_NewValue
            // 
            this.label_Edit_NewValue.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Edit_NewValue.Location = new System.Drawing.Point(12, 87);
            this.label_Edit_NewValue.Name = "label_Edit_NewValue";
            this.label_Edit_NewValue.Size = new System.Drawing.Size(140, 23);
            this.label_Edit_NewValue.TabIndex = 2;
            this.label_Edit_NewValue.Text = "New Value:";
            this.label_Edit_NewValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_Edit
            // 
            this.button_Edit.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Edit.Location = new System.Drawing.Point(158, 125);
            this.button_Edit.Name = "button_Edit";
            this.button_Edit.Size = new System.Drawing.Size(312, 30);
            this.button_Edit.TabIndex = 3;
            this.button_Edit.Text = "Confirm";
            this.button_Edit.UseVisualStyleBackColor = true;
            this.button_Edit.Click += new System.EventHandler(this.button_Edit_Click);
            // 
            // textBox_Edit_NewValue
            // 
            this.textBox_Edit_NewValue.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Edit_NewValue.Location = new System.Drawing.Point(158, 87);
            this.textBox_Edit_NewValue.Name = "textBox_Edit_NewValue";
            this.textBox_Edit_NewValue.Size = new System.Drawing.Size(312, 27);
            this.textBox_Edit_NewValue.TabIndex = 4;
            // 
            // label_Edit_EQPID_Value
            // 
            this.label_Edit_EQPID_Value.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Edit_EQPID_Value.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Edit_EQPID_Value.Location = new System.Drawing.Point(158, 18);
            this.label_Edit_EQPID_Value.Name = "label_Edit_EQPID_Value";
            this.label_Edit_EQPID_Value.Size = new System.Drawing.Size(312, 23);
            this.label_Edit_EQPID_Value.TabIndex = 5;
            this.label_Edit_EQPID_Value.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Edit_ItemName_Value
            // 
            this.label_Edit_ItemName_Value.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Edit_ItemName_Value.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Edit_ItemName_Value.Location = new System.Drawing.Point(158, 52);
            this.label_Edit_ItemName_Value.Name = "label_Edit_ItemName_Value";
            this.label_Edit_ItemName_Value.Size = new System.Drawing.Size(312, 23);
            this.label_Edit_ItemName_Value.TabIndex = 6;
            this.label_Edit_ItemName_Value.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Form_Edit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 173);
            this.Controls.Add(this.label_Edit_ItemName_Value);
            this.Controls.Add(this.label_Edit_EQPID_Value);
            this.Controls.Add(this.textBox_Edit_NewValue);
            this.Controls.Add(this.button_Edit);
            this.Controls.Add(this.label_Edit_NewValue);
            this.Controls.Add(this.label_Edit_EQPID_Item);
            this.Controls.Add(this.label_Edit_ItemName_Item);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Edit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Edit_ItemName_Item;
        private System.Windows.Forms.Label label_Edit_EQPID_Item;
        private System.Windows.Forms.Label label_Edit_NewValue;
        private System.Windows.Forms.Button button_Edit;
        private System.Windows.Forms.TextBox textBox_Edit_NewValue;
        private System.Windows.Forms.Label label_Edit_EQPID_Value;
        private System.Windows.Forms.Label label_Edit_ItemName_Value;
    }
}