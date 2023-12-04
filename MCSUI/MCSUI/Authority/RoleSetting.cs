using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MCSUI.Authority
{
    public partial class RoleSetting : UserControl
    {
        public string loginUser = string.Empty;
        public RoleSetting()
        {
            InitializeComponent();
        }
        public RoleSetting(string whoLogin)
        {
            InitializeComponent();
            loginUser = whoLogin;
            checkedListBox_RoleSetting_Step3.SetItemChecked(0, true);
            checkedListBox_RoleSetting_Step3.SetItemCheckState(0, CheckState.Indeterminate);
            checkedListBox_RoleSetting_Step3.SetItemChecked(4, true);
            checkedListBox_RoleSetting_Step3.SetItemCheckState(4, CheckState.Indeterminate);
        }
        private void clearCheckedListBox(CheckedListBox obj)
        {
            CommonFunction comm = new CommonFunction();
            try
            {
                for (int i = 0; i < obj.Items.Count; i++) obj.SetItemChecked(i, false);
            }
            catch (Exception ex)
            {
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        private void onlySelectOneItem(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Checked) return;
            for (int i = 0; i < ((CheckedListBox)sender).Items.Count; i++) ((CheckedListBox)sender).SetItemChecked(i, false);
            e.NewValue = CheckState.Checked;
        }
        private void checkedListBox_RoleSetting_Step1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string errMessage = string.Empty;
            groupBox_RoleSetting_Step2.Enabled = true;
            if (checkedListBox_RoleSetting_Step1.Text == "ADD ROLE")
            {
                checkedListBox_RoleSetting_Step2.Visible = false;
                textBox_RoleSetting_Step2.Visible = true;
            }
            else if (checkedListBox_RoleSetting_Step1.Text == "DELETE ROLE")
            {
                checkedListBox_RoleSetting_Step2.Visible = true;
                textBox_RoleSetting_Step2.Visible = false;
                DataSet allRoleID = ServiceHelper.GetService().getAllRoleID(ref errMessage);
                checkedListBox_RoleSetting_Step2.Items.Clear();
                foreach (DataRow datarow in allRoleID.Tables[0].Rows)
                {
                    checkedListBox_RoleSetting_Step2.Items.Add(datarow["role_id"].ToString());
                }
            }
            groupBox_RoleSetting_Step3.Enabled = false;
            groupBox_RoleSetting_Step4.Enabled = false;
        }
        private void checkedListBox_RoleSetting_Step1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            onlySelectOneItem(sender, e);
        }
        private void textBox_RoleSetting_Step2_TextChanged(object sender, EventArgs e)
        {
            if (textBox_RoleSetting_Step2.Text != "")
            {
                groupBox_RoleSetting_Step3.Enabled = true;
                groupBox_RoleSetting_Step4.Enabled = true;
            }
            else
            {
                groupBox_RoleSetting_Step3.Enabled = false;
            }
        }
        private void checkedListBox_RoleSetting_Step2_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool result = false;
            for (int index = 0; index < checkedListBox_RoleSetting_Step2.Items.Count; index++)
                result = result | checkedListBox_RoleSetting_Step2.GetItemChecked(index);
            if (result) groupBox_RoleSetting_Step4.Enabled = true;
            else groupBox_RoleSetting_Step4.Enabled = false;
        }
        private void checkedListBox_RoleSetting_Step3_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool result = false;
            for (int index = 0; index < checkedListBox_RoleSetting_Step3.Items.Count; index++)
                result = result | checkedListBox_RoleSetting_Step3.GetItemChecked(index);
            if(result) groupBox_RoleSetting_Step4.Enabled = true;
            else groupBox_RoleSetting_Step4.Enabled = false;
        }
        private void button_BindingAuthority_Click(object sender, EventArgs e)
        {
            string errMessage = "";
            DataSet userList = null;
            bool flag = false;
            bool result = true;
            //if (checkedListBox_RoleSetting_Step1.Text == "ADD ROLE")
            if (checkedListBox_RoleSetting_Step1.GetItemChecked(0))
            {
                foreach (string roleid in textBox_RoleSetting_Step2.Lines.Distinct())
                {
                    ServiceHelper.GetService().deleteRoleSetting(roleid, ref errMessage);
                    foreach (string item in checkedListBox_RoleSetting_Step3.CheckedItems)
                    {
                        flag = ServiceHelper.GetService().insertRoleSetting(roleid, item, ref errMessage);
                        if (!flag) MessageBox.Show("Insert Role ID " + roleid + " Faild, Reason is " + errMessage, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result = result & flag;
                    }
                }
                if (result)
                {
                    MessageBox.Show("Insert Data Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearCheckedListBox(checkedListBox_RoleSetting_Step1);
                    clearCheckedListBox(checkedListBox_RoleSetting_Step3);
                    textBox_RoleSetting_Step2.Clear();
                }
            }
            else
            {
                userList = ServiceHelper.GetService().getUserBaseOnRole(checkedListBox_RoleSetting_Step2.Text, ref errMessage);
                if (userList.Tables[0].Rows.Count > 0) MessageBox.Show("Some users use this role id '" + checkedListBox_RoleSetting_Step2.Text + "'", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (ServiceHelper.GetService().deleteRoleSetting(checkedListBox_RoleSetting_Step2.Text, ref errMessage))
                        MessageBox.Show("Delete Data Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else MessageBox.Show("Delete Data Faild, Reason is " + errMessage, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                DataSet allRoleID = ServiceHelper.GetService().getAllRoleID(ref errMessage);
                checkedListBox_RoleSetting_Step2.Items.Clear();
                foreach (DataRow datarow in allRoleID.Tables[0].Rows)
                {
                    checkedListBox_RoleSetting_Step2.Items.Add(datarow["role_id"].ToString());
                }
                clearCheckedListBox(checkedListBox_RoleSetting_Step1);
            }
        }
        private void checkedListBox_RoleSetting_Step2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            onlySelectOneItem(sender, e);
        }
    }
}
