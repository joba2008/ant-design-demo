using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using TIBCO.Rendezvous;

namespace MCSUI.Display
{
    public partial class StockerDisplay : UserControl
    {
        public string currentEquipment = string.Empty;
        public string n2purgelocation = string.Empty;
        public string charginglocation = string.Empty;
        public StockerDisplay(Dictionary<string, MCSUI.GlobalParameter.recordInfoBaseOnLocation> FOUPLocationMapping, string EquipmentID)
        {
            InitializeComponent();
            try
            {
                Bitmap bitmap = null;
                CommonFunction comm = new CommonFunction();
                //---图片檔只读取一次
                //Image imgExist = comm.getPicture("stocker_exist.png");
                //Image imgNotexist = comm.getPicture("stocker_empty.png");
                //Image imgEmpty = comm.getPicture("empty.png");
                //Image imgLoadport = comm.getPicture("loadport.png");
                //Image imgBL = comm.getPicture("BL.png");
                //Image imgHL = comm.getPicture("HL.png");
                //Image imgHR = comm.getPicture("HR.png");
                //Image imgBR = comm.getPicture("BR.png");
                Image imgExist = Properties.Resources.stocker_exist;
                Image imgNotexist = Properties.Resources.stocker_empty;
                Image imgEmpty = Properties.Resources.empty;
                Image imgLoadport = Properties.Resources.loadport;
                Image imgBL = Properties.Resources.BL;
                Image imgHL = Properties.Resources.HL;
                Image imgHR = Properties.Resources.HR;
                Image imgBR = Properties.Resources.BR;
                Image imgLoadport_1 = Properties.Resources.loadport_1;
                Image imgLoadport_2 = Properties.Resources.loadport_2;
                Image imgLoadport_3 = Properties.Resources.loadport_3;
                Image imgLoadport_4 = Properties.Resources.loadport_4;
                string errMessage = string.Empty;
                string subtype = string.Empty;
                int aNumber = 0;
                int directionCount = 0;
                int rowCount = 0;
                int colCount = 0;
                //---画出stocker的layout
                DataSet dsSubtype = ServiceHelper.GetService().getSubtypeBaseOnEquipmentName(EquipmentID, ref errMessage);
                foreach (DataRow datarow in dsSubtype.Tables[0].Rows) subtype = datarow["subtype"].ToString();
                if (subtype == "FOUP CARRIER")
                {
                    dataGridViewStocker.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewStocker.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    aNumber = 0;
                    directionCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "DIRECTION"));
                    rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "ROW"));
                    colCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "COLUMN"));
                    for (int iColumn = 0; iColumn < colCount; iColumn++)
                    {
                        //dataGridViewStocker.Columns.Add(iColumn.ToString(), iColumn.ToString());
                        DataGridViewImageColumn datagridviewimagecolumn = new DataGridViewImageColumn();
                        dataGridViewStocker.Columns.Insert(iColumn, datagridviewimagecolumn);
                    }
                    for (int iDirection = 1; iDirection <= directionCount; iDirection++)
                    {
                        if (iDirection == directionCount & directionCount > 1) aNumber = rowCount;
                        for (int iRow = 0; iRow < rowCount; iRow++)
                        {
                            dataGridViewStocker.Rows.Add();
                            for (int iColumn = 0; iColumn < colCount; iColumn++)
                            {
                                string locationID = string.Empty;
                                if (iDirection == 1) locationID = iDirection.ToString() + String.Format("{0:D2}", (rowCount - iRow)) + String.Format("{0:D2}", (iColumn + 1));
                                else locationID = iDirection.ToString() + String.Format("{0:D2}", (iRow + 1)) + String.Format("{0:D2}", (iColumn + 1));
                                if ((iDirection == directionCount) && (iRow >= 0 && iRow < 5) && ((iColumn >= 0 && iColumn < 3) || (iColumn >= 29 && iColumn < 32)))
                                {
                                    if (locationID == "20201" || locationID == "20230")
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Value = imgLoadport_1;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.Alignment = DataGridViewContentAlignment.BottomRight;
                                    }
                                    else if (locationID == "20203" || locationID == "20232")
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Value = imgLoadport_2;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.Alignment = DataGridViewContentAlignment.BottomLeft;
                                    }
                                    else if (locationID == "20401" || locationID == "20430")
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Value = imgLoadport_3;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.Alignment = DataGridViewContentAlignment.TopRight;
                                    }
                                    else if (locationID == "20403" || locationID == "20432")
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Value = imgLoadport_4;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.Alignment = DataGridViewContentAlignment.TopLeft;
                                    }
                                    else
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Value = imgEmpty;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    }
                                    dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.GhostWhite;
                                    if (iColumn >= 0 && iColumn < 3)
                                    {
                                        if (FOUPLocationMapping["30101"].foup_id != " " &&
                                            FOUPLocationMapping["30101"].foup_id != "")
                                        {
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = "30101#" + FOUPLocationMapping["30101"].foup_id + "#" + FOUPLocationMapping["30101"].port_status;
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.Yellow;
                                        }
                                        else
                                        {
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = "30101#" + "#" + FOUPLocationMapping["30101"].port_status;
                                        }
                                    }
                                    else if (iColumn >= 29 && iColumn < 32)
                                    {
                                        if (FOUPLocationMapping["30102"].foup_id != " " &&
                                            FOUPLocationMapping["30102"].foup_id != "")
                                        {
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = "30102#" + FOUPLocationMapping["30102"].foup_id + "#" + FOUPLocationMapping["30102"].port_status;
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.Yellow;
                                        }
                                        else
                                        {
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = "30102#" + "#" + FOUPLocationMapping["30102"].port_status;
                                        }
                                    }
                                }
                                else
                                {
                                    bitmap = comm.conbinePictureTexts_Stocker("stocker_exist.png", locationID);
                                    dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Value = bitmap;
                                    if (FOUPLocationMapping[locationID].location_available == "1")
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.Red;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = locationID + "#" + FOUPLocationMapping[locationID].foup_id;
                                    }
                                    else if (FOUPLocationMapping[locationID].foup_id != " " && FOUPLocationMapping[locationID].foup_id != "")
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.LightGreen;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = locationID + "#" + FOUPLocationMapping[locationID].foup_id;
                                    }
                                    else
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.White;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = locationID + "#";
                                        //if (FOUPLocationMapping[locationID].location_available == "0") dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.LightGreen;
                                        //if (FOUPLocationMapping[locationID].location_available == "1") dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.Red;
                                        if (FOUPLocationMapping[locationID].location_available == "2")
                                        {
                                            n2purgelocation += locationID + "#";
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.LightSalmon;
                                        }
                                        else if (FOUPLocationMapping[locationID].location_available == "3")
                                        {
                                            charginglocation += locationID + "#";
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.LightSkyBlue;
                                        }
                                    }
                                    dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Tag = FOUPLocationMapping[locationID].location_available;
                                }
                            }
                        }
                    }
                }
                else if (subtype == "RETICLE CARRIER")
                {
                    dataGridViewStocker.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    dataGridViewStocker.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    aNumber = 0;
                    directionCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "DIRECTION"));
                    rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "RETICLE_ROW"));
                    colCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "RETICLE_COLUMN"));
                    for (int iColumn = 0; iColumn < colCount; iColumn++)
                    {
                        DataGridViewImageColumn datagridviewimagecolumn = new DataGridViewImageColumn();
                        dataGridViewStocker.Columns.Insert(iColumn, datagridviewimagecolumn);
                    }
                    for (int iDirection = 1; iDirection <= directionCount; iDirection++)
                    {
                        if (iDirection == directionCount & directionCount > 1) aNumber = rowCount;
                        for (int iRow = 0; iRow < rowCount; iRow++)
                        {
                            dataGridViewStocker.Rows.Add();
                            for (int iColumn = 0; iColumn < colCount; iColumn++)
                            {
                                string locationID = string.Empty;
                                if (iDirection == 1) locationID = iDirection.ToString() + String.Format("{0:D2}", (rowCount - iRow)) + String.Format("{0:D2}", (iColumn + 1));
                                else locationID = iDirection.ToString() + String.Format("{0:D2}", (iRow + 1)) + String.Format("{0:D2}", (iColumn + 1));
                                if ((iDirection == directionCount) && (iRow >= 0 && iRow < 6) && ((iColumn >= 0 && iColumn < 4)))
                                {
                                    if (locationID == "20201" || locationID == "20203")
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Value = imgLoadport_1;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.Alignment = DataGridViewContentAlignment.BottomCenter;
                                    }
                                    else if (locationID == "20202" || locationID == "20204")
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Value = imgLoadport_2;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.Alignment = DataGridViewContentAlignment.BottomCenter;
                                    }
                                    else if (locationID == "20501" || locationID == "20503")
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Value = imgLoadport_3;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.Alignment = DataGridViewContentAlignment.TopCenter;
                                    }
                                    else if (locationID == "20502" || locationID == "20504")
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Value = imgLoadport_4;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.Alignment = DataGridViewContentAlignment.TopCenter;
                                    }
                                    else
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Value = imgEmpty;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    }
                                    dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.GhostWhite;
                                    if (iColumn >= 0 && iColumn < 2)
                                    {
                                        if (FOUPLocationMapping["30101"].foup_id != " " &&
                                            FOUPLocationMapping["30101"].foup_id != "")
                                        {
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = "30101#" + FOUPLocationMapping["30101"].foup_id + "#" + FOUPLocationMapping["30101"].port_status;
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.Yellow;
                                        }
                                        else
                                        {
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = "30101#" + "#" + FOUPLocationMapping["30101"].port_status;
                                        }
                                        //dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_1_LOCATIONID") + "#";
                                    }

                                    else if (iColumn >= 2 && iColumn < 4)
                                    {
                                        if (FOUPLocationMapping["30102"].foup_id != " " &&
                                            FOUPLocationMapping["30102"].foup_id != "")
                                        {
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = "30102#" + FOUPLocationMapping["30102"].foup_id + "#" + FOUPLocationMapping["30102"].port_status;
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.Yellow;
                                        }
                                        else
                                        {
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = "30102#" + "#" + FOUPLocationMapping["30102"].port_status;
                                        }
                                        //dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_2_LOCATIONID") + "#";
                                    }
                                }
                                else
                                {
                                    bitmap = comm.conbinePictureTexts_Stocker("stocker_exist.png", locationID);
                                    dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Value = bitmap;
                                    if (FOUPLocationMapping[locationID].location_available == "1")
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.Red;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = locationID + "#" + FOUPLocationMapping[locationID].foup_id;
                                    }
                                    else if (FOUPLocationMapping[locationID].foup_id != " " && FOUPLocationMapping[locationID].foup_id != "")
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.LightGreen;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = locationID + "#" + FOUPLocationMapping[locationID].foup_id;
                                    }
                                    else
                                    {
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.White;
                                        dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].ToolTipText = locationID + "#";
                                        //if (FOUPLocationMapping[locationID].location_available == "0") dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.LightGreen;
                                        //if (FOUPLocationMapping[locationID].location_available == "1") dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.Red;
                                        if (FOUPLocationMapping[locationID].location_available == "2")
                                        {
                                            n2purgelocation += locationID + "#";
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.LightSalmon;
                                        }
                                        else if (FOUPLocationMapping[locationID].location_available == "3")
                                        {
                                            charginglocation += locationID + "#";
                                            dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Style.BackColor = Color.LightSkyBlue;
                                        }
                                    }
                                    dataGridViewStocker.Rows[iRow + aNumber].Cells[iColumn].Tag = FOUPLocationMapping[locationID].location_available;
                                }
                            }
                        }
                    }
                }
                currentEquipment = EquipmentID;
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
        private void dataGridViewStocker_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string TipText = dataGridViewStocker.Rows[dataGridViewStocker.CurrentCell.RowIndex].Cells[dataGridViewStocker.CurrentCell.ColumnIndex].ToolTipText.Trim();
                string Tag = dataGridViewStocker.Rows[dataGridViewStocker.CurrentCell.RowIndex].Cells[dataGridViewStocker.CurrentCell.ColumnIndex].Tag.ToString();
                CommonFunction comm = new CommonFunction();
                string message = comm.sendLocationToMainForm(TipText.Split('#')[0].Trim(), TipText.Split('#')[1].Trim(), Tag.Trim());
                //---Sending the value of usercontrol to main form;
                delegateEvent(message);
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
        private void StockerDisplay_Load(object sender, EventArgs e)
        {
            TIBCO.Rendezvous.Environment.Open();
            //---Cancel the pre-selected field in "DataGridView"
            dataGridViewStocker.ClearSelection();
            //---Subscribe to listen event from EAP
            MCSUI.Form_Main.delegateStockerEvent += new MCSUI.Form_Main.delegateSetting(HandleMessageFromEAP);
        }
        private void dataGridViewStocker_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //dataGridViewStocker.CurrentCell.ColumnIndex
            //dataGridViewStocker.CurrentCell.RowIndex
        }
        private void AvoidTwinkle()
        {
            //---解决DataGridView更新闪烁问题
            Type type = dataGridViewStocker.GetType();
            PropertyInfo propertyinfo = type.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            propertyinfo.SetValue(dataGridViewStocker, true, null);
        }
        private void changeLoadPortBackColor(int rowStart, int rowEnd, int columnStart, int columnEnd, string foupid)
        {
            CommonFunction comm = new CommonFunction();
            for (int idxRow = rowStart; idxRow <= rowEnd; idxRow++)
            {
                for (int idxCol = columnStart; idxCol <= columnEnd; idxCol++)
                {
                    if (foupid == string.Empty)
                    {
                        dataGridViewStocker.Rows[idxRow].Cells[idxCol].Style.BackColor = Color.GhostWhite;
                        dataGridViewStocker.Rows[idxRow].Cells[idxCol].ToolTipText =
                            comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_1_LOCATIONID") + "#";
                    }
                    else
                    {
                        dataGridViewStocker.Rows[idxRow].Cells[idxCol].Style.BackColor = Color.Yellow;
                        dataGridViewStocker.Rows[idxRow].Cells[idxCol].ToolTipText =
                            comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_1_LOCATIONID") + "#" + foupid;
                    }
                }
            }
        }
        private void changeLoadPortBackColor(int rowStart, int rowEnd, int columnStart, int columnEnd, string foupid, string portstatus)
        {
            CommonFunction comm = new CommonFunction();
            for (int idxRow = rowStart; idxRow <= rowEnd; idxRow++)
            {
                for (int idxCol = columnStart; idxCol <= columnEnd; idxCol++)
                {
                    if (foupid == string.Empty)
                    {
                        dataGridViewStocker.Rows[idxRow].Cells[idxCol].Style.BackColor = Color.GhostWhite;
                        dataGridViewStocker.Rows[idxRow].Cells[idxCol].ToolTipText =
                            comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_1_LOCATIONID") + "##" + portstatus;
                    }
                    else
                    {
                        dataGridViewStocker.Rows[idxRow].Cells[idxCol].Style.BackColor = Color.Yellow;
                        dataGridViewStocker.Rows[idxRow].Cells[idxCol].ToolTipText =
                            comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_1_LOCATIONID") + "#" + foupid + "#" + portstatus;
                    }
                }
            }
        }
        private void changeLoadPortBackColorforTransferTimer(int rowStart, int rowEnd, int columnStart, int columnEnd, Color color)
        {
            CommonFunction comm = new CommonFunction();
            for (int idxRow = rowStart; idxRow <= rowEnd; idxRow++)
            {
                for (int idxCol = columnStart; idxCol <= columnEnd; idxCol++)
                {
                    dataGridViewStocker.Rows[idxRow].Cells[idxCol].Style.BackColor = color;
                }
            }
        }
        //---Handle message after receiving message
        private void HandleMessageFromEAP(string message)
        {
            try
            {
                CommonFunction comm = new CommonFunction();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(message.Trim());
                int rowNumInDataGrid = 0, columnInDataGrid = 0;
                string subtype = string.Empty;
                string errMessage = string.Empty;
                string messageName = xmldoc.GetElementsByTagName("MESSAGENAME")[0].ChildNodes[0].InnerText;
                string equipmenttype = string.Empty, foupid = string.Empty, equipmentname = string.Empty, locationid = string.Empty, source = string.Empty, destination = string.Empty,
                       location_x = string.Empty, location_y = string.Empty, locationstatus = string.Empty, location_direction = string.Empty, equipmentstatus = string.Empty,
                       equipmentstatusdescription = string.Empty, squipmentstatusflag = string.Empty, equipmentid = string.Empty;
                XmlNodeList xmlnodelist_equipment = null, xmlnodelist_location = null;
                switch (messageName)
                {
                    case "SEARCHFOUPID":
                        try { equipmenttype = xmldoc.GetElementsByTagName("EQUIPMENTTYPE")[0].ChildNodes[0].InnerText; } catch { equipmenttype = ""; }
                        try { equipmentid = xmldoc.GetElementsByTagName("EQUIPMENTID")[0].ChildNodes[0].InnerText; } catch { equipmentid = ""; }
                        try { foupid = xmldoc.GetElementsByTagName("FOUPID")[0].ChildNodes[0].InnerText; } catch { foupid = ""; }
                        changeBackgroundColor(equipmentid, equipmenttype, foupid);
                        break;
                    case "TransferStartReport":
                        try { equipmentid = xmldoc.GetElementsByTagName("STOCKNAME")[0].ChildNodes[0].InnerText; } catch { equipmentid = ""; }
                        try { foupid = xmldoc.GetElementsByTagName("CARRIERNAME")[0].ChildNodes[0].InnerText; } catch { foupid = ""; }
                        try { source = xmldoc.GetElementsByTagName("FROMLOCATION")[0].ChildNodes[0].InnerText; } catch { foupid = ""; }
                        try { destination = xmldoc.GetElementsByTagName("TOLOCATION")[0].ChildNodes[0].InnerText; } catch { foupid = ""; }
                        changeBackgroundColor(equipmentid, equipmenttype, foupid, source);
                        changeBackgroundColor(equipmentid, equipmenttype, foupid, destination);
                        break;
                    case "TransferEndReport":
                        label_TransferTimerStartTime.Text = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
                        break;
                    case "StockInitialAllInfo":
                        try
                        {
                            TimerClass.DestroyTimer();
                            AvoidTwinkle();
                            xmldoc.LoadXml(message.Trim());
                            xmlnodelist_equipment = xmldoc.SelectNodes("//STOCKNAME");
                            for (int idx_equipment = 0; idx_equipment < xmlnodelist_equipment.Count; idx_equipment++)
                            {
                                equipmentname = xmlnodelist_equipment[idx_equipment].ChildNodes[0].InnerText;
                                DataSet dsSubtype = ServiceHelper.GetService().getSubtypeBaseOnEquipmentName(equipmentname, ref errMessage);
                                foreach (DataRow datarow in dsSubtype.Tables[0].Rows) subtype = datarow["subtype"].ToString();
                                if (currentEquipment == equipmentname)
                                {
                                    xmlnodelist_location = xmldoc.SelectNodes("//LOCATION");
                                    for (int idx_location = 0; idx_location < xmlnodelist_location.Count; idx_location++)
                                    {
                                        locationid = xmlnodelist_location[idx_location].ChildNodes[0].InnerText;

                                        if (locationid == "20701")
                                        {
                                            string temp = "";
                                        }
                                        locationstatus = xmlnodelist_location[idx_location].ChildNodes[4].InnerText;
                                        if (locationid == comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_1_LOCATIONID"))
                                        {
                                            if (locationstatus.Split('#').Length > 1)
                                            {
                                                if (locationstatus.Split('#')[0].Trim() == "FULL")
                                                {
                                                    if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 0, 2, foupid, locationstatus.Split('#')[1].Trim());
                                                    else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 0, 1, foupid, locationstatus.Split('#')[1].Trim());
                                                }
                                                else if (locationstatus.Split('#')[0].Trim() == "EMPTY")
                                                {
                                                    if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 0, 2, string.Empty, locationstatus.Split('#')[1].Trim());
                                                    else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 0, 1, string.Empty, locationstatus.Split('#')[1].Trim());
                                                }
                                            }
                                            else
                                            {
                                                if (locationstatus == "FULL")
                                                {
                                                    if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 0, 2, foupid);
                                                    else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 0, 1, foupid);
                                                }
                                                else if (locationstatus == "EMPTY")
                                                {
                                                    if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 0, 2, string.Empty);
                                                    else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 0, 1, string.Empty);
                                                }
                                            }
                                        }
                                        else if (locationid == comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_2_LOCATIONID"))
                                        {
                                            if (locationstatus.Split('#').Length > 1)
                                            {
                                                if (locationstatus.Split('#')[0].Trim() == "FULL")
                                                {
                                                    if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 29, 31, foupid, locationstatus.Split('#')[1].Trim());
                                                    else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 2, 3, foupid, locationstatus.Split('#')[1].Trim());
                                                }
                                                else if (locationstatus.Split('#')[0].Trim() == "EMPTY")
                                                {
                                                    if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 29, 31, string.Empty, locationstatus.Split('#')[1].Trim());
                                                    else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 2, 3, string.Empty, locationstatus.Split('#')[1].Trim());
                                                }
                                            }
                                            else
                                            {
                                                if (locationstatus == "FULL")
                                                {
                                                    if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 29, 31, foupid);
                                                    else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 2, 3, foupid);
                                                }
                                                else if (locationstatus == "EMPTY")
                                                {
                                                    if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 29, 31, string.Empty);
                                                    else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 2, 3, string.Empty);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            location_direction = xmlnodelist_location[idx_location].ChildNodes[1].InnerText;
                                            location_x = xmlnodelist_location[idx_location].ChildNodes[2].InnerText;
                                            location_y = xmlnodelist_location[idx_location].ChildNodes[3].InnerText;
                                            locationstatus = xmlnodelist_location[idx_location].ChildNodes[4].InnerText;
                                            foupid = xmlnodelist_location[idx_location].ChildNodes[5].InnerText;
                                            columnInDataGrid = int.Parse(location_y) - 1;
                                            bool flag = false;
                                            if (subtype == "FOUP CARRIER")
                                            {
                                                if (location_direction == comm.ReadIni("CONFIG.INI", "STOCKER", "DIRECTION")) rowNumInDataGrid = dataGridViewStocker.Rows.Count / 2 + int.Parse(location_x) - 1;
                                                else rowNumInDataGrid = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "ROW")) - int.Parse(location_x);
                                                for (int idxRow = 9; idxRow <= 13; idxRow++)
                                                {
                                                    for (int idxCol = 0; idxCol <= 2; idxCol++)
                                                    {
                                                        if (idxRow == rowNumInDataGrid && idxCol == columnInDataGrid)
                                                            flag = true;
                                                    }
                                                }
                                                for (int idxRow = 9; idxRow <= 13; idxRow++)
                                                {
                                                    for (int idxCol = 29; idxCol <= 31; idxCol++)
                                                    {
                                                        if (idxRow == rowNumInDataGrid && idxCol == columnInDataGrid)
                                                            flag = true;
                                                    }
                                                }
                                            }
                                            else if (subtype == "RETICLE CARRIER")
                                            {
                                                if (location_direction == comm.ReadIni("CONFIG.INI", "STOCKER", "DIRECTION")) rowNumInDataGrid = dataGridViewStocker.Rows.Count / 2 + int.Parse(location_x) - 1;
                                                else rowNumInDataGrid = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "RETICLE_ROW")) - int.Parse(location_x);
                                                for (int idxRow = 11; idxRow <= 15; idxRow++)
                                                {
                                                    for (int idxCol = 0; idxCol <= 1; idxCol++)
                                                    {
                                                        if (idxRow == rowNumInDataGrid && idxCol == columnInDataGrid)
                                                            flag = true;
                                                    }
                                                }
                                                for (int idxRow = 11; idxRow <= 15; idxRow++)
                                                {
                                                    for (int idxCol = 2; idxCol <= 3; idxCol++)
                                                    {
                                                        if (idxRow == rowNumInDataGrid && idxCol == columnInDataGrid)
                                                            flag = true;
                                                    }
                                                }
                                            }
                                            if (!flag)
                                            {


                                                if (locationstatus == "UNAVAILABLE") dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.Red;
                                                else if (locationstatus == "FULL") dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.LightGreen;
                                                else if (locationstatus == "EMPTY")
                                                {
                                                    if (charginglocation.Contains(locationid)) dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.LightSkyBlue;
                                                    else if (n2purgelocation.Contains(locationid)) dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.LightSalmon;
                                                    else dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.White;
                                                }
                                                if (dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].ToolTipText.Split('#')[1].Trim() != foupid)
                                                {
                                                    //Bitmap bitmap = comm.conbinePictureTexts_Stocker("stocker_exist.png", locationid);
                                                    //dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Value = bitmap;
                                                    dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].ToolTipText = locationid + "#" + foupid;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        { }
                        break;
                    case "UpdateStockLocationInfo":
                        AvoidTwinkle();
                        xmldoc.LoadXml(message.Trim());
                        xmlnodelist_equipment = xmldoc.SelectNodes("//Body");
                        for (int idx_equipment = 0; idx_equipment < xmlnodelist_equipment.Count; idx_equipment++)
                        {
                            equipmentname = xmlnodelist_equipment[idx_equipment].ChildNodes[0].InnerText;
                            DataSet dsSubtype = ServiceHelper.GetService().getSubtypeBaseOnEquipmentName(equipmentname, ref errMessage);
                            foreach (DataRow datarow in dsSubtype.Tables[0].Rows) subtype = datarow["subtype"].ToString();
                            if (currentEquipment == equipmentname)
                            {
                                locationid = xmlnodelist_equipment[idx_equipment].ChildNodes[1].InnerText;
                                location_direction = xmlnodelist_equipment[idx_equipment].ChildNodes[2].InnerText;
                                location_x = xmlnodelist_equipment[idx_equipment].ChildNodes[3].InnerText;
                                location_y = xmlnodelist_equipment[idx_equipment].ChildNodes[4].InnerText;
                                locationstatus = xmlnodelist_equipment[idx_equipment].ChildNodes[5].InnerText;
                                foupid = xmlnodelist_equipment[idx_equipment].ChildNodes[6].InnerText;
                                //rowNumInDataGrid = dataGridViewStocker.Rows.Count - int.Parse(location_x);
                                if (location_direction != (int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "DIRECTION")) + 1).ToString())
                                {
                                    //if (location_direction == comm.ReadIni("CONFIG.INI", "STOCKER", "DIRECTION")) rowNumInDataGrid = dataGridViewStocker.Rows.Count - int.Parse(location_x);
                                    //if (location_direction == comm.ReadIni("CONFIG.INI", "STOCKER", "DIRECTION")) rowNumInDataGrid = int.Parse(location_x) + int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "ROW")) + 1;
                                    //else rowNumInDataGrid = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "ROW")) - int.Parse(location_x);
                                    int rowCount = 0;
                                    int colCount = 0;
                                    if (subtype == "FOUP CARRIER")
                                    {
                                        rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "ROW"));
                                        colCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "COLUMN"));
                                    }
                                    else if (subtype == "RETICLE CARRIER")
                                    {
                                        rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "RETICLE_ROW"));
                                        colCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "RETICLE_COLUMN"));
                                    }
                                    if (int.Parse(location_direction) == 1) rowNumInDataGrid = rowCount - int.Parse(location_x);
                                    else rowNumInDataGrid = int.Parse(location_x) + rowCount - 1;
                                    columnInDataGrid = int.Parse(location_y) - 1;
                                    Bitmap bitmap = comm.conbinePictureTexts_Stocker("stocker_exist.png", locationid);
                                    //if (locationstatus == "FULL") dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Value = Properties.Resources.stocker_exist;
                                    dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Value = bitmap;
                                    if (locationstatus == "UNAVAILABLE") dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.Red;
                                    if (locationstatus == "FULL") dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.LightGreen;
                                    if (locationstatus == "EMPTY") dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.White;
                                    dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].ToolTipText = locationid + "#" + foupid;
                                }
                                else
                                {
                                    if (locationid == comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_1_LOCATIONID"))
                                    {
                                        if (locationstatus.Split('#').Length > 1)
                                        {
                                            if (locationstatus.Split('#')[0].Trim() == "FULL")
                                            {
                                                if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 0, 2, foupid, locationstatus.Split('#')[1].Trim());
                                                else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 0, 1, foupid, locationstatus.Split('#')[1].Trim());
                                            }
                                            else if (locationstatus.Split('#')[0].Trim() == "EMPTY")
                                            {
                                                if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 0, 2, string.Empty, locationstatus.Split('#')[1].Trim());
                                                else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 0, 1, string.Empty, locationstatus.Split('#')[1].Trim());
                                            }
                                        }
                                        else
                                        {
                                            if (locationstatus == "FULL")
                                            {
                                                if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 0, 2, foupid);
                                                else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 0, 1, foupid);
                                            }
                                            else if (locationstatus == "EMPTY")
                                            {
                                                if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 0, 2, string.Empty);
                                                else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 0, 1, string.Empty);
                                            }
                                        }
                                    }
                                    else if (locationid == comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_2_LOCATIONID"))
                                    {
                                        if (locationstatus.Split('#').Length > 1)
                                        {
                                            if (locationstatus.Split('#')[0].Trim() == "FULL")
                                            {
                                                if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 29, 31, foupid, locationstatus.Split('#')[1].Trim());
                                                else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 2, 3, foupid, locationstatus.Split('#')[1].Trim());
                                            }
                                            else if (locationstatus.Split('#')[0].Trim() == "EMPTY")
                                            {
                                                if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 29, 31, string.Empty, locationstatus.Split('#')[1].Trim());
                                                else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 2, 3, string.Empty, locationstatus.Split('#')[1].Trim());
                                            }
                                        }
                                        else
                                        {
                                            if (locationstatus == "FULL")
                                            {
                                                if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 29, 31, foupid);
                                                else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 2, 3, foupid);
                                            }
                                            else if (locationstatus == "EMPTY")
                                            {
                                                if (subtype == "FOUP CARRIER") changeLoadPortBackColor(9, 13, 29, 31, string.Empty);
                                                else if (subtype == "RETICLE CARRIER") changeLoadPortBackColor(11, 16, 2, 3, string.Empty);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "ShowStockMessage":
                        AvoidTwinkle();
                        xmldoc.LoadXml(message.Trim());
                        xmlnodelist_equipment = xmldoc.SelectNodes("//Body");
                        for (int idx_equipment = 0; idx_equipment < xmlnodelist_equipment.Count; idx_equipment++)
                        {
                            equipmentname = xmlnodelist_equipment[idx_equipment].ChildNodes[0].InnerText;
                            equipmentstatusdescription = xmlnodelist_equipment[idx_equipment].ChildNodes[1].InnerText;
                            squipmentstatusflag = xmlnodelist_equipment[idx_equipment].ChildNodes[2].InnerText;
                            if (currentEquipment == equipmentname)
                            {
                                switch (squipmentstatusflag.Trim())
                                {
                                    case "0":
                                        //label_ERack_Equipment.ForeColor = Color.Black;
                                        delegateEvent(comm.ShowMessage(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now), equipmentname, equipmentstatusdescription));
                                        break;
                                    case "1":
                                        //label_ERack_Equipment.ForeColor = Color.Red;
                                        delegateEvent(comm.ShowAlarmMessage(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now), equipmentname, "STOCKER", equipmentstatusdescription));
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
                string errMessage = "", direction = "", location_x = "", location_y = "", location_id = "", subtype = string.Empty;
                int rowCount = 0, colCount = 0;
                DataSet locationofeachfoup = ServiceHelper.GetService().QueryLocationBaseOnFOUPID(equipmentid, equipmenttype, foupid, ref errMessage);
                DataSet dsSubtype = ServiceHelper.GetService().getSubtypeBaseOnEquipmentName(equipmentid, ref errMessage);
                foreach (DataRow datarow in dsSubtype.Tables[0].Rows) subtype = datarow["subtype"].ToString();
                if (!(locationofeachfoup.Tables[0].Rows.Count > 1 ||
                    locationofeachfoup.Tables[0].Rows.Count == 0))
                {
                    foreach (DataRow datarow in locationofeachfoup.Tables[0].Rows)
                    {
                        direction = datarow["location_direction"].ToString();
                        location_x = datarow["location_x"].ToString();
                        location_y = datarow["location_y"].ToString();
                        location_id = datarow["location_id"].ToString();
                        break;
                    }
                    if (subtype == "FOUP CARRIER")
                    {
                        rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "ROW"));
                        colCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "COLUMN"));
                    }
                    else if (subtype == "RETICLE CARRIER")
                    {
                        rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "RETICLE_ROW"));
                        colCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "RETICLE_COLUMN"));
                    }
                    int rowNumInDataGrid = 0, columnInDataGrid = 0;
                    if (int.Parse(direction) == 1) rowNumInDataGrid = rowCount - int.Parse(location_x);
                    else rowNumInDataGrid = int.Parse(location_x) + rowCount - 1;
                    columnInDataGrid = int.Parse(location_y) - 1;
                    button_RecordCurrentBackColor.BackColor = dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor;
                    //---这里要使用System.Timers.Timer，而不是System.Form.Timer
                    System.Timers.Timer timer = new System.Timers.Timer();
                    timer.Interval = double.Parse(comm.ReadIni("CONFIG.INI", "TIMER", "INTERVAL"));
                    timer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => timer_ChangeBackColor_Tick(sender, e, dataGridViewStocker, rowNumInDataGrid, columnInDataGrid, timer));
                    timer.Start();
                    label_TimerStartTime.Text = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now.AddSeconds(double.Parse(comm.ReadIni("CONFIG.INI", "TIMER", "COUNTTOSTOP"))));
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
        private void changeBackgroundColor(string equipmentid, string equipmenttype, string foupid, string location)
        {
            CommonFunction comm = new CommonFunction();
            try
            {
                if (currentEquipment == equipmentid)
                {
                    string errMessage = "", direction = "", location_x = "", location_y = "", location_id = "", subtype = string.Empty;
                    int rowCount = 0, colCount = 0;
                    DataSet dsSubtype = ServiceHelper.GetService().getSubtypeBaseOnEquipmentName(equipmentid, ref errMessage);
                    foreach (DataRow datarow in dsSubtype.Tables[0].Rows) subtype = datarow["subtype"].ToString();
                    if (location != string.Empty && equipmentid != string.Empty)
                    {
                        direction = location.Substring(0, 1);
                        location_x = location.Substring(1, 2);
                        location_y = location.Substring(3, 2);
                        location_id = location;
                        if (subtype == "FOUP CARRIER")
                        {
                            rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "ROW"));
                            colCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "COLUMN"));
                        }
                        else if (subtype == "RETICLE CARRIER")
                        {
                            rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "RETICLE_ROW"));
                            colCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "RETICLE_COLUMN"));
                        }
                        int rowNumInDataGrid = 0, columnInDataGrid = 0;
                        if (direction == "3")
                        {
                            rowNumInDataGrid = int.Parse(location_id);
                            columnInDataGrid = int.Parse(location_id);
                        }
                        else
                        {
                            if (int.Parse(direction) == 1) rowNumInDataGrid = rowCount - int.Parse(location_x);
                            else rowNumInDataGrid = int.Parse(location_x) + rowCount - 1;
                            columnInDataGrid = int.Parse(location_y) - 1;
                            button_RecordCurrentBackColorTransfer.BackColor = dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor;
                        }
                        //---这里要使用System.Timers.Timer，而不是System.Form.Timer
                        System.Timers.Timer timer = new System.Timers.Timer();
                        timer.Interval = double.Parse(comm.ReadIni("CONFIG.INI", "TRANSFERTIMER", "INTERVAL"));
                        timer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => transferTimer_ChangeBackColor_Tick(sender, e, dataGridViewStocker, rowNumInDataGrid, columnInDataGrid, timer, currentEquipment, location_id));
                        timer.Start();
                        label_TransferTimerStartTime.Text = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now.AddSeconds(double.Parse(comm.ReadIni("CONFIG.INI", "TRANSFERTIMER", "COUNTTOSTOP"))));
                    }
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
                //解决DataGridView更新闪烁问题
                Type type = datagridview.GetType();
                PropertyInfo propertyinfo = type.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                propertyinfo.SetValue(datagridview, true, null);
                if (dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor == button_RecordCurrentBackColor.BackColor)
                {
                    dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = button_TipColor.BackColor;
                }
                else
                {
                    dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = button_RecordCurrentBackColor.BackColor;
                }
                if (string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) == label_TimerStartTime.Text)
                {
                    timer.Stop();
                    dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = button_RecordCurrentBackColor.BackColor;
                    message = comm.clearTextBoxSearchFOUP();
                    delegateEvent(message);
                }
            }
            catch (Exception ex)
            {
                timer.Stop();
                CommonFunction comm = new CommonFunction();
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        private void transferTimer_ChangeBackColor_Tick(object sender, System.Timers.ElapsedEventArgs e, DataGridView datagridview, int rowNumInDataGrid, int columnInDataGrid, System.Timers.Timer timer, string equipment, string locationid)
        {
            try
            {
                CommonFunction comm = new CommonFunction();
                string message = string.Empty;
                string subtype = string.Empty;
                string errMessage = string.Empty;
                string foupid = string.Empty;
                DataSet dsSubtype = ServiceHelper.GetService().getSubtypeBaseOnEquipmentName(currentEquipment, ref errMessage);
                foreach (DataRow datarow in dsSubtype.Tables[0].Rows) subtype = datarow["subtype"].ToString();
                dsSubtype = ServiceHelper.GetService().getFOUPInfo(equipment, locationid, ref errMessage);
                foreach (DataRow datarow in dsSubtype.Tables[0].Rows) foupid = datarow["foupid"].ToString();
                //解决DataGridView更新闪烁问题
                Type type = datagridview.GetType();
                PropertyInfo propertyinfo = type.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                propertyinfo.SetValue(datagridview, true, null);
                if (locationid.ToString() == comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_1_LOCATIONID"))
                {
                    if (subtype == "FOUP CARRIER")
                    {
                        if (dataGridViewStocker.Rows[9].Cells[0].Style.BackColor == Color.GhostWhite)
                            changeLoadPortBackColorforTransferTimer(9, 13, 0, 2, button_TransferTimerColor.BackColor);
                        else changeLoadPortBackColorforTransferTimer(9, 13, 0, 2, Color.GhostWhite);
                        if (double.Parse(string.Format("{0:yyyyMMddHHmmss}", DateTime.Now)) > double.Parse(label_TransferTimerStartTime.Text))
                        {
                            timer.Stop();
                            //changeLoadPortBackColorforTransferTimer(9, 13, 0, 2, Color.GhostWhite);
                            if (foupid.Trim() != string.Empty) changeLoadPortBackColorforTransferTimer(9, 13, 0, 2, Color.Yellow);
                            else changeLoadPortBackColorforTransferTimer(9, 13, 0, 2, Color.GhostWhite);
                        }
                    }
                    else if (subtype == "RETICLE CARRIER")
                    {
                        if (dataGridViewStocker.Rows[11].Cells[0].Style.BackColor == Color.GhostWhite)
                            changeLoadPortBackColorforTransferTimer(11, 16, 0, 1, button_TransferTimerColor.BackColor);
                        else changeLoadPortBackColorforTransferTimer(11, 16, 0, 1, Color.GhostWhite);
                        if (double.Parse(string.Format("{0:yyyyMMddHHmmss}", DateTime.Now)) > double.Parse(label_TransferTimerStartTime.Text))
                        {
                            timer.Stop();
                            //changeLoadPortBackColorforTransferTimer(11, 16, 0, 1, Color.GhostWhite);
                            if (foupid.Trim() != string.Empty) changeLoadPortBackColorforTransferTimer(11, 16, 0, 1, Color.Yellow);
                            else changeLoadPortBackColorforTransferTimer(11, 16, 0, 1, Color.GhostWhite);
                        }
                    }
                }
                else if (locationid.ToString() == comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_2_LOCATIONID"))
                {
                    if (subtype == "FOUP CARRIER")
                    {
                        if (dataGridViewStocker.Rows[9].Cells[29].Style.BackColor == Color.GhostWhite)
                            changeLoadPortBackColorforTransferTimer(9, 13, 29, 31, button_TransferTimerColor.BackColor);
                        else changeLoadPortBackColorforTransferTimer(9, 13, 29, 31, Color.GhostWhite);
                        if (double.Parse(string.Format("{0:yyyyMMddHHmmss}", DateTime.Now)) > double.Parse(label_TransferTimerStartTime.Text))
                        {
                            timer.Stop();
                            //changeLoadPortBackColorforTransferTimer(9, 13, 29, 31, Color.GhostWhite);
                            if (foupid.Trim() != string.Empty) changeLoadPortBackColorforTransferTimer(9, 13, 29, 31, Color.Yellow);
                            else changeLoadPortBackColorforTransferTimer(9, 13, 29, 31, Color.GhostWhite);
                        }
                    }
                    else if (subtype == "RETICLE CARRIER")
                    {
                        if (dataGridViewStocker.Rows[11].Cells[2].Style.BackColor == Color.GhostWhite)
                            changeLoadPortBackColorforTransferTimer(11, 16, 2, 3, button_TransferTimerColor.BackColor);
                        else changeLoadPortBackColorforTransferTimer(11, 16, 2, 3, Color.GhostWhite);
                        if (double.Parse(string.Format("{0:yyyyMMddHHmmss}", DateTime.Now)) > double.Parse(label_TransferTimerStartTime.Text))
                        {
                            timer.Stop();
                            //changeLoadPortBackColorforTransferTimer(11, 16, 2, 3, Color.GhostWhite);
                            if (foupid.Trim() != string.Empty) changeLoadPortBackColorforTransferTimer(11, 16, 2, 3, Color.Yellow);
                            else changeLoadPortBackColorforTransferTimer(11, 16, 2, 3, Color.GhostWhite);
                        }
                    }
                }
                else
                {
                    string locationStatus = string.Empty;
                    string strResult = string.Empty;
                    DataSet dsLocationStatus = ServiceHelper.GetService().getLocationStatus(equipment, locationid, ref strResult);
                    foreach (DataRow datarow in dsLocationStatus.Tables[0].Rows) locationStatus = datarow["statuscode"].ToString();
                    //if (locationStatus == "0") button_RecordCurrentBackColorTransfer.BackColor = Color.LightGreen;
                    if (locationStatus == "1") button_RecordCurrentBackColorTransfer.BackColor = Color.Red;
                    if (locationStatus == "2") button_RecordCurrentBackColorTransfer.BackColor = Color.LightSalmon;
                    if (locationStatus == "3") button_RecordCurrentBackColorTransfer.BackColor = Color.LightSkyBlue;
                    if (dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor == button_RecordCurrentBackColorTransfer.BackColor)
                        dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = button_TransferTimerColor.BackColor;
                    else
                        dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = button_RecordCurrentBackColorTransfer.BackColor;
                    if (double.Parse(string.Format("{0:yyyyMMddHHmmss}", DateTime.Now)) > double.Parse(label_TransferTimerStartTime.Text))
                    {
                        timer.Stop();
                        //if (foupid.Trim() != string.Empty)
                        if (dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].ToolTipText.Split('#')[1].Trim() != string.Empty)
                            dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.LightGreen;
                        else
                        {
                            if (locationStatus == "1") dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.Red;
                            else if (locationStatus == "2") dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.LightSalmon;
                            else if (locationStatus == "3") dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.LightSkyBlue;
                            else dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.White;
                        }
                        //if (locationStatus == "0") dataGridViewStocker.Rows[rowNumInDataGrid].Cells[columnInDataGrid].Style.BackColor = Color.LightGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                timer.Stop();
                CommonFunction comm = new CommonFunction();
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        private void dataGridViewStocker_Paint(object sender, PaintEventArgs e)
        {
            //int rowCount = 0;
            //int columnLength = 0;
            //string subtype = string.Empty;
            //string errMessage = string.Empty;
            //DataSet dsSubtype = ServiceHelper.GetService().getSubtypeBaseOnEquipmentName(currentEquipment, ref errMessage);
            //foreach (DataRow datarow in dsSubtype.Tables[0].Rows) subtype = datarow["subtype"].ToString();
            //rowCount = this.dataGridViewStocker.RowCount / 2;
            //if (subtype == "FOUP CARRIER") columnLength = this.dataGridViewStocker.Width;
            //else if (subtype == "RETICLE CARRIER") columnLength = this.dataGridViewStocker.CurrentCell.Size.Width * this.dataGridViewStocker.ColumnCount;
            //for (int idx = 1; idx <= 3; idx++) e.Graphics.DrawLine(Pens.Magenta, 0, this.dataGridViewStocker.CurrentCell.Size.Height * rowCount + idx, columnLength, this.dataGridViewStocker.CurrentCell.Size.Height * rowCount + idx);
        }
        private void dataGridViewStocker_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
            e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
        }
    }
}