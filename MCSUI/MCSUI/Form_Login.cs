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
    public partial class Form_Login : Form
    {
        public static string loginUser { set; get; }
        public Form_Login()
        {
            InitializeComponent();
        }
        private void button_Login_OK_Click(object sender, EventArgs e)
        {
            if (checkUserAuthority(textBox_Login_UserID.Text, textBox_Login_PassWord.Text))
            {
                this.DialogResult = DialogResult.OK;
                Form_Login.loginUser = textBox_Login_UserID.Text;
            }
            else MessageBox.Show("Login Failed, Please Check User ID or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }
        private bool checkUserAuthority(string id, string password)
        {
            try
            {
                string errMessage = string.Empty;
                string count = string.Empty;
                DataSet UserAuthority = ServiceHelper.GetService().checkUserAuthority(id, password, ref errMessage);
                foreach (DataRow datarow in UserAuthority.Tables[0].Rows) count = datarow["COUNT"].ToString();
                if (count == "0") return false;
                else return true;
            }
            catch
            {
                return false;
            }
        }
        private void button_Login_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
