using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MCSUI
{
    class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form_Main());
            Form_Login frmLogin = new Form_Login();
            if (frmLogin.ShowDialog() == DialogResult.OK) Application.Run(new Form_Main(Form_Login.loginUser));
        }
    }
}
