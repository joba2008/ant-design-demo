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
    public partial class BindingAuthority : UserControl
    {
        public string loginUser = string.Empty;
        public BindingAuthority()
        {
            InitializeComponent();
        }
        public BindingAuthority(string whoLogin)
        {
            InitializeComponent();
            loginUser = whoLogin;
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
        private void checkedListBox_BindingAuthority_Step1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string errMessage = string.Empty;           
            groupBox_BindingAuthority_Step2.Enabled = true;
            if (checkedListBox_BindingAuthority_Step1.Text == "ADD USER")
            {
                checkedListBox_BindingAuthority_Step2.Visible = false;
                textBox_BindingAuthority_Step2.Visible = true;
                //---
                DataSet allRoleID = ServiceHelper.GetService().getAllRoleID(ref errMessage);
                checkedListBox_BindingAuthority_Step3.Items.Clear();
                foreach (DataRow datarow in allRoleID.Tables[0].Rows)
                    checkedListBox_BindingAuthority_Step3.Items.Add(datarow["role_id"].ToString());
            }
            else if (checkedListBox_BindingAuthority_Step1.Text == "DELETE USER")
            {
                checkedListBox_BindingAuthority_Step2.Visible = true;
                textBox_BindingAuthority_Step2.Visible = false;
                //---
                DataSet allUser = ServiceHelper.GetService().getAllUserID(ref errMessage);
                checkedListBox_BindingAuthority_Step2.Items.Clear();
                foreach (DataRow datarow in allUser.Tables[0].Rows)
                    checkedListBox_BindingAuthority_Step2.Items.Add(datarow["user_name"].ToString());
            }
        }
        private void checkedListBox_BindingAuthority_Step1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            onlySelectOneItem(sender, e);
        }
        private void textBox_BindingAuthority_Step2_TextChanged(object sender, EventArgs e)
        {
            if (textBox_BindingAuthority_Step2.Text != "") groupBox_BindingAuthority_Step3.Enabled = true;
            else groupBox_BindingAuthority_Step3.Enabled = false;
        }
        private void checkedListBox_BindingAuthority_Step3_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool result = false;
            for (int index = 0; index < checkedListBox_BindingAuthority_Step3.Items.Count; index++)
                result = result | checkedListBox_BindingAuthority_Step3.GetItemChecked(index);
            if (result) groupBox_BindingAuthority_Step4.Enabled = true;
            else groupBox_BindingAuthority_Step4.Enabled = false;
        }
        private void button_BindingAuthority_Click(object sender, EventArgs e)
        {
            string errMessage = string.Empty;
            bool flag = false;
            bool result = true;
            if (checkedListBox_BindingAuthority_Step1.GetItemChecked(0))
            {
                foreach (string user in textBox_BindingAuthority_Step2.Lines.Distinct())
                {
                    ServiceHelper.GetService().deleteUserSetting(user, ref errMessage);
                    foreach (string item in checkedListBox_BindingAuthority_Step3.CheckedItems)
                    {
                        try
                        {
                            ServiceHelper.GetService().deleteUserData(user, ref errMessage);
                            ServiceHelper.GetService().insertUserData(user, user, user + "@seeya-tech.com", loginUser, ref errMessage);
                        }
                        catch {} 
                        try
                        {
                            flag = ServiceHelper.GetService().insertUserSetting(user, item, ref errMessage);
                            if (!flag) MessageBox.Show("Add User " + user + " Faild, Reason is " + errMessage, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            result = result & flag;
                        }
                        catch
                        {
                            flag = false;
                            result = result & flag;
                        }
                    }
                }
                if (result)
                {
                    MessageBox.Show("Insert Data Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearCheckedListBox(checkedListBox_BindingAuthority_Step1);
                    clearCheckedListBox(checkedListBox_BindingAuthority_Step3);
                    textBox_BindingAuthority_Step2.Clear();
                }
            }
            else if (checkedListBox_BindingAuthority_Step1.GetItemChecked(1))
            {
                try
                {
                    ServiceHelper.GetService().deleteUserData(checkedListBox_BindingAuthority_Step2.Text, ref errMessage);
                }
                catch { }
                try
                {
                    result = ServiceHelper.GetService().deleteUserSetting(checkedListBox_BindingAuthority_Step2.Text, ref errMessage);
                    clearCheckedListBox(checkedListBox_BindingAuthority_Step1);
                    DataSet allUser = ServiceHelper.GetService().getAllUserID(ref errMessage);
                    checkedListBox_BindingAuthority_Step2.Items.Clear();
                    foreach (DataRow datarow in allUser.Tables[0].Rows)
                        checkedListBox_BindingAuthority_Step2.Items.Add(datarow["user_name"].ToString());
                    if (result) MessageBox.Show("Delete Data Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else MessageBox.Show("Delete Data Faild, Reason is " + errMessage, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch{ }
            }
        }
        private void checkedListBox_BindingAuthority_Step3_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            onlySelectOneItem(sender, e);
        }
        private void checkedListBox_BindingAuthority_Step2_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool result = false;
            for (int index = 0; index < checkedListBox_BindingAuthority_Step2.Items.Count; index++)
                result = result | checkedListBox_BindingAuthority_Step2.GetItemChecked(index);
            if (result) groupBox_BindingAuthority_Step4.Enabled = true;
            else groupBox_BindingAuthority_Step4.Enabled = false;
        }
        private void checkedListBox_BindingAuthority_Step2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            onlySelectOneItem(sender, e);
        }
    }
}