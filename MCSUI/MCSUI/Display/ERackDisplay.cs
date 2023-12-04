using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Reflection;


namespace MCSUI.Display
{
    public partial class ERackDisplay : UserControl
    {
        public ERackDisplay()
        {
            InitializeComponent();
            dataGridViewERack.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            try
            {
                CommonFunction comm = new CommonFunction();
                //---画出stocker的layout
                int aNumber = 0;
                int directionCount = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "DIRECTION"));
                int rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "FOUP_ROW"));
                int colCount = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "FOUP_COLUMN"));
                for (int iColumn = 0; iColumn < colCount; iColumn++)
                {
                    dataGridViewERack.Columns.Add(iColumn.ToString(), iColumn.ToString());
                }
                for (int iDirection = 1; iDirection <= directionCount; iDirection++)
                {
                    if (iDirection == directionCount & directionCount > 1) aNumber = rowCount + 1;
                    for (int iRow = 0; iRow < rowCount; iRow++)
                    {
                        dataGridViewERack.Rows.Add();
                        for (int iColumn = 0; iColumn < colCount; iColumn++)
                        {
                            //---组成location id, 格式为"Direction" "Row" "Column", "Row"与"Column"如果只有个位数需补零
                            dataGridViewERack.Rows[iRow + aNumber].Cells[iColumn].Value = iDirection.ToString() +
                                                                                            String.Format("{0:D2}", (rowCount - iRow)) +
                                                                                            String.Format("{0:D2}", (iColumn + 1));
                            dataGridViewERack.Rows[iRow + aNumber].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                    //---产生manual port的位置
                    if (iDirection != directionCount) dataGridViewERack.Rows.Add();
                }
                //dataGridViewERack.Rows[rowCount].Cells[0].Value = comm.ReadIni("CONFIG.INI", "ERACK", "MANUALPORT_1_LOCATIONID");
                //dataGridViewERack.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dataGridViewERack.Rows[rowCount].Cells[colCount - 1].Value = comm.ReadIni("CONFIG.INI", "ERACK", "MANUALPORT_2_LOCATIONID");
                //dataGridViewERack.Columns[colCount - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch (Exception ex)
            {
                CommonFunction comm = new CommonFunction();
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        public ERackDisplay(Dictionary<string, MCSUI.GlobalParameter.recordInfoBaseOnLocation> FOUPLocationMapping, string EquipmentID)
        {
            InitializeComponent();
            dataGridViewERack.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            try
            {
                int rowCount = 0, colCount = 0;
                Bitmap bitmap = null;
                CommonFunction comm = new CommonFunction();
                //---画出erack的layout
                int aNumber = 0;
                int characterlocationtodistributeequipmenttype = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "CHARACTER_LOCATION_TO_DISTRIBUTE_EQUIPMENT_TYPE"));
                int directionCount = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "DIRECTION"));
                /*
                 * Wafer Erack一共4个，3排4列
                 * Mask Erack一共2个，4排5列
                 * 
                 * LIEW0400, LIEW0300
                 * LIEW0200, LIEW0100
                 * LIER0100, LIER0200
                 * W是Wafer货架，R是Mask货架
                 */
                label_ERack_Equipment.Text = EquipmentID;
                string subtype = string.Empty;
                string errMessage = string.Empty;
                DataSet dsSubtype = ServiceHelper.GetService().getSubtypeBaseOnEquipmentName(EquipmentID, ref errMessage);
                foreach (DataRow datarow in dsSubtype.Tables[0].Rows) subtype = datarow["subtype"].ToString();
                if (subtype == "RETICLE CARRIER")
                {
                    rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "RETICLE_ROW"));
                    colCount = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "RETICLE_COLUMN"));
                }
                else
                {
                    colCount = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "FOUP_COLUMN"));
                    rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "FOUP_ROW"));
                }
                for (int iColumn = 0; iColumn < colCount; iColumn++)
                {
                    //dataGridViewERack.Columns.Add(iColumn.ToString(), iColumn.ToString());
                    DataGridViewImageColumn datagridviewimagecolumn = new DataGridViewImageColumn();
                    //DataGridViewButtonColumn datagridviewbuttoncolumn = new DataGridViewButtonColumn();
                    dataGridViewERack.Columns.Insert(iColumn, datagridviewimagecolumn);
                }
                for (int iDirection = 1; iDirection <= directionCount; iDirection++)
                {
                    if (iDirection == directionCount & directionCount > 1) aNumber = rowCount + 1;
                    for (int iRow = 0; iRow < rowCount; iRow++)
                    {
                        dataGridViewERack.Rows.Add();
                        for (int iColumn = 0; iColumn < colCount; iColumn++)
                        {
                            string locationID = String.Format("{0:D2}", (rowCount - iRow)) +
                                                String.Format("{0:D2}", (iColumn + 1)); String.Format("{0:D2}", (iColumn + 1));
                            bitmap = comm.conbinePictureTexts("erack_exist.png", locationID);
                            if (FOUPLocationMapping[locationID].foup_id.Trim() != "")
                            {
                                //bitmap = comm.conbinePictureTexts("erack_exist.png", FOUPLocationMapping[locationID].foup_id);
                                dataGridViewERack.Rows[iRow + aNumber].Cells[iColumn].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridViewERack.Rows[iRow + aNumber].Cells[iColumn].Value = bitmap;
                                dataGridViewERack.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.LightGreen;
                                dataGridViewERack.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = locationID + "#" + FOUPLocationMapping[locationID].foup_id;
                            }
                            else
                            {
                                //bitmap = comm.conbinePictureTexts("erack_empty.png", "");
                                dataGridViewERack.Rows[iRow + aNumber].Cells[iColumn].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridViewERack.Rows[iRow + aNumber].Cells[iColumn].Value = bitmap;
                                dataGridViewERack.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.White;
                                dataGridViewERack.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = locationID + "#";
                            }
                            dataGridViewERack.Rows[iRow + aNumber].Cells[iColumn].Tag = FOUPLocationMapping[locationID].location_available;
                            //if (FOUPLocationMapping[locationID].location_available == "0") dataGridViewERack.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.LightGreen;
                            if (FOUPLocationMapping[locationID].location_available == "1") dataGridViewERack.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.Red;
                        }
                    }
                    //---产生manual port的位置
                    if (iDirection != directionCount) dataGridViewERack.Rows.Add();
                }
                //dataGridViewERack.Rows[rowCount].Cells[0].Value = comm.ReadIni("CONFIG.INI", "ERACK", "MANUALPORT_1_LOCATIONID");
                //dataGridViewERack.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dataGridViewERack.Rows[rowCount].Cells[colCount - 1].Value = comm.ReadIni("CONFIG.INI", "ERACK", "MANUALPORT_2_LOCATIONID");
                //dataGridViewERack.Columns[colCount - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch (Exception ex)
            {
                CommonFunction comm = new CommonFunction();
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        public delegate void delegateSetting(string message);
        public event delegateSetting delegateEvent;
        private void dataGridViewERack_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //ERack不需要Transfer的功能
                //delegateEvent(dataGridViewERack.Rows[dataGridViewERack.CurrentCell.RowIndex].Cells[dataGridViewERack.CurrentCell.ColumnIndex].ToolTipText.Trim() + "#" +
                //              dataGridViewERack.Rows[dataGridViewERack.CurrentCell.RowIndex].Cells[dataGridViewERack.CurrentCell.ColumnIndex].Tag.ToString());
            }
            catch
            { }
        }
        private void ERackDisplay_Load(object sender, EventArgs e)
        {
            TIBCO.Rendezvous.Environment.Open();
            //---
            dataGridViewERack.ClearSelection();
            //---Subscribe to listen event from EAP
            MCSUI.Form_Main.delegateERackEvent += new MCSUI.Form_Main.delegateSetting(HandleMessageFromEAP);
        }
        private void AvoidTwinkle()
        {
            //---解决DataGridView更新闪烁问题
            Type type = dataGridViewERack.GetType();
            PropertyInfo propertyinfo = type.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            propertyinfo.SetValue(dataGridViewERack, true, null);
        }
        //---Handle message after receiving message
        private void HandleMessageFromEAP(string message)
        {
            try
            {
                CommonFunction comm = new CommonFunction();
                XmlDocument xmldoc = new XmlDocument();
                Bitmap bitmap = null;
                xmldoc.LoadXml(message.Trim());
                int rowNumInDataGrid = 0, columnInDataGrid = 0;
                string messageName = xmldoc.GetElementsByTagName("MESSAGENAME")[0].ChildNodes[0].InnerText;
                string equipmenttype = string.Empty, foupid = string.Empty, equipmentname = string.Empty, locationid = string.Empty,
                       location_x = string.Empty, location_y = string.Empty, locationstatus = string.Empty, equipmentstatus = string.Empty,
                       equipmentstatusdescription = string.Empty, squipmentstatusflag = string.Empty;
                XmlDocument xmldocument = new XmlDocument();
                XmlNodeList xmlnodelist_equipment = null, xmlnodelist_location = null;
                comm.LogRecordFun("TIBCORV", "Internal message, " + comm.createFormattedXML(message), comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH"));
                switch (messageName)
                {
                    case "SEARCHFOUPID":
                        try { equipmenttype = xmldoc.GetElementsByTagName("EQUIPMENTTYPE")[0].ChildNodes[0].InnerText; } catch { equipmenttype = ""; }
                        try { equipmentname = xmldoc.GetElementsByTagName("EQUIPMENTID")[0].ChildNodes[0].InnerText; } catch { equipmentname = ""; }
                        try { foupid = xmldoc.GetElementsByTagName("FOUPID")[0].ChildNodes[0].InnerText; } catch { foupid = ""; }
                        changeBackgroundColor(equipmentname, equipmenttype, foupid);
                        break;
                    case "ERackInitialAllInfo":
                        AvoidTwinkle();
                        xmldocument.LoadXml(message.Trim());
                        xmlnodelist_equipment = xmldocument.SelectNodes("//Body/EQUIPMENT");
                        for (int idx_equipment = 0; idx_equipment < xmlnodelist_equipment.Count; idx_equipment++)
                        {
                            equipmentname = xmlnodelist_equipment[idx_equipment].ChildNodes[0].InnerText;
                            equipmenttype = xmlnodelist_equipment[idx_equipment].ChildNodes[1].InnerText;
                            xmlnodelist_location = xmlnodelist_equipment[idx_equipment].ChildNodes[2].ChildNodes;
                            if (label_ERack_Equipment.Text == equipmentname)
                            {
                                for (int idx_location = 0; idx_location < xmlnodelist_location.Count; idx_location++)
                                {
                                    locationid = xmlnodelist_location[idx_location].ChildNodes[0].InnerText;
                                    location_x = xmlnodelist_location[idx_location].ChildNodes[1].InnerText;
                                    location_y = xmlnodelist_location[idx_location].ChildNodes[2].InnerText;
                                    locationstatus = xmlnodelist_location[idx_location].ChildNodes[3].InnerText;
                                    foupid = xmlnodelist_location[idx_location].ChildNodes[4].InnerText;
                                    rowNumInDataGrid = dataGridViewERack.Rows.Count - int.Parse(location_x);
                                    columnInDataGrid = int.Parse(location_y) - 1;
                                    if (locationstatus == "FULL") dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.LightGreen;
                                    if (locationstatus == "EMPTY") dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.White;
                                    bitmap = comm.conbinePictureTexts("erack_exist.png", locationid);
                                    dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Value = bitmap;
                                    dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].ToolTipText = locationid + "#" + foupid;
                                }
                            }
                        }
                        TimerClass.DestroyTimer();
                        break;
                    case "UpdateERackInfo":
                        AvoidTwinkle();
                        xmldocument.LoadXml(message.Trim());
                        xmlnodelist_equipment = xmldocument.SelectNodes("//Body");
                        for (int idx_equipment = 0; idx_equipment < xmlnodelist_equipment.Count; idx_equipment++)
                        {
                            equipmentname = xmlnodelist_equipment[idx_equipment].ChildNodes[0].InnerText;
                            equipmenttype = xmlnodelist_equipment[idx_equipment].ChildNodes[1].InnerText;
                            xmlnodelist_location = xmlnodelist_equipment[idx_equipment].ChildNodes[2].ChildNodes;
                            if (label_ERack_Equipment.Text == equipmentname)
                            {
                                for (int idx_location = 0; idx_location < xmlnodelist_location.Count; idx_location++)
                                {
                                    locationid = xmlnodelist_location[idx_location].ChildNodes[0].InnerText;
                                    location_x = xmlnodelist_location[idx_location].ChildNodes[1].InnerText;
                                    location_y = xmlnodelist_location[idx_location].ChildNodes[2].InnerText;
                                    locationstatus = xmlnodelist_location[idx_location].ChildNodes[3].InnerText;
                                    foupid = xmlnodelist_location[idx_location].ChildNodes[4].InnerText;
                                    rowNumInDataGrid = dataGridViewERack.Rows.Count - int.Parse(location_x);
                                    columnInDataGrid = int.Parse(location_y) - 1;
                                    if (locationstatus == "FULL") dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.LightGreen;
                                    if (locationstatus == "EMPTY") dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.White;
                                    bitmap = comm.conbinePictureTexts("erack_exist.png", locationid);
                                    dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Value = bitmap;
                                    dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].ToolTipText = locationid + "#" + foupid;
                                }
                            }
                        }
                        break;
                    case "UpdateERackBoxInfo":
                        AvoidTwinkle();
                        xmldocument.LoadXml(message.Trim());
                        xmlnodelist_equipment = xmldocument.SelectNodes("//Body");
                        for (int idx_equipment = 0; idx_equipment < xmlnodelist_equipment.Count; idx_equipment++)
                        {
                            equipmentname = xmlnodelist_equipment[idx_equipment].ChildNodes[0].InnerText;
                            equipmenttype = xmlnodelist_equipment[idx_equipment].ChildNodes[1].InnerText;
                            if (label_ERack_Equipment.Text == equipmentname)
                            {
                                locationid = xmlnodelist_equipment[idx_equipment].ChildNodes[2].InnerText;
                                location_x = xmlnodelist_equipment[idx_equipment].ChildNodes[3].InnerText;
                                location_y = xmlnodelist_equipment[idx_equipment].ChildNodes[4].InnerText;
                                locationstatus = xmlnodelist_equipment[idx_equipment].ChildNodes[5].InnerText;
                                foupid = xmlnodelist_equipment[idx_equipment].ChildNodes[6].InnerText;
                                rowNumInDataGrid = dataGridViewERack.Rows.Count - int.Parse(location_x);
                                columnInDataGrid = int.Parse(location_y) - 1;
                                //if (locationstatus == "FULL") bitmap = comm.conbinePictureTexts("erack_exist.png", foupid);
                                //if (locationstatus == "EMPTY") bitmap = comm.conbinePictureTexts("erack_empty.png", foupid);
                                //dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Value = bitmap;
                                //dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].ToolTipText = locationid + "#" + foupid;
                                if (locationstatus == "FULL") dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.LightGreen;
                                if (locationstatus == "EMPTY") dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.White;
                                bitmap = comm.conbinePictureTexts("erack_exist.png", locationid);
                                dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Value = bitmap;
                                dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].ToolTipText = locationid + "#" + foupid;
                            }
                        }
                        break;
                    case "ShowMessage":
                        AvoidTwinkle();
                        xmldocument.LoadXml(message.Trim());
                        xmlnodelist_equipment = xmldocument.SelectNodes("//Body");
                        for (int idx_equipment = 0; idx_equipment < xmlnodelist_equipment.Count; idx_equipment++)
                        {
                            equipmentname = xmlnodelist_equipment[idx_equipment].ChildNodes[0].InnerText;
                            equipmentstatusdescription = xmlnodelist_equipment[idx_equipment].ChildNodes[1].InnerText;
                            squipmentstatusflag = xmlnodelist_equipment[idx_equipment].ChildNodes[2].InnerText;
                            if (label_ERack_Equipment.Text == equipmentname)
                            {
                                switch (squipmentstatusflag.Trim())
                                {
                                    case "0":
                                        //label_ERack_Equipment.ForeColor = Color.Black;
                                        delegateEvent(comm.ShowMessage(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now), equipmentname, equipmentstatusdescription));
                                        break;
                                    case "1":
                                        //label_ERack_Equipment.ForeColor = Color.Red;
                                        delegateEvent(comm.ShowAlarmMessage(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now), equipmentname, "ERACK", equipmentstatusdescription));
                                        break;
                                }
                                //delegateEvent(comm.ShowMessage(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now), equipmentname, equipmentstatusdescription));
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonFunction comm = new CommonFunction();
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        private void changeBackgroundColor(string equipmentid, string equipmenttype, string foupid)
        {
            CommonFunction comm = new CommonFunction();
            try
            {
                string errMessage = "", equipmentID = "", direction = "", location_x = "", location_y = "", location_id = "";
                DataSet locationofeachfoup = ServiceHelper.GetService().QueryLocationBaseOnFOUPIDWithoutEquipmentIDAndType(foupid, ref errMessage);
                if (!(locationofeachfoup.Tables[0].Rows.Count > 1 ||
                    locationofeachfoup.Tables[0].Rows.Count == 0))
                {
                    foreach (DataRow datarow in locationofeachfoup.Tables[0].Rows)
                    {
                        equipmentID = datarow["name"].ToString();
                        direction = datarow["location_direction"].ToString();
                        location_x = datarow["location_x"].ToString();
                        location_y = datarow["location_y"].ToString();
                        location_id = datarow["location_id"].ToString();
                        break;
                    }
                    int rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "ROW"));
                    int colCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "COLUMN"));
                    int rowNumInDataGrid = 0, columnInDataGrid = 0;
                    if (label_ERack_Equipment.Text == equipmentID)
                    {
                        rowNumInDataGrid = dataGridViewERack.Rows.Count - int.Parse(location_x);
                        columnInDataGrid = int.Parse(location_y) - 1;
                        button_RecordCurrentBackColor.BackColor = dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor;
                        //---这里要使用System.Timers.Timer，而不是System.Form.Timer
                        System.Timers.Timer timer = new System.Timers.Timer();
                        timer.Interval = double.Parse(comm.ReadIni("CONFIG.INI", "TIMER", "INTERVAL")) * 1000;
                        timer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => timer_ChangeBackColor_Tick(sender, e, dataGridViewERack, rowNumInDataGrid, columnInDataGrid, timer));
                        timer.Start();
                        label_TimerStartTime.Text = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now.AddSeconds(double.Parse(comm.ReadIni("CONFIG.INI", "TIMER", "COUNTTOSTOP"))));
                    }
                }
                else if (locationofeachfoup.Tables[0].Rows.Count == 0)
                {
                    //MessageBox.Show("FOUP '" + foupid + "' Not Found");
                    delegateEvent(comm.ShowAlarmMessage(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now), equipmentid, equipmenttype, comm.EncodeBase64("FOUP '" + foupid + "' Not Found")));
                }
                else
                {
                    //MessageBox.Show("Least Two FOUP Have Same ID '" + foupid  + "'");
                    delegateEvent(comm.ShowAlarmMessage(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now), equipmentid, equipmenttype, comm.EncodeBase64("Least Two FOUP Have Same ID '" + foupid + "'")));
                }
            }
            catch (Exception ex)
            {
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        private void timer_ChangeBackColor_Tick(object sender, System.Timers.ElapsedEventArgs e, DataGridView datagridview, int rowNumInDataGrid, int columnInDataGrid, System.Timers.Timer timer)
        {
            try
            {
                CommonFunction comm = new CommonFunction();
                string message = "";
                AvoidTwinkle();
                if (dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor == button_RecordCurrentBackColor.BackColor)
                {
                    dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = button_TipColor.BackColor;
                }
                else
                {
                    dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = button_RecordCurrentBackColor.BackColor;
                }
                if (string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) == label_TimerStartTime.Text)
                {
                    timer.Stop();
                    dataGridViewERack.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = button_RecordCurrentBackColor.BackColor;
                    message = comm.clearTextBoxSearchFOUP();
                    delegateEvent(message);
                }
            }
            catch (Exception ex)
            {
                CommonFunction comm = new CommonFunction();
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        private void dataGridViewERack_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
            e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
        }
    }
}