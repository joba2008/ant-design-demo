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
    public partial class Frm_CurrentJobList : Form
    {
        public Frm_CurrentJobList()
        {
            InitializeComponent();
            string errMessage = string.Empty;
            DataSet allRecord = ServiceHelper.GetService().GetAllCurrentTask(
                                    string.Format("{0:yyyyMMddHHmmss}", DateTime.Now.AddDays(-1)),
                                    string.Format("{0:yyyyMMddHHmmss}", DateTime.Now.AddDays(1)), 
                                    ref errMessage);
            dataGridView_CurrentJobList.DataSource = allRecord.Tables[0];
            dataGridView_CurrentJobList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_CurrentJobList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView_CurrentJobList.DefaultCellStyle.Font = new Font("Consolas", 11);
        }
    }
}
