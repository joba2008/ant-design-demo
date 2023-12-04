using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MCSUI.Authority
{
    public partial class PersonalSetting : UserControl
    {
        public string loginUser = string.Empty;
        public PersonalSetting()
        {
            InitializeComponent();
        }
        public PersonalSetting(string whoLogin)
        {
            string errMessage = string.Empty;
            DataSet userinfo = new DataSet();
            InitializeComponent();
            loginUser = whoLogin;
            textBox_PersonalSetting_UserID.Text = loginUser;
            userinfo = ServiceHelper.GetService().getUserInfo(loginUser, ref errMessage);
            foreach (DataRow datarow in userinfo.Tables[0].Rows)
            {
                //textBox_PersonalSetting_Password.Text = datarow["PW"].ToString();
                textBox_PersonalSetting_Email.Text = datarow["MAIL"].ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bool result = true;
            string errMessage = string.Empty;
            if (textBox_PersonalSetting_Password_DoubleConfirm.Text !=
                textBox_PersonalSetting_Password.Text)
            {
                MessageBox.Show("Password settings are different", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Regex.IsMatch(textBox_PersonalSetting_Email.Text, @"^([\w-]+\.)*?[\w-]+@[\w-]+\.([\w-]+\.)*?[\w]+$"))
            {
                result = ServiceHelper.GetService().updateUserInfo(textBox_PersonalSetting_Password.Text,
                                                                   textBox_PersonalSetting_Email.Text,
                                                                   loginUser, ref errMessage);
                if (result) MessageBox.Show("Update Data Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else MessageBox.Show("Update Data Faild, Reason is " + errMessage, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
