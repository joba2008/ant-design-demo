using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MCSUI
{
    public partial class Form_Edit : Form
    {
        public delegate void delegateSetting(string message);
        public event delegateSetting delegateEvent;
        public Form_Edit()
        {
            InitializeComponent();
        }
        public Form_Edit(string equipment, string itemname)
        {
            InitializeComponent();
            label_Edit_EQPID_Value.Text = equipment;
            label_Edit_ItemName_Value.Text = itemname;
            textBox_Edit_NewValue.Select();
        }
        private void button_Edit_Click(object sender, EventArgs e)
        {
            bool result = true;
            string errMessage = string.Empty;
            CommonFunction comm = new CommonFunction();
            result = ServiceHelper.GetService().updateBasicInformation(label_Edit_EQPID_Value.Text, 
                                                                       label_Edit_ItemName_Value.Text, 
                                                                       textBox_Edit_NewValue.Text, 
                                                                       ref errMessage);
            this.Close();
            if (result)
            {
                MessageBox.Show("Update Complete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                delegateEvent(comm.updateBasicInfo(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now)));
            }
            else
                MessageBox.Show("Update Failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
