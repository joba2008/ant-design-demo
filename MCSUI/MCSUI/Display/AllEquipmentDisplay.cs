using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MCSUI.Display
{
    public partial class AllEquipmentDisplay : UserControl
    {
        public AllEquipmentDisplay()
        {
            InitializeComponent();
            //AutoSizeColumn(dataGridViewAllEQPStatus);
            dataGridViewAllEQPStatus.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            int rowcount = 0;
            string errMessage = "";
            DataSet allequipmentbasicstatus = ServiceHelper.GetService().QueryAllEquipmentBasicStatus(ref errMessage);
            foreach (DataRow datarow in allequipmentbasicstatus.Tables[0].Rows)
            {
                dataGridViewAllEQPStatus.Rows.Add();
                dataGridViewAllEQPStatus.Rows[rowcount].Cells[0].Value = datarow["type"];
                dataGridViewAllEQPStatus.Rows[rowcount].Cells[1].Value = datarow["subtype"];
                dataGridViewAllEQPStatus.Rows[rowcount].Cells[2].Value = datarow["name"];
                dataGridViewAllEQPStatus.Rows[rowcount].Cells[3].Value = datarow["capacity"];
                dataGridViewAllEQPStatus.Rows[rowcount].Cells[4].Value = datarow["foupexist"];
                dataGridViewAllEQPStatus.Rows[rowcount].Cells[5].Value = Math.Round((double.Parse(datarow["foupexist"].ToString())/double.Parse(datarow["capacity"].ToString()))*100, 2).ToString() + " %";
                dataGridViewAllEQPStatus.Rows[rowcount].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (datarow["valiable"].ToString() == "0")
                {
                    dataGridViewAllEQPStatus.Rows[rowcount].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    dataGridViewAllEQPStatus.Rows[rowcount].DefaultCellStyle.BackColor = Color.Red;
                }
                rowcount++;
            }
        }

        private void AllEquipmentDisplay_Load(object sender, EventArgs e)
        {
            dataGridViewAllEQPStatus.ClearSelection();
        }
        //private void AutoSizeColumn(DataGridView dgViewFiles)
        //{
        //    int width = 0;
        //    for (int i = 0; i < dgViewFiles.Columns.Count; i++)
        //    {
        //        dgViewFiles.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
        //        width += dgViewFiles.Columns[i].Width;
        //    }
        //    if (width > dgViewFiles.Size.Width)
        //    {
        //        dgViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        //    }
        //    else
        //    {
        //        dgViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //    }
        //    dgViewFiles.Columns[1].Frozen = true;
        //}
    }
}
