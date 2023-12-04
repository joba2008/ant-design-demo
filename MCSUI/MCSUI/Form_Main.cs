using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;
using TIBCO.Rendezvous;
using System.Configuration;
using System.Threading;
using System.IO;

namespace MCSUI
{
    public partial class Form_Main : Form
    {
        public string functionName = string.Empty;
        private delegate void InvokeUpdateState(string message);
        private delegate void InvokeUpdateStateWithoutParameter();
        public delegate void delegateSetting(string message);
        public delegate void delegateRecordUserID(string message);
        //public static event delegateSetting delegateAllEquipmentEvent;
        public static event delegateSetting delegateStockerEvent;
        public static event delegateSetting delegateERackEvent;
        Form_ProgressBar progressBar = new Form_ProgressBar();
        public Form_Main()
        {
            InitializeComponent();
        }
        public Form_Main(string userid)
        {
            InitializeComponent();
            textBox_Display_LoginUser.Text = userid.Trim();
            tabControl_Main.SizeMode = TabSizeMode.Fixed;
            tabControl_Main.ItemSize = new Size(120, 25);
            tabControl_Config.SizeMode = TabSizeMode.Fixed;
            tabControl_Config.ItemSize = new Size(120, 25);
            MyMessager msg = new MyMessager();
            Application.AddMessageFilter(msg);
            timer1.Start();
        }
        private void DoWork_Handler(object sender, DoWorkEventArgs args)
        {
            try
            {
                int progressbarvalue = 0;
                int intervl = 0;
                intervl = 100 / int.Parse(args.Argument.ToString()) + 1;
                if (GlobalParameter.ProgressBarFlag)
                {
                    BackgroundWorker worker = sender as BackgroundWorker;
                    for (int idx = 1; idx <= int.Parse(args.Argument.ToString()); idx++)
                    {
                        if (worker.CancellationPending)
                        {
                            args.Cancel = true;
                            break;
                        }
                        else
                        {
                            if (idx * intervl > 100) progressbarvalue = 100;
                            else progressbarvalue = idx * intervl;
                            worker.ReportProgress(progressbarvalue);
                            Thread.Sleep(500);
                        }
                    }
                }
                GlobalParameter.ProgressBarFlag = false;
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
        private void ProcessChanged_Handler(object sender, ProgressChangedEventArgs e)
        {
            progressBar.SetValue(e.ProgressPercentage);
        }
        private void RunWorkerCompleted_Handler(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Close();
        }
        private void getFOUPIDLcationRelation(string equipmentType, string equipmentID)
        {
            try
            {
                string errMessage = string.Empty;
                //DataSet FoupExistOrNot = ServiceHelper.GetService().CheckFoupExistOrNot(comboBox_DisplayTransfer_EquipmentType.Text,
                //                                                                        comboBox_DisplayTransfer_EquipmentID.Text,
                //                                                                        ref errMessage);
                DataSet FoupExistOrNot = ServiceHelper.GetService().CheckFoupExistOrNot(equipmentType,
                                                                                        equipmentID,
                                                                                        ref errMessage);
                GlobalParameter.LocationFoupIDMapping.Clear();
                foreach (DataRow datarow in FoupExistOrNot.Tables[0].Rows)
                {
                    GlobalParameter.recordInfoBaseOnLocation recordinfobaseonlocation = new GlobalParameter.recordInfoBaseOnLocation();
                    recordinfobaseonlocation.location_id = datarow["location_id"].ToString();
                    recordinfobaseonlocation.foup_id = datarow["foupid"].ToString();
                    recordinfobaseonlocation.location_available = datarow["available"].ToString();
                    try
                    {
                        string temp = string.Empty;
                        if (datarow["portstatus"].ToString() == "0") temp = "PB";
                        else if (datarow["portstatus"].ToString() == "1") temp = "PL";
                        else if (datarow["portstatus"].ToString() == "2") temp = "PU";
                        recordinfobaseonlocation.port_status = temp;
                    }
                    catch
                    {
                        recordinfobaseonlocation.port_status = string.Empty;
                    }
                    GlobalParameter.LocationFoupIDMapping.Add(datarow["location_id"].ToString(), recordinfobaseonlocation);
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
        private void getLocationinfoBaseOnEquipment(string EquipmentType)
        {
            try
            {
                TabPage tabpage = new TabPage();
                CommonFunction comm = new CommonFunction();
                if (EquipmentType == "STOCKER")
                {
                    getFOUPIDLcationRelation(comboBox_DisplayTransfer_EquipmentType.Text, comboBox_DisplayTransfer_EquipmentID.Text);
                    MCSUI.Display.StockerDisplay stockerdisplay = new MCSUI.Display.StockerDisplay(GlobalParameter.LocationFoupIDMapping, comboBox_DisplayTransfer_EquipmentID.Text);
                    stockerdisplay.delegateEvent += new MCSUI.Display.StockerDisplay.delegateSetting(setting);
                    stockerdisplay.Dock = DockStyle.Fill;
                    tabpage.Controls.Add(stockerdisplay);
                    tabControlDisplayTransfer_DisplayTransfer.TabPages.Add(tabpage);
                }
                else if (EquipmentType == "ERACK")
                {
                    int counter1 = 0;
                    int counter2 = 0;
                    int displayboardcolumncount = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "DISPLAYBOARD_COLUMN_COUNT"));
                    int distancebetweentwoitem = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "DISTANCE_BETWEEN_TWO_ITEM"));
                    showProcessBar();
                    foreach (var item in comboBox_DisplayTransfer_EquipmentID.Items)
                    {
                        if (counter1 % displayboardcolumncount == 0 && counter1 >= displayboardcolumncount) counter1 = 0;
                        getFOUPIDLcationRelation(comboBox_DisplayTransfer_EquipmentType.Text, item.ToString());
                        MCSUI.Display.ERackDisplay erackdisplay = new MCSUI.Display.ERackDisplay(GlobalParameter.LocationFoupIDMapping, item.ToString());
                        erackdisplay.delegateEvent += new MCSUI.Display.ERackDisplay.delegateSetting(setting);
                        erackdisplay.Left = counter1 * erackdisplay.Size.Width + counter1 * distancebetweentwoitem;
                        erackdisplay.Top = Decimal.ToInt32(Math.Floor(Convert.ToDecimal(counter2) / displayboardcolumncount)) * erackdisplay.Size.Height +
                                           distancebetweentwoitem * Decimal.ToInt32(Math.Floor(Convert.ToDecimal(counter2) / displayboardcolumncount)) * 2;
                        tabpage.Controls.Add(erackdisplay);
                        counter1++;
                        counter2++;
                    }
                    tabControlDisplayTransfer_DisplayTransfer.TabPages.Add(tabpage);
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
        private void getEquipmentID(int selectindex, bool addTagALL)
        {
            CommonFunction comm = new CommonFunction();
            try
            {
                string errMessage = string.Empty;
                comboBox_DisplayTransfer_EquipmentID.Items.Clear();
                if (comboBox_DisplayTransfer_EquipmentType.Text.ToUpper() == "ALL")
                {
                    comboBox_DisplayTransfer_EquipmentID.Items.Add("ALL");
                    comboBox_DisplayTransfer_EquipmentID.SelectedIndex = selectindex;
                    //---
                    TabPage tabpage = new TabPage();
                    MCSUI.Display.AllEquipmentDisplay allequipmentdisplay = new MCSUI.Display.AllEquipmentDisplay();
                    allequipmentdisplay.Dock = DockStyle.Fill;
                    tabpage.Controls.Add(allequipmentdisplay);
                    tabControlDisplayTransfer_DisplayTransfer.TabPages.Add(tabpage);
                }
                else if (comboBox_DisplayTransfer_EquipmentType.Text.ToUpper() == "STOCKER")
                {
                    DataSet alleqpid = ServiceHelper.GetService().GetAllEqpID(
                        comboBox_DisplayTransfer_EquipmentType.Text.ToUpper(), ref errMessage);
                    if (addTagALL) comboBox_DisplayTransfer_EquipmentID.Items.Add("ALL");
                    foreach (DataRow datarow in alleqpid.Tables[0].Rows) comboBox_DisplayTransfer_EquipmentID.Items.Add(datarow["name"].ToString());
                    comboBox_DisplayTransfer_EquipmentID.SelectedIndex = selectindex;
                    getFOUPIDLcationRelation(comboBox_DisplayTransfer_EquipmentType.Text, comboBox_DisplayTransfer_EquipmentID.Text);
                    TabPage tabpage = new TabPage();
                    MCSUI.Display.StockerDisplay stockerdisplay = new MCSUI.Display.StockerDisplay(GlobalParameter.LocationFoupIDMapping, comboBox_DisplayTransfer_EquipmentID.Text);
                    stockerdisplay.delegateEvent += new MCSUI.Display.StockerDisplay.delegateSetting(setting);
                    stockerdisplay.Dock = DockStyle.Fill;
                    tabpage.Controls.Add(stockerdisplay);
                    tabControlDisplayTransfer_DisplayTransfer.TabPages.Add(tabpage);
                }
                else
                {
                    DataSet alleqpid = ServiceHelper.GetService().GetAllEqpID(
                        comboBox_DisplayTransfer_EquipmentType.Text.ToUpper(), ref errMessage);
                    if (addTagALL) comboBox_DisplayTransfer_EquipmentID.Items.Add("ALL");
                    foreach (DataRow datarow in alleqpid.Tables[0].Rows) comboBox_DisplayTransfer_EquipmentID.Items.Add(datarow["name"].ToString());
                    GlobalParameter.ProgressBarFlag = true;
                    comboBox_DisplayTransfer_EquipmentID.SelectedIndex = selectindex;
                    getFOUPIDLcationRelation(comboBox_DisplayTransfer_EquipmentType.Text, comboBox_DisplayTransfer_EquipmentID.Text);
                    TabPage tabpage = new TabPage();
                    MCSUI.Display.ERackDisplay erackdisplay = new MCSUI.Display.ERackDisplay(GlobalParameter.LocationFoupIDMapping, comboBox_DisplayTransfer_EquipmentID.Text);
                    erackdisplay.delegateEvent += new MCSUI.Display.ERackDisplay.delegateSetting(setting);
                    //erackdisplay.Dock = DockStyle.Fill;
                    tabpage.Controls.Add(erackdisplay);
                    tabControlDisplayTransfer_DisplayTransfer.TabPages.Add(tabpage);
                }
                comboBox_DisplayTransfer_EquipmentID.Refresh();
            }
            catch (Exception ex)
            {
                comm = new CommonFunction();
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        private void getEquipmentID_AlarmRecord(int selectindex, bool addTagALL)
        {
            try
            {
                string errMessage = string.Empty;
                comboBox_AlarmRecord_EquipmentID.Items.Clear();
                if (comboBox_AlarmRecord_EquipmentType.Text.ToUpper() == "ALL")
                {
                    comboBox_AlarmRecord_EquipmentID.Items.Add("ALL");
                    comboBox_AlarmRecord_EquipmentID.SelectedIndex = selectindex;
                }
                else if (comboBox_AlarmRecord_EquipmentType.Text.ToUpper() == "STOCKER")
                {
                    DataSet alleqpid = ServiceHelper.GetService().GetAllEqpID(
                        comboBox_AlarmRecord_EquipmentType.Text.ToUpper(), ref errMessage);
                    if (addTagALL)
                    {
                        comboBox_AlarmRecord_EquipmentID.Items.Add("ALL");
                    }
                    foreach (DataRow datarow in alleqpid.Tables[0].Rows)
                    {
                        comboBox_AlarmRecord_EquipmentID.Items.Add(datarow["name"].ToString());
                    }
                    comboBox_AlarmRecord_EquipmentID.SelectedIndex = selectindex;
                }
                else
                {
                    DataSet alleqpid = ServiceHelper.GetService().GetAllEqpID(
                        comboBox_AlarmRecord_EquipmentType.Text.ToUpper(), ref errMessage);
                    if (addTagALL)
                    {
                        comboBox_AlarmRecord_EquipmentID.Items.Add("ALL");
                    }
                    foreach (DataRow datarow in alleqpid.Tables[0].Rows)
                    {
                        comboBox_AlarmRecord_EquipmentID.Items.Add(datarow["name"].ToString());
                    }
                    comboBox_AlarmRecord_EquipmentID.SelectedIndex = selectindex;
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
        private void getEquipmentID_FOUPRecord(int selectindex, bool addTagALL)
        {
            try
            {
                string errMessage = string.Empty;
                comboBox_FOUPRecord_EquipmentID.Items.Clear();
                if (comboBox_FOUPRecord_EquipmentType.Text.ToUpper() == "ALL")
                {
                    comboBox_FOUPRecord_EquipmentID.Items.Add("ALL");
                    comboBox_FOUPRecord_EquipmentID.SelectedIndex = selectindex;
                }
                else if (comboBox_FOUPRecord_EquipmentType.Text.ToUpper() == "STOCKER")
                {
                    DataSet alleqpid = ServiceHelper.GetService().GetAllEqpID(
                        comboBox_FOUPRecord_EquipmentType.Text.ToUpper(), ref errMessage);
                    if (addTagALL)
                    {
                        comboBox_FOUPRecord_EquipmentID.Items.Add("ALL");
                    }
                    foreach (DataRow datarow in alleqpid.Tables[0].Rows)
                    {
                        comboBox_FOUPRecord_EquipmentID.Items.Add(datarow["name"].ToString());
                    }
                    comboBox_FOUPRecord_EquipmentID.SelectedIndex = selectindex;
                }
                else
                {
                    DataSet alleqpid = ServiceHelper.GetService().GetAllEqpID(
                        comboBox_FOUPRecord_EquipmentType.Text.ToUpper(), ref errMessage);
                    if (addTagALL)
                    {
                        comboBox_FOUPRecord_EquipmentID.Items.Add("ALL");
                    }
                    foreach (DataRow datarow in alleqpid.Tables[0].Rows)
                    {
                        comboBox_FOUPRecord_EquipmentID.Items.Add(datarow["name"].ToString());
                    }
                    comboBox_FOUPRecord_EquipmentID.SelectedIndex = selectindex;
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
        private void getEquipmentType(int selectindex, bool addTagALL)
        {
            try
            {
                string errMessage = string.Empty;
                comboBox_DisplayTransfer_EquipmentType.Items.Clear();
                comboBox_DisplayTransfer_EquipmentID.Items.Clear();
                DataSet allracktype = ServiceHelper.GetService().GetAllRackType(ref errMessage);
                if (addTagALL)
                {
                    comboBox_DisplayTransfer_EquipmentType.Items.Add("ALL");
                }
                foreach (DataRow datarow in allracktype.Tables[0].Rows)
                {
                    comboBox_DisplayTransfer_EquipmentType.Items.Add(datarow["type"].ToString());
                }
                comboBox_DisplayTransfer_EquipmentType.SelectedIndex = selectindex;
                comboBox_DisplayTransfer_EquipmentType.Refresh();
                if (tabControlDisplayTransfer_DisplayTransfer.TabPages.Count > 0)
                    tabControlDisplayTransfer_DisplayTransfer.TabPages.RemoveAt(0);
                getEquipmentID(selectindex, addTagALL);
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
        private void getEquipmentType_AlarmRecord(int selectindex, bool addTagALL)
        {
            try
            {
                string errMessage = string.Empty;
                comboBox_AlarmRecord_EquipmentType.Items.Clear();
                comboBox_AlarmRecord_EquipmentID.Items.Clear();
                DataSet allracktype = ServiceHelper.GetService().GetAllRackType(ref errMessage);
                if (addTagALL)
                {
                    comboBox_AlarmRecord_EquipmentType.Items.Add("ALL");
                }
                foreach (DataRow datarow in allracktype.Tables[0].Rows)
                {
                    comboBox_AlarmRecord_EquipmentType.Items.Add(datarow["type"].ToString());
                }
                comboBox_AlarmRecord_EquipmentType.SelectedIndex = selectindex;
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
        private void getEquipmentType_FOUPRecord(int selectindex, bool addTagALL)
        {
            try
            {
                string errMessage = string.Empty;
                comboBox_FOUPRecord_EquipmentType.Items.Clear();
                comboBox_FOUPRecord_EquipmentID.Items.Clear();
                DataSet allracktype = ServiceHelper.GetService().GetAllRackType(ref errMessage);
                if (addTagALL)
                {
                    comboBox_FOUPRecord_EquipmentType.Items.Add("ALL");
                }
                foreach (DataRow datarow in allracktype.Tables[0].Rows)
                {
                    comboBox_FOUPRecord_EquipmentType.Items.Add(datarow["type"].ToString());
                }
                comboBox_FOUPRecord_EquipmentType.SelectedIndex = selectindex;
                getEquipmentID_FOUPRecord(selectindex, addTagALL);
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
        private void setTimePicker()
        {
            try
            {
                dateTimePicker_FOUPRecord_Begin.Value = DateTime.Now.AddDays(-7);
                dateTimePicker_FOUPRecord_End.Value = DateTime.Now;
                dateTimePicker_Transfer_Begin.Value = DateTime.Now.AddDays(-7);
                dateTimePicker_Transfer_End.Value = DateTime.Now;
                dateTimePicker_N2Purge_Begin.Value = DateTime.Now.AddDays(-7);
                dateTimePicker_N2Purge_End.Value = DateTime.Now;
                dateTimePicker_TaskInfo_Begin.Value = DateTime.Now.AddDays(-1);
                dateTimePicker_TaskInfo_End.Value = DateTime.Now;
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
        private void Form_Main_Load(object sender, EventArgs e)
        {
            try
            {
                string errMessage = string.Empty;
                var message_field = "";
                var subject = "";
                var network = "";
                var port = "";
                var deamon = "";
                string[] deamonlist = null;
                Transport transport = null;
                CommonFunction comm = new CommonFunction();
                //--- Check Authority
                DataSet allFunction = ServiceHelper.GetService().getFunctionBaseOnUserName(textBox_Display_LoginUser.Text, ref errMessage);
                foreach (DataRow datarow in allFunction.Tables[0].Rows)
                {
                    if (functionName == "") functionName = datarow["function"].ToString();
                    else functionName = functionName + "#" + datarow["function"].ToString();
                }
                if (!functionName.Contains("CREATE TRANSFER JOB")) { checkBox_DisplayTransfer_Transfer.Visible = false; }
                if (!functionName.Contains("EQUIPMENT MANAGEMENT")) { tabControl_Config.TabPages.RemoveAt(0); }
                if (!functionName.Contains("AUTHORITY MANAGEMENT"))
                {
                    checkedListBox_Authority.Items.Clear();
                    checkedListBox_Authority.Items.Add("Personal Setting");
                }
                //--- Update Title
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fileversioninfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                string fileversion = fileversioninfo.FileVersion;
                this.Text = " MCS Client, Version " + fileversion;
                //--- Initial Time Picker
                setTimePicker();
                //---
                getEquipmentType(0, true);
                button_DisplayTransfer_Clear.Focus();
                checkBox_DisplayTransfer_ShowEquipmentStatus.Checked = true;
                checkBox_DisplayTransfer_ShowLocation.Checked = false;
                groupBox_DisplayTransfer_Start.Visible = false;
                groupBox_SearchFOUP.Visible = false;
                comboBox_DisplayTransfer_EquipmentType.Enabled = false;
                comboBox_DisplayTransfer_EquipmentID.Enabled = false;
                checkBox_DisplayTransfer_Transfer.Enabled = false;
                //--- Initial Config Tabpage
                QueryBasicSetting();
                //--- Initial Event Handler
                BackGroundWorker.WorkerReportsProgress = true;
                BackGroundWorker.WorkerSupportsCancellation = true;
                BackGroundWorker.DoWork += DoWork_Handler;
                BackGroundWorker.ProgressChanged += ProcessChanged_Handler;
                BackGroundWorker.RunWorkerCompleted += RunWorkerCompleted_Handler;
                //--- Liten message from EAP
                TIBCO.Rendezvous.Environment.Open();
                message_field = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "MESSAGE_FIELD");
                subject = comm.ReadIni("CONFIG.INI", "TIBCORV_FROM_EAP", "SUBJECT");
                network = comm.ReadIni("CONFIG.INI", "TIBCORV_FROM_EAP", "NETWORK");
                port = comm.ReadIni("CONFIG.INI", "TIBCORV_FROM_EAP", "PORT");
                deamon = comm.ReadIni("CONFIG.INI", "TIBCORV_FROM_EAP", "DEAMON");
                deamonlist = deamon.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                transport = CreateNetPort(port, network, deamonlist);
                //Listener listenerq = new Listener(Queue.Default, transport, subject, new object());
                //use inbox, modify by chunwei, 20190909
                Listener listenerq = new Listener(Queue.Default, transport, subject, transport.CreateInbox());
                listenerq.MessageReceived += new MessageReceivedEventHandler(HandleMessageFromEAP);
                var dispacher = new Dispatcher(listenerq.Queue);
                dispacher.Resume();
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
        private void ClearAlarmRecord()
        {
            //--- It is not work for 'dataGridView_AlarmRecord_Display.DataSource'
            //while (dataGridView_AlarmRecord_Display.Rows.Count != 0)
            //{
            //    dataGridView_AlarmRecord_Display.Rows.RemoveAt(0);
            //}
            //---
            dataGridView_AlarmRecord_Display.DataSource = null;
        }
        private void ClearFOUPRecord()
        {
            //--- It is not work for 'dataGridView_FOUPRecord_Display.DataSource'
            //while (dataGridView_FOUPRecord_Display.Rows.Count != 0)
            //{
            //    dataGridView_FOUPRecord_Display.Rows.RemoveAt(0);
            //}
            //--- It is not work for 'dataGridView_FOUPRecord_Display.DataSource'
            //dataGridView_FOUPRecord_Display.Rows.Clear();
            //---
            dataGridView_FOUPRecord_Display.DataSource = null;
        }
        private void ClearTransferRecord()
        {
            while (dataGridView_TransferRecord_Display.Rows.Count != 0)
            {
                dataGridView_TransferRecord_Display.Rows.RemoveAt(0);
            }
        }
        private void ClearN2PurgeRecord()
        {
            while (dataGridView_N2Purge.Rows.Count != 0)
            {
                dataGridView_N2Purge.Rows.RemoveAt(0);
            }
        }
        private void ClearBasicSetting()
        {
            dataGridView_Config_ShowSomething.DataSource = null;
        }
        private void ClearTaskInfo()
        {
            dataGridView_TaskInfo.DataSource = null;
        }
        private void QueryAlarmRecord(string type, string name)
        {
            try
            {
                //int rowcount = 0;
                string errMessage = string.Empty;
                DataSet allAlarmRecord = ServiceHelper.GetService().queryAlarmRecord(type, name, ref errMessage);
                dataGridView_AlarmRecord_Display.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                ClearAlarmRecord();
                //--- Loading data to DataGridView is too slow
                //foreach (DataRow datarow in allAlarmRecord.Tables[0].Rows)
                //{
                //    dataGridView_AlarmRecord_Display.Rows.Add();
                //    dataGridView_AlarmRecord_Display.Rows[rowcount].Cells[0].Value = datarow["update_time"];
                //    dataGridView_AlarmRecord_Display.Rows[rowcount].Cells[1].Value = datarow["type"];
                //    dataGridView_AlarmRecord_Display.Rows[rowcount].Cells[2].Value = datarow["name"];
                //    dataGridView_AlarmRecord_Display.Rows[rowcount].Cells[3].Value = datarow["location_id"];
                //    dataGridView_AlarmRecord_Display.Rows[rowcount].Cells[4].Value = datarow["alarm"];
                //    dataGridView_AlarmRecord_Display.Rows[rowcount].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //    dataGridView_AlarmRecord_Display.Rows[rowcount].Height = 500;
                //    rowcount++;
                //}
                //--- Batter method
                dataGridView_AlarmRecord_Display.DataSource = allAlarmRecord.Tables[0];
                dataGridView_AlarmRecord_Display.DefaultCellStyle.BackColor = Color.LightGray;
                dataGridView_AlarmRecord_Display.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView_AlarmRecord_Display.Columns[0].HeaderCell.Value = "Update Time";
                dataGridView_AlarmRecord_Display.Columns[1].HeaderCell.Value = "Equipment Type";
                dataGridView_AlarmRecord_Display.Columns[2].HeaderCell.Value = "Equipment Name";
                dataGridView_AlarmRecord_Display.Columns[3].HeaderCell.Value = "Alarm";
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
        private void QueryFOUPCurrentRecord(string type, string name)
        {
            try
            {
                //int rowcount = 0;
                string errMessage = string.Empty;
                DataSet allFOUPRecord = ServiceHelper.GetService().queryFOUPCurrentRecord(type, name, ref errMessage);
                dataGridView_FOUPRecord_Display.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                ClearFOUPRecord();
                //--- Loading data to DataGridView is too slow
                //foreach (DataRow datarow in allFOUPRecord.Tables[0].Rows)
                //{
                //    dataGridView_FOUPRecord_Display.Rows.Add();
                //    dataGridView_FOUPRecord_Display.Rows[rowcount].Cells[0].Value = datarow["foupid"];
                //    dataGridView_FOUPRecord_Display.Rows[rowcount].Cells[1].Value = datarow["type"];
                //    dataGridView_FOUPRecord_Display.Rows[rowcount].Cells[2].Value = datarow["subtype"];
                //    dataGridView_FOUPRecord_Display.Rows[rowcount].Cells[3].Value = datarow["name"];
                //    dataGridView_FOUPRecord_Display.Rows[rowcount].Cells[4].Value = datarow["location_id"];
                //    dataGridView_FOUPRecord_Display.Rows[rowcount].Cells[5].Value = datarow["update_time"];
                //    dataGridView_FOUPRecord_Display.Rows[rowcount].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //    dataGridView_FOUPRecord_Display.Rows[rowcount].Height = 500;
                //    rowcount++;
                //}
                //--- Batter method
                dataGridView_FOUPRecord_Display.DataSource = allFOUPRecord.Tables[0];
                dataGridView_FOUPRecord_Display.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView_FOUPRecord_Display.DefaultCellStyle.BackColor = Color.White;
                dataGridView_FOUPRecord_Display.Columns[0].HeaderCell.Value = "FOUP ID";
                dataGridView_FOUPRecord_Display.Columns[1].HeaderCell.Value = "Equipment Type";
                dataGridView_FOUPRecord_Display.Columns[2].HeaderCell.Value = "Equipment Subtype";
                dataGridView_FOUPRecord_Display.Columns[3].HeaderCell.Value = "Equipment Name";
                dataGridView_FOUPRecord_Display.Columns[4].HeaderCell.Value = "Location ID";
                dataGridView_FOUPRecord_Display.Columns[5].HeaderCell.Value = "Update Time";
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
        private void QueryCurrentFOUPIDList(string type, string name)
        {
            try
            {
                string errMessage = string.Empty;
                DataSet allFOUPRecord = ServiceHelper.GetService().queryFOUPCurrentRecord(type, name, ref errMessage);
                //--- Loading data to DataGridView is too slow
                comboBox_FOUPRecord_FOUPID.Items.Clear();
                comboBox_FOUPRecord_FOUPID.Items.Add("ALL");
                foreach (DataRow datarow in allFOUPRecord.Tables[0].Rows)
                {
                    comboBox_FOUPRecord_FOUPID.Items.Add(datarow["foupid"]);
                }
                comboBox_FOUPRecord_FOUPID.SelectedIndex = 0;
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
        private void QueryTransferFOUPIDList(string foupid, string begin, string end)
        {
            try
            {
                string errMessage = string.Empty;
                DataSet allTransferRecord = ServiceHelper.GetService().queryTransferRecord(foupid, begin, end, ref errMessage);
                //--- Loading data to DataGridView is too slow
                comboBox_Transfer_FOUPID.Items.Clear();
                comboBox_Transfer_FOUPID.Items.Add("ALL");
                foreach (DataRow datarow in allTransferRecord.Tables[0].Rows)
                {
                    if (comboBox_Transfer_FOUPID.Items.IndexOf(datarow["foupid"]) < 0)
                        comboBox_Transfer_FOUPID.Items.Add(datarow["foupid"]);
                }
                comboBox_Transfer_FOUPID.SelectedIndex = 0;
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
        private void QueryN2PurgeFOUPIDList(string foupid, string begin, string end)
        {
            try
            {
                string errMessage = string.Empty;
                DataSet allN2PurgeRecord = ServiceHelper.GetService().queryN2PurgeRecord(foupid, begin, end, ref errMessage);
                //--- Loading data to DataGridView is too slow
                comboBox_N2Purge_FoupID.Items.Clear();
                comboBox_N2Purge_FoupID.Items.Add("ALL");
                foreach (DataRow datarow in allN2PurgeRecord.Tables[0].Rows)
                {
                    if (comboBox_N2Purge_FoupID.Items.IndexOf(datarow["foupid"]) < 0)
                        comboBox_N2Purge_FoupID.Items.Add(datarow["foupid"]);
                }
                comboBox_N2Purge_FoupID.SelectedIndex = 0;
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
        private void QueryFOUPHistoryRecord(string type, string name, string foupid, string beginTime, string endTime)
        {
            try
            {
                string errMessage = string.Empty;
                DataSet allFOUPRecord = ServiceHelper.GetService().queryFOUPHistoryRecord(type, name, foupid, beginTime, endTime, ref errMessage);
                dataGridView_FOUPRecord_Display.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                ClearFOUPRecord();
                dataGridView_FOUPRecord_Display.DataSource = allFOUPRecord.Tables[0];
                dataGridView_FOUPRecord_Display.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView_FOUPRecord_Display.DefaultCellStyle.BackColor = Color.LightGray;
                dataGridView_FOUPRecord_Display.Columns[0].HeaderCell.Value = "FOUP ID";
                dataGridView_FOUPRecord_Display.Columns[1].HeaderCell.Value = "Equipment Type";
                dataGridView_FOUPRecord_Display.Columns[2].HeaderCell.Value = "Equipment Subtype";
                dataGridView_FOUPRecord_Display.Columns[3].HeaderCell.Value = "Equipment Name";
                dataGridView_FOUPRecord_Display.Columns[4].HeaderCell.Value = "Location ID";
                dataGridView_FOUPRecord_Display.Columns[5].HeaderCell.Value = "Update Time";
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
        private void QueryFOUPHistoryFOUPList(string type, string name, string foupid, string beginTime, string endTime)
        {
            try
            {
                string errMessage = string.Empty;
                DataSet allFOUPRecord = ServiceHelper.GetService().queryFOUPHistoryRecord(type, name, foupid, beginTime, endTime, ref errMessage);
                //--- Loading data to DataGridView is too slow
                comboBox_FOUPRecord_FOUPID.Items.Clear();
                comboBox_FOUPRecord_FOUPID.Items.Add("ALL");
                foreach (DataRow datarow in allFOUPRecord.Tables[0].Rows)
                {
                    if (comboBox_FOUPRecord_FOUPID.Items.IndexOf(datarow["foupid"]) < 0)
                        comboBox_FOUPRecord_FOUPID.Items.Add(datarow["foupid"]);
                }
                comboBox_FOUPRecord_FOUPID.SelectedIndex = 0;
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
        private void QueryTransferRecord(string foupid, string beginTime, string endTime)
        {
            try
            {
                string errMessage = string.Empty;
                DataSet allTransferRecord = ServiceHelper.GetService().queryTransferRecord(foupid, beginTime, endTime, ref errMessage);
                dataGridView_TransferRecord_Display.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                ClearTransferRecord();
                dataGridView_TransferRecord_Display.DataSource = allTransferRecord.Tables[0];
                dataGridView_TransferRecord_Display.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView_TransferRecord_Display.DefaultCellStyle.BackColor = Color.LightGray;
                dataGridView_TransferRecord_Display.Columns[0].HeaderCell.Value = "Update Time";
                dataGridView_TransferRecord_Display.Columns[1].HeaderCell.Value = "FOUP ID";
                dataGridView_TransferRecord_Display.Columns[2].HeaderCell.Value = "Equipment Name";
                dataGridView_TransferRecord_Display.Columns[3].HeaderCell.Value = "Equipment Type";
                dataGridView_TransferRecord_Display.Columns[4].HeaderCell.Value = "Source Location";
                dataGridView_TransferRecord_Display.Columns[5].HeaderCell.Value = "Destination Location";
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
        private void QueryN2PurgeRecord(string foupid, string beginTime, string endTime)
        {
            try
            {
                string errMessage = string.Empty;
                DataSet allN2PurgeRecord = ServiceHelper.GetService().queryN2PurgeRecord(foupid, beginTime, endTime, ref errMessage);
                dataGridView_N2Purge.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                ClearN2PurgeRecord();
                dataGridView_N2Purge.DataSource = allN2PurgeRecord.Tables[0];
                dataGridView_N2Purge.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView_N2Purge.DefaultCellStyle.BackColor = Color.LightGray;
                dataGridView_N2Purge.Columns[0].HeaderCell.Value = "FOUP ID";
                dataGridView_N2Purge.Columns[1].HeaderCell.Value = "Equipment Name";
                dataGridView_N2Purge.Columns[2].HeaderCell.Value = "Equipment Type";
                dataGridView_N2Purge.Columns[3].HeaderCell.Value = "Location ID";
                dataGridView_N2Purge.Columns[4].HeaderCell.Value = "Begin Time";
                dataGridView_N2Purge.Columns[5].HeaderCell.Value = "End Time";
                dataGridView_N2Purge.Columns[6].HeaderCell.Value = "Duration Time (Second)";
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
        private void QueryBasicSetting()
        {
            try
            {
                string errMessage = string.Empty;
                DataSet allBasicSetting = ServiceHelper.GetService().queryBasicSetting(ref errMessage);
                dataGridView_Config_ShowSomething.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                ClearBasicSetting();
                dataGridView_Config_ShowSomething.DataSource = allBasicSetting.Tables[0];
                dataGridView_Config_ShowSomething.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView_Config_ShowSomething.Columns[0].HeaderCell.Value = "Type";
                dataGridView_Config_ShowSomething.Columns[1].HeaderCell.Value = "Subtype";
                dataGridView_Config_ShowSomething.Columns[2].HeaderCell.Value = "Equipment Name";
                dataGridView_Config_ShowSomething.Columns[3].HeaderCell.Value = "Equipement Capacity";
                dataGridView_Config_ShowSomething.Columns[4].HeaderCell.Value = "HSMS IP";
                dataGridView_Config_ShowSomething.Columns[5].HeaderCell.Value = "HSMS Port";
                dataGridView_Config_ShowSomething.Columns[6].HeaderCell.Value = "HSMS Device ID";
                dataGridView_Config_ShowSomething.Columns[7].HeaderCell.Value = "EQP Avaliable";
                dataGridView_Config_ShowSomething.Columns[8].HeaderCell.Value = "Count of Avaliable Locaion";
                dataGridView_Config_ShowSomething.Columns[9].HeaderCell.Value = "Count of Unavaliable Locaion";
                dataGridView_Config_ShowSomething.Columns[10].HeaderCell.Value = "Count of N2 Purge Locaion";
                dataGridView_Config_ShowSomething.Columns[11].HeaderCell.Value = "Count of Charging Locaion";
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
        private void QueryTaskInfoCondition(string tasktype, string taskcradle, string foupid, string taskstatus, string start_time, string end_time)
        {
            try
            {
                string errMessage = string.Empty;
                DataSet allTaskInfoCondition = ServiceHelper.GetService().getTaskInformation(tasktype, taskcradle, foupid, taskstatus, start_time, end_time, ref errMessage);
                comboBox_TaskInfo_TaskType.Items.Clear();
                comboBox_TaskInfo_TaskType.Items.Add("ALL");
                comboBox_TaskInfo_TaskCradle.Items.Clear();
                comboBox_TaskInfo_TaskCradle.Items.Add("ALL");
                comboBox_TaskInfo_FOOUPID.Items.Clear();
                comboBox_TaskInfo_FOOUPID.Items.Add("ALL");
                comboBox_TaskInfo_TaskStatus.Items.Clear();
                comboBox_TaskInfo_TaskStatus.Items.Add("ALL");
                foreach (DataRow datarow in allTaskInfoCondition.Tables[0].Rows)
                {
                    if (comboBox_TaskInfo_TaskType.Items.IndexOf(datarow["tasktype"]) < 0)
                        comboBox_TaskInfo_TaskType.Items.Add(datarow["tasktype"]);
                    if (comboBox_TaskInfo_TaskCradle.Items.IndexOf(datarow["taskcradle"]) < 0)
                        comboBox_TaskInfo_TaskCradle.Items.Add(datarow["taskcradle"]);
                    if (comboBox_TaskInfo_FOOUPID.Items.IndexOf(datarow["foupid"]) < 0)
                        comboBox_TaskInfo_FOOUPID.Items.Add(datarow["foupid"]);
                    if (comboBox_TaskInfo_TaskStatus.Items.IndexOf(datarow["taskstatus"]) < 0)
                        comboBox_TaskInfo_TaskStatus.Items.Add(datarow["taskstatus"]);
                }
                comboBox_TaskInfo_TaskType.SelectedIndex = 0;
                comboBox_TaskInfo_TaskCradle.SelectedIndex = 0;
                comboBox_TaskInfo_FOOUPID.SelectedIndex = 0;
                comboBox_TaskInfo_TaskStatus.SelectedIndex = 0;
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
        private void QueryTaskInfo(string tasktype, string taskcradle, string foupid, string taskstatus, string start_time, string end_time)
        {
            try
            {
                string errMessage = string.Empty;
                DataSet allTaskInfo = ServiceHelper.GetService().getTaskInformation(tasktype, taskcradle, foupid, taskstatus, start_time, end_time, ref errMessage);
                dataGridView_TaskInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                ClearTaskInfo();
                dataGridView_TaskInfo.DataSource = allTaskInfo.Tables[0];
                dataGridView_TaskInfo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView_TaskInfo.DefaultCellStyle.BackColor = Color.LightGray;
                dataGridView_TaskInfo.Columns[0].HeaderCell.Value = "Task Type";
                dataGridView_TaskInfo.Columns[1].HeaderCell.Value = "Task Cradle";
                dataGridView_TaskInfo.Columns[2].HeaderCell.Value = "Task Status";
                dataGridView_TaskInfo.Columns[3].HeaderCell.Value = "Remark";
                dataGridView_TaskInfo.Columns[4].HeaderCell.Value = "User Name";
                dataGridView_TaskInfo.Columns[5].HeaderCell.Value = "FOUP ID";
                dataGridView_TaskInfo.Columns[6].HeaderCell.Value = "Source Location";
                dataGridView_TaskInfo.Columns[7].HeaderCell.Value = "Destination Location";
                dataGridView_TaskInfo.Columns[8].HeaderCell.Value = "Equipment Type";
                dataGridView_TaskInfo.Columns[9].HeaderCell.Value = "Equipment Name";
                dataGridView_TaskInfo.Columns[10].HeaderCell.Value = "Update Time";
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
        private void HandleMessageFromEAP(object listener, MessageReceivedEventArgs messageReceivedEventArgs)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    string logpath = string.Empty;
                    string message = string.Empty;
                    string messageName = string.Empty;
                    string equipmentid = string.Empty;
                    string foupid = string.Empty;
                    string source = string.Empty;
                    string destination = string.Empty;
                    string numberoftimes = string.Empty;
                    string sMessage = string.Empty;
                    CommonFunction comm = new CommonFunction();
                    var message_field = comm.ReadIni("CONFIG.INI", "TIBCORV_FROM_EAP", "MESSAGE_FIELD");
                    logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                    message = messageReceivedEventArgs.Message.GetField(message_field).Value.ToString();
                    comm.LogRecordFun("TIBCORV", "Receive from EAP, " + comm.createFormattedXML(message), logpath);
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(message.Trim());
                    if (xmldoc.GetElementsByTagName("MESSAGENAME")[0] == null
                        || xmldoc.GetElementsByTagName("MESSAGENAME")[0].ChildNodes == null
                        || xmldoc.GetElementsByTagName("MESSAGENAME")[0].ChildNodes.Count == 0)
                    {
                        return;
                    }
                    messageName = xmldoc.GetElementsByTagName("MESSAGENAME")[0].ChildNodes[0].InnerText;
                    if (messageName == "StockInitialAllInfo" || messageName == "UpdateStockLocationInfo" || messageName == "ShowStockMessage" ||
                        messageName == "TransferStartReport" || messageName == "TransferEndReport" || messageName == "CycleTransferStartReport" ||
                        messageName == "CycleTransferEndReport")
                    {
                        if (delegateStockerEvent != null) delegateStockerEvent(message);
                        if (messageName == "CycleTransferStartReport")
                        {
                            try { equipmentid = xmldoc.GetElementsByTagName("STOCKNAME")[0].ChildNodes[0].InnerText.Trim(); } catch { equipmentid = string.Empty; }
                            try { foupid = xmldoc.GetElementsByTagName("CARRIERNAME")[0].ChildNodes[0].InnerText.Trim(); } catch { foupid = string.Empty; }
                            try { source = xmldoc.GetElementsByTagName("FROMLOCATION")[0].ChildNodes[0].InnerText.Trim(); } catch { source = string.Empty; }
                            try { destination = xmldoc.GetElementsByTagName("TOLOCATION")[0].ChildNodes[0].InnerText.Trim(); } catch { destination = string.Empty; }
                            try { numberoftimes = xmldoc.GetElementsByTagName("NUMBEROFTIMES")[0].ChildNodes[0].InnerText.Trim(); } catch { numberoftimes = string.Empty; }
                            CycleTransferListNode node = new CycleTransferListNode();
                            node.equipment = equipmentid;
                            node.foupid = foupid;
                            node.source = source;
                            node.destination = destination;
                            node.numberoftimes = numberoftimes;
                            node.userid = string.Empty;
                            try
                            {
                                CycleTransferListNode temp = CommonFunction.cycletransferlist[equipmentid];
                            }
                            catch
                            {
                                //--- update dictionary
                                if (equipmentid.Trim() != string.Empty &&
                                    foupid.Trim() != string.Empty &&
                                    source.Trim() != string.Empty &&
                                    destination.Trim() != string.Empty &&
                                    numberoftimes.Trim() != string.Empty)
                                {
                                    CommonFunction.cycletransferlist.Add(equipmentid, node);
                                }
                            }
                            try
                            {
                                //--- update information in ui
                                if (comboBox_DisplayTransfer_EquipmentID.Text == node.equipment)
                                {
                                    if (equipmentid.Trim() != string.Empty &&
                                        foupid.Trim() != string.Empty &&
                                        source.Trim() != string.Empty &&
                                        destination.Trim() != string.Empty &&
                                        numberoftimes.Trim() != string.Empty)
                                    {
                                        checkBox_CycleTransfer.Checked = true;
                                        label_DisplayTransfer_Source.Text = node.source;
                                        label_DisplayTransfer_FoupID.Text = node.foupid;
                                        label_DisplayTransfer_Destination.Text = node.destination;
                                        textBox_CycleTransfer.Text = node.numberoftimes;
                                    }
                                }
                            }
                            catch { }
                        }
                        if (messageName == "CycleTransferEndReport")
                        {
                            try { equipmentid = xmldoc.GetElementsByTagName("STOCKNAME")[0].ChildNodes[0].InnerText; } catch { equipmentid = ""; }
                            try { foupid = xmldoc.GetElementsByTagName("CARRIERNAME")[0].ChildNodes[0].InnerText; } catch { foupid = ""; }
                            try { source = xmldoc.GetElementsByTagName("FROMLOCATION")[0].ChildNodes[0].InnerText; } catch { source = ""; }
                            try { destination = xmldoc.GetElementsByTagName("TOLOCATION")[0].ChildNodes[0].InnerText; } catch { destination = ""; }
                            try { numberoftimes = xmldoc.GetElementsByTagName("NUMBEROFTIMES")[0].ChildNodes[0].InnerText; } catch { numberoftimes = ""; }
                            try
                            {
                                CommonFunction.cycletransferlist.Remove(equipmentid);
                                label_DisplayTransfer_Source.Text = string.Empty;
                                label_DisplayTransfer_FoupID.Text = string.Empty;
                                label_DisplayTransfer_Destination.Text = string.Empty;
                                textBox_CycleTransfer.Text = string.Empty;
                                button_DisplayTransfer_Start.Enabled = true;
                            }
                            catch { }
                        }
                        //--- Control Cycle Transfer in MCS Client
                        //if (messageName == "TransferEndReport")
                        //{
                        //    if (CommonFunction.cycletransferlist.Count > 0)
                        //    {
                        //        try { equipmentid = xmldoc.GetElementsByTagName("STOCKNAME")[0].ChildNodes[0].InnerText; } catch (Exception) { equipmentid = ""; }
                        //        try { foupid = xmldoc.GetElementsByTagName("CARRIERNAME")[0].ChildNodes[0].InnerText; } catch (Exception) { foupid = ""; }
                        //        try { source = xmldoc.GetElementsByTagName("FROMLOCATION")[0].ChildNodes[0].InnerText; } catch (Exception) { foupid = ""; }
                        //        try { destination = xmldoc.GetElementsByTagName("TOLOCATION")[0].ChildNodes[0].InnerText; } catch (Exception) { foupid = ""; }
                        //        if (CommonFunction.cycletransferlist.First.Value.equipment == equipmentid &&
                        //            CommonFunction.cycletransferlist.First.Value.foupid == foupid &&
                        //            CommonFunction.cycletransferlist.First.Value.source == source &&
                        //            CommonFunction.cycletransferlist.First.Value.destination == destination)
                        //        {
                        //            sMessage = "[Cycle Transfer] Sequence Number " + CommonFunction.cycletransferlist.First.Value.sequence + ", " + CommonFunction.cycletransferlist.First.Value.source + " Transfer To " + CommonFunction.cycletransferlist.First.Value.destination + " Complete";
                        //            fRichTextBox_ShowMessage(sMessage);
                        //            CommonFunction.cycletransferlist.RemoveFirst();
                        //            Thread.Sleep(1000);
                        //            if (CommonFunction.cycletransferlist.Count > 0)
                        //            {
                        //                var subject = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "SUBJECT");
                        //                var network = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "NETWORK");
                        //                var port = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "PORT");
                        //                var deamon = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "DEAMON");
                        //                string content = string.Empty;
                        //                string[] deamonlist = deamon.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        //                Transport transport = CreateNetPort(port, network, deamonlist);
                        //                checkBox_DisplayTransfer_Transfer.Enabled = false;
                        //                comboBox_DisplayTransfer_EquipmentID.Enabled = false;
                        //                var tmessage = new TIBCO.Rendezvous.Message();
                        //                tmessage.SendSubject = subject;
                        //                content = comm.CreateTransferJob(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now),
                        //                                                 CommonFunction.cycletransferlist.First.Value.equipment,
                        //                                                 CommonFunction.cycletransferlist.First.Value.foupid,
                        //                                                 CommonFunction.cycletransferlist.First.Value.source,
                        //                                                 CommonFunction.cycletransferlist.First.Value.destination,
                        //                                                 CommonFunction.cycletransferlist.First.Value.userid);
                        //                tmessage.AddField(message_field, content);
                        //                comm.LogRecordFun("TIBCORV", "Send To EAP, " + comm.createFormattedXML(content), comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH"));
                        //                SendTibcoRVMessage(transport, tmessage);
                        //                sMessage = "[Cycle Transfer] Next Job:  Sequence Number " + CommonFunction.cycletransferlist.First.Value.sequence + ", " + CommonFunction.cycletransferlist.First.Value.source + " Transfer To " + CommonFunction.cycletransferlist.First.Value.destination;
                        //                fRichTextBox_ShowMessage(sMessage);
                        //            }
                        //            else
                        //            {
                        //                button_DisplayTransfer_Start.Enabled = true;
                        //                sMessage = "[Cycle Transfer] All Job Complete";
                        //                fRichTextBox_ShowMessage(sMessage);
                        //                label_DisplayTransfer_Source.Text = "";
                        //                label_DisplayTransfer_FoupID.Text = "";
                        //                label_DisplayTransfer_Destination.Text = "";
                        //            }
                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                        if (delegateERackEvent != null) delegateERackEvent(message);
                    }
                });
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
        //---解决跨执行序的问题
        private void fCearTextBoxSearchFOUP(string sMessage)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textBox_SearchFOUP.Text = sMessage;
            });
        }
        private void fRichTextBox_ShowMessage(string sMessage)
        {
            DateTime now = DateTime.Now;
            string strTime = now.ToString("yyyy/MM/dd HH:mm:ss");
            CommonFunction comm = new CommonFunction();
            string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
            this.Invoke((MethodInvoker)delegate
            {
                richTextBox_ShowMessage.SelectionColor = Color.Black;
                richTextBox_ShowMessage.SelectionFont = new Font("Consolas", 9, FontStyle.Regular);
                string[] aRichTextBox_ShowMessage = richTextBox_ShowMessage.Text.Split('\n');
                if (strTime + " # " + sMessage != aRichTextBox_ShowMessage[aRichTextBox_ShowMessage.Length - 1])
                {
                    if (richTextBox_ShowMessage.Lines.Length == 0) richTextBox_ShowMessage.AppendText(now.ToString("yyyy/MM/dd HH:mm:ss") + " # " + sMessage);
                    else richTextBox_ShowMessage.AppendText(System.Environment.NewLine + now.ToString("yyyy/MM/dd HH:mm:ss") + " # " + sMessage);
                    comm.LogRecordFun("TRACE", sMessage, logpath);
                }
            });
        }
        private void fRichTextBox_ChangeRobotStatus(string equipmentid, string sMessage)
        {
            this.Invoke((MethodInvoker)delegate
            {
                string status = string.Empty;
                if (sMessage.Contains("设备的Robot当前状态变更为Idle！")) status = "IDLE";
                if (sMessage.Contains("设备的Robot当前状态变更为Run！")) status = "RUN";
                if (sMessage.Contains("设备的Robot当前状态变更为Down！")) status = "DOWN";
                if (sMessage.Contains("设备的Robot当前状态变更为PM！")) status = "PM";
                if (comboBox_DisplayTransfer_EquipmentID.Text.Trim() == equipmentid.Trim())
                {
                    label_RobotStatusValue.Text = status;
                }
            });
        }
        private void fRichTextBox_ShowAlarmMessage(string sMessage)
        {
            DateTime now = DateTime.Now;
            string strTime = now.ToString("yyyy/MM/dd HH:mm:ss");
            CommonFunction comm = new CommonFunction();
            string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
            this.Invoke((MethodInvoker)delegate
            {
                richTextBox_ShowMessage.SelectionColor = Color.Red;
                richTextBox_ShowMessage.SelectionFont = new Font("Consolas", 9, FontStyle.Regular);
                string[] aRichTextBox_ShowMessage = richTextBox_ShowMessage.Text.Split('\n');
                if (strTime + " # " + sMessage != aRichTextBox_ShowMessage[aRichTextBox_ShowMessage.Length - 1])
                {
                    if (richTextBox_ShowMessage.Lines.Length == 0) richTextBox_ShowMessage.AppendText(now.ToString("yyyy/MM/dd HH:mm:ss") + " # " + sMessage);
                    else richTextBox_ShowMessage.AppendText(System.Environment.NewLine + now.ToString("yyyy/MM/dd HH:mm:ss") + " # " + sMessage);
                    comm.LogRecordFun("TRACE", sMessage, logpath);
                }
            });
        }
        private void fRrichTextBox_ShowMessage_ScrollToCaret()
        {
            this.Invoke((MethodInvoker)delegate
            {
                richTextBox_ShowMessage.ScrollToCaret();
            });
        }
        private void setting(string value)
        {
            try
            {
                CommonFunction comm = new CommonFunction();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(value.Trim());
                string messageName = xmldoc.GetElementsByTagName("MESSAGENAME")[0].ChildNodes[0].InnerText;
                string location = "", visiable = "", foupid = "";
                switch (messageName)
                {
                    case "UPDATEBASICINFO":
                        QueryBasicSetting();
                        break;
                    case "SENDLOCATIONTOMAINFORM":
                        if (checkBox_DisplayTransfer_Transfer.Checked == true)
                        {
                            try { location = xmldoc.GetElementsByTagName("LOCATION")[0].ChildNodes[0].InnerText; } catch { location = ""; }
                            try { foupid = xmldoc.GetElementsByTagName("FOUPID")[0].ChildNodes[0].InnerText; } catch { foupid = ""; }
                            try { visiable = xmldoc.GetElementsByTagName("VISIABLE")[0].ChildNodes[0].InnerText; } catch { visiable = ""; }
                            if (location == comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_1_LOCATIONID") ||
                                location == comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_2_LOCATIONID"))
                            {
                                if (label_DisplayTransfer_Source.Text == "") label_DisplayTransfer_Source.Text = location;
                                else label_DisplayTransfer_Destination.Text = location;
                            }
                            else
                            {
                                if (label_DisplayTransfer_Source.Text == "")
                                {
                                    if (visiable == "1") MessageBox.Show("Location " + location + " Is Unavailable, Please Select Other Location As Source", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    else if (foupid == "") MessageBox.Show("FOUP Does Not Exist On Location " + location + ", Please Select Other Location As Source", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    else
                                    {
                                        label_DisplayTransfer_Source.Text = location;
                                        label_DisplayTransfer_FoupID.Text = foupid;
                                    }
                                }
                                else
                                {
                                    if (visiable == "1") MessageBox.Show("Location " + location + " Is Unavailable, Please Select Other Location As Destination", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    else if (foupid != "") MessageBox.Show("FOUP Exist On Location " + location + ", Please Select Other Location As Destination", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    else label_DisplayTransfer_Destination.Text = location;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("If You Want To Create Transfer Job, Please Check 'Transfer'", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    case "CLEARTEXTBOXSEARCHFOUP":
                        fCearTextBoxSearchFOUP("");
                        break;
                    case "SHOWMESSAGE":
                        //fRichTextBox_ShowMessage(xmldoc.GetElementsByTagName("EQUIPMENTNAME")[0].ChildNodes[0].InnerText + ", " +
                        //                         comm.DecodeBase64(xmldoc.GetElementsByTagName("STATUSDESCRIPTION")[0].ChildNodes[0].InnerText));
                        fRichTextBox_ShowMessage(comm.DecodeBase64(xmldoc.GetElementsByTagName("STATUSDESCRIPTION")[0].ChildNodes[0].InnerText));
                        fRichTextBox_ChangeRobotStatus(xmldoc.GetElementsByTagName("EQUIPMENTNAME")[0].ChildNodes[0].InnerText,
                                                       comm.DecodeBase64(xmldoc.GetElementsByTagName("STATUSDESCRIPTION")[0].ChildNodes[0].InnerText));
                        break;
                    case "SHOWALARMMESSAGE":
                        if (xmldoc.GetElementsByTagName("EQUIPMENTTYPE")[0].ChildNodes[0].InnerText == "ERACK")
                            fRichTextBox_ShowAlarmMessage(xmldoc.GetElementsByTagName("EQUIPMENTTYPE")[0].ChildNodes[0].InnerText + ", " +
                                                          comm.DecodeBase64(xmldoc.GetElementsByTagName("STATUSDESCRIPTION")[0].ChildNodes[0].InnerText));
                        else
                        {
                            //fRichTextBox_ShowAlarmMessage(xmldoc.GetElementsByTagName("EQUIPMENTNAME")[0].ChildNodes[0].InnerText + ", " +
                            //                              comm.DecodeBase64(xmldoc.GetElementsByTagName("STATUSDESCRIPTION")[0].ChildNodes[0].InnerText));
                            fRichTextBox_ShowAlarmMessage(comm.DecodeBase64(xmldoc.GetElementsByTagName("STATUSDESCRIPTION")[0].ChildNodes[0].InnerText));
                        }
                        fRichTextBox_ChangeRobotStatus(xmldoc.GetElementsByTagName("EQUIPMENTNAME")[0].ChildNodes[0].InnerText,
                                                       comm.DecodeBase64(xmldoc.GetElementsByTagName("STATUSDESCRIPTION")[0].ChildNodes[0].InnerText));
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
        private void button_DisplayTransfer_Clear_Click(object sender, EventArgs e)
        {
            try
            {
                button_DisplayTransfer_Start.Enabled = true;
                string content = string.Empty;
                CommonFunction comm = new CommonFunction();
                var subject = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "SUBJECT");
                var network = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "NETWORK");
                var port = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "PORT");
                var deamon = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "DEAMON");
                var message_field = comm.ReadIni("CONFIG.INI", "TIBCORV_FROM_EAP", "MESSAGE_FIELD");
                string[] deamonlist = deamon.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                Transport transport = CreateNetPort(port, network, deamonlist);
                var message = new TIBCO.Rendezvous.Message();
                message.SendSubject = subject;
                if (button_DisplayTransfer_Clear.Text == "Cancel")
                {
                    if (label_DisplayTransfer_FoupID.Text.Trim() == string.Empty ||
                        label_DisplayTransfer_Destination.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Source Location or Destination Location is Empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        //if (CommonFunction.cycletransferlist.Count > 0) CommonFunction.cycletransferlist.Clear();
                        try
                        {
                            CycleTransferListNode node = CommonFunction.cycletransferlist[comboBox_DisplayTransfer_EquipmentID.Text];
                            content = comm.CancelCycleTransferJob(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now),
                                                              node.equipment,
                                                              node.foupid,
                                                              node.source,
                                                              node.destination,
                                                              node.userid);
                            message.AddField(message_field, content);
                            comm.LogRecordFun("TIBCORV", "Send To EAP, " + comm.createFormattedXML(content), comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH"));
                            SendTibcoRVMessage(transport, message);
                            MessageBox.Show("MCS Client Send Command '" + button_DisplayTransfer_Clear.Text + "' To MCS Server Complete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CommonFunction.cycletransferlist.Remove(comboBox_DisplayTransfer_EquipmentID.Text);
                            string sMessage = "MCS Client Send 'Cancel Cycle Transfer' to MCS Server, Equipment: " + comboBox_DisplayTransfer_EquipmentID.Text +
                                              ", FOUP ID: " + label_DisplayTransfer_FoupID.Text +
                                              ", Source: " + label_DisplayTransfer_Source.Text +
                                              ", Destination: " + label_DisplayTransfer_Destination.Text;
                            fRichTextBox_ShowMessage(sMessage);
                            textBox_CycleTransfer.Text = string.Empty;
                        }
                        catch { }
                    }
                    label_DisplayTransfer_Source.Text = string.Empty;
                    label_DisplayTransfer_FoupID.Text = string.Empty;
                    label_DisplayTransfer_Destination.Text = string.Empty;
                    checkBox_CycleTransfer.Checked = false;
                }
                else
                {
                    label_DisplayTransfer_Source.Text = string.Empty;
                    label_DisplayTransfer_FoupID.Text = string.Empty;
                    label_DisplayTransfer_Destination.Text = string.Empty;
                    checkBox_CycleTransfer.Checked = false;
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
        private void button_DisplayTransfer_Start_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = true;
                bool flag2 = true;
                CommonFunction comm = new CommonFunction();
                if (label_DisplayTransfer_Source.Text == "")
                {
                    MessageBox.Show("Please Double Click One Location As Source", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (label_DisplayTransfer_Destination.Text == "")
                {
                    MessageBox.Show("Please Double Click One Location As Destination", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (label_DisplayTransfer_Destination.Text == label_DisplayTransfer_Source.Text)
                {
                    MessageBox.Show("Source And Destination Have Same Value", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label_DisplayTransfer_Source.Text = "";
                    label_DisplayTransfer_Destination.Text = "";
                }
                else if ((label_DisplayTransfer_Destination.Text == comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_1_LOCATIONID") || label_DisplayTransfer_Destination.Text == comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_2_LOCATIONID")) &&
                         (label_DisplayTransfer_Source.Text == comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_1_LOCATIONID") || label_DisplayTransfer_Source.Text == comm.ReadIni("CONFIG.INI", "STOCKER", "MANUALPORT_2_LOCATIONID")))
                {
                    MessageBox.Show("Equipment Does Not Support Function 'Loadport To Loadport'", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label_DisplayTransfer_Source.Text = "";
                    label_DisplayTransfer_Destination.Text = "";
                }
                else
                {
                    var subject = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "SUBJECT");
                    var network = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "NETWORK");
                    var port = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "PORT");
                    var deamon = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "DEAMON");
                    var message_field = comm.ReadIni("CONFIG.INI", "TIBCORV_FROM_EAP", "MESSAGE_FIELD");
                    string content = string.Empty;
                    string[] deamonlist = deamon.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    Transport transport = CreateNetPort(port, network, deamonlist);
                    checkBox_DisplayTransfer_Transfer.Enabled = false;
                    comboBox_DisplayTransfer_EquipmentID.Enabled = false;
                    var message = new TIBCO.Rendezvous.Message();
                    message.SendSubject = subject;
                    if (button_DisplayTransfer_Start.Text == "Transfer")
                    {
                        content = comm.CreateTransferJob(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now),
                                                         comboBox_DisplayTransfer_EquipmentID.Text,
                                                         label_DisplayTransfer_FoupID.Text,
                                                         label_DisplayTransfer_Source.Text,
                                                         label_DisplayTransfer_Destination.Text,
                                                         textBox_Display_LoginUser.Text);
                    }
                    else if (button_DisplayTransfer_Start.Text == "Charge")
                    {
                        content = comm.CreateChargeJob(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now),
                                                       comboBox_DisplayTransfer_EquipmentID.Text,
                                                       label_DisplayTransfer_FoupID.Text,
                                                       label_DisplayTransfer_Source.Text,
                                                       label_DisplayTransfer_Destination.Text,
                                                       textBox_Display_LoginUser.Text);
                    }
                    else if (button_DisplayTransfer_Start.Text == "Start")
                    {
                        //int sequence = 0;
                        //if (CommonFunction.cycletransferlist.Count > 0) CommonFunction.cycletransferlist.Clear();
                        try
                        {
                            //--- Control Cycle Transfer in MCS Client
                            //int gap = Int32.Parse(textBox_CycleTransfer.Text.Trim());
                            //for (int idx = 1; idx <= int.Parse(textBox_CycleTransfer.Text) * 2; idx++)
                            //{
                            //    CycleTransferListNode node = new CycleTransferListNode();
                            //    if (idx % 2 == 0)
                            //    {
                            //        node.sequence = sequence;
                            //        node.source = label_DisplayTransfer_Destination.Text;
                            //        node.destination = label_DisplayTransfer_Source.Text;
                            //        node.equipment = comboBox_DisplayTransfer_EquipmentID.Text;
                            //        node.foupid = label_DisplayTransfer_FoupID.Text;
                            //        node.userid = textBox_Display_LoginUser.Text;
                            //    }
                            //    else
                            //    {
                            //        sequence++;
                            //        node.sequence = sequence;
                            //        node.source = label_DisplayTransfer_Source.Text;
                            //        node.destination = label_DisplayTransfer_Destination.Text;
                            //        node.equipment = comboBox_DisplayTransfer_EquipmentID.Text;
                            //        node.foupid = label_DisplayTransfer_FoupID.Text;
                            //        node.userid = textBox_Display_LoginUser.Text;
                            //    }
                            //    CommonFunction.cycletransferlist.AddLast(node);
                            //}
                            //content = comm.CreateTransferJob(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now),
                            //                                 CommonFunction.cycletransferlist.First.Value.equipment,
                            //                                 CommonFunction.cycletransferlist.First.Value.foupid,
                            //                                 CommonFunction.cycletransferlist.First.Value.source,
                            //                                 CommonFunction.cycletransferlist.First.Value.destination,
                            //                                 CommonFunction.cycletransferlist.First.Value.userid);
                            //string sMessage = "[Cycle Transfer] First Job:  Sequence Number " + CommonFunction.cycletransferlist.First.Value.sequence + ", " + CommonFunction.cycletransferlist.First.Value.source + " Transfer To " + CommonFunction.cycletransferlist.First.Value.destination;
                            //fRichTextBox_ShowMessage(sMessage);
                            //--- Control Cycle Transfer in MCS Server
                            try
                            {
                                int.Parse(textBox_CycleTransfer.Text);
                            }
                            catch
                            {
                                MessageBox.Show("Field 'Number of Times' is not a Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            button_DisplayTransfer_Start.Enabled = false;
                            string sMessage = "MCS Client Send 'Start Cycle Transfer' to MCS Server, Equipment: " + comboBox_DisplayTransfer_EquipmentID.Text +
                                              ", FOUP ID: " + label_DisplayTransfer_FoupID.Text +
                                              ", Source: " + label_DisplayTransfer_Source.Text +
                                              ", Destination: " + label_DisplayTransfer_Destination.Text;
                            fRichTextBox_ShowMessage(sMessage);
                            CycleTransferListNode node = new CycleTransferListNode();
                            node.equipment = comboBox_DisplayTransfer_EquipmentID.Text;
                            node.foupid = label_DisplayTransfer_FoupID.Text;
                            node.source = label_DisplayTransfer_Source.Text;
                            node.destination = label_DisplayTransfer_Destination.Text;
                            node.numberoftimes = textBox_CycleTransfer.Text;
                            node.userid = textBox_Display_LoginUser.Text;
                            CommonFunction.cycletransferlist.Add(node.equipment, node);
                            content = comm.CreateCycleTransferJob(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now),
                                                                  node.equipment,
                                                                  node.foupid,
                                                                  node.source,
                                                                  node.destination,
                                                                  node.userid,
                                                                  node.numberoftimes);
                            //flag = false;
                        }
                        catch
                        {
                            //MessageBox.Show("Number of Times is not a number", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            flag2 = false;
                            button_DisplayTransfer_Start.Enabled = true;
                        }
                    }
                    message.AddField(message_field, content);
                    comm.LogRecordFun("TIBCORV", "Send To EAP, " + comm.createFormattedXML(content), comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH"));
                    SendTibcoRVMessage(transport, message);
                    if (flag2 == true) MessageBox.Show("MCS Client Send Command '" + button_DisplayTransfer_Start.Text + "' To MCS Server Complete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (flag)
                    {
                        //label_DisplayTransfer_Source.Text = "";
                        //label_DisplayTransfer_FoupID.Text = "";
                        //label_DisplayTransfer_Destination.Text = "";
                        checkBox_CycleTransfer.Checked = false;
                        textBox_CycleTransfer.Text = string.Empty;
                    }
                }
                //if MCS Client does not accept reply from EAP, button "Start" need to disable.
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
        private void checkBox_DisplayTransfer_ShowFOUPID_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControlDisplayTransfer_DisplayTransfer.TabPages.Count > 0)
                    tabControlDisplayTransfer_DisplayTransfer.TabPages.RemoveAt(0);
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
        private void updateShowLocation()
        {
            checkBox_DisplayTransfer_ShowLocation.Enabled = false;
            try
            {
                CommonFunction comm = new CommonFunction();
                if (checkBox_DisplayTransfer_ShowLocation.Checked == true)
                {
                    if (tabControlDisplayTransfer_DisplayTransfer.TabPages.Count > 0)
                        tabControlDisplayTransfer_DisplayTransfer.TabPages.RemoveAt(0);
                    checkBox_DisplayTransfer_ShowEquipmentStatus.Checked = false;
                    comboBox_DisplayTransfer_EquipmentType.Enabled = true;
                    comboBox_DisplayTransfer_EquipmentID.Enabled = true;
                    checkBox_DisplayTransfer_Transfer.Enabled = true;
                    groupBox_SearchFOUP.Visible = true;
                    getEquipmentType(0, false);
                }
                else
                {
                    checkBox_DisplayTransfer_ShowEquipmentStatus.Checked = true;
                    checkBox_DisplayTransfer_Transfer.Checked = false;
                    checkBox_DisplayTransfer_Transfer.Enabled = false;
                    comboBox_DisplayTransfer_EquipmentType.Enabled = false;
                    comboBox_DisplayTransfer_EquipmentID.Enabled = false;
                    groupBox_DisplayTransfer_Start.Visible = false;
                    groupBox_DisplayTransfer_BatchTransfer.Visible = false;
                    groupBox_SearchFOUP.Visible = false;
                    getEquipmentType(0, true);
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
            checkBox_DisplayTransfer_ShowLocation.Enabled = true;
        }
        private void checkBox_DisplayTransfer_ShowLocation_MouseClick(object sender, MouseEventArgs e)
        {
            //updateShowLocation();
        }
        private void checkBox_DisplayTransfer_ShowLocation_Click(object sender, EventArgs e)
        {
            updateShowLocation();
        }
        private void updateShowEquipmentstatus()
        {
            checkBox_DisplayTransfer_ShowEquipmentStatus.Visible = false;
            try
            {
                //---
                if (tabControlDisplayTransfer_DisplayTransfer.TabPages.Count > 0)
                    tabControlDisplayTransfer_DisplayTransfer.TabPages.RemoveAt(0);
                //---
                if (checkBox_DisplayTransfer_ShowEquipmentStatus.Checked == true)
                {
                    groupBox_DisplayTransfer_Start.Visible = false;
                    groupBox_DisplayTransfer_BatchTransfer.Visible = false;
                    checkBox_DisplayTransfer_ShowLocation.Checked = false;
                    checkBox_DisplayTransfer_Transfer.Checked = false;
                    comboBox_DisplayTransfer_EquipmentType.Enabled = false;
                    comboBox_DisplayTransfer_EquipmentID.Enabled = false;
                    checkBox_DisplayTransfer_Transfer.Enabled = false;
                    groupBox_SearchFOUP.Visible = false;
                    getEquipmentType(0, true);
                }
                else
                {
                    checkBox_DisplayTransfer_ShowLocation.Checked = true;
                    checkBox_DisplayTransfer_Transfer.Checked = false;
                    comboBox_DisplayTransfer_EquipmentType.Enabled = true;
                    comboBox_DisplayTransfer_EquipmentID.Enabled = true;
                    groupBox_SearchFOUP.Visible = true;
                    getEquipmentType(0, false);
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
            checkBox_DisplayTransfer_ShowEquipmentStatus.Visible = true;
        }
        private void checkBox_DisplayTransfer_ShowEquipmentStatus_MouseClick(object sender, MouseEventArgs e)
        {
            //updateShowEquipmentstatus();
        }
        private void checkBox_DisplayTransfer_ShowEquipmentStatus_Click(object sender, EventArgs e)
        {
            updateShowEquipmentstatus();
        }
        private void updateTransfer()
        {
            try
            {
                checkBox_DisplayTransfer_ShowEquipmentStatus.Checked = false;
                groupBox_DisplayTransfer_Start.Visible = true;
                label_DisplayTransfer_Source.Text = "";
                label_DisplayTransfer_Destination.Text = "";
                //---
                if (checkBox_DisplayTransfer_Transfer.Checked == true)
                {
                    groupBox_DisplayTransfer_Start.Visible = true;
                    groupBox_DisplayTransfer_BatchTransfer.Visible = true;
                    try
                    {
                        CycleTransferListNode node = CommonFunction.cycletransferlist[comboBox_DisplayTransfer_EquipmentID.Text];
                        if (node.equipment.Trim() != string.Empty &&
                            node.foupid.Trim() != string.Empty &&
                            node.source.Trim() != string.Empty &&
                            node.destination.Trim() != string.Empty &&
                            node.numberoftimes.Trim() != string.Empty)
                        {
                            checkBox_CycleTransfer.Checked = true;
                            label_DisplayTransfer_Source.Text = node.source;
                            label_DisplayTransfer_FoupID.Text = node.foupid;
                            label_DisplayTransfer_Destination.Text = node.destination;
                            textBox_CycleTransfer.Text = node.numberoftimes;
                        }
                    }
                    catch
                    { }
                }
                else
                {
                    groupBox_DisplayTransfer_Start.Visible = false;
                    groupBox_DisplayTransfer_BatchTransfer.Visible = false;
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
        private void checkBox_DisplayTransfer_Transfer_Click(object sender, EventArgs e)
        {
            updateTransfer();
        }
        private void checkBox_DisplayTransfer_Transfer_MouseClick(object sender, MouseEventArgs e)
        {
            //updateTransfer();
        }
        private List<NetTransport> TransportList = new List<NetTransport>();
        private NetTransport CreateNetPort(string Service, string Network, string[] deamonlist)
        {
            NetTransport transport = null;
            if (deamonlist.Length == 0)
            {
                try
                {
                    transport = new NetTransport(Service, Network, "");
                    TransportList.Add(transport);
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
            else
            {
                foreach (string daemon in deamonlist)
                {
                    try
                    {
                        transport = new NetTransport(Service, Network, daemon);
                        TransportList.Add(transport);
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
            }
            if (TransportList.Count > 0) transport = TransportList[0];
            else throw new Exception("Connect Failed");
            return transport;
        }
        private void SendTibcoRVMessage(Transport transport, TIBCO.Rendezvous.Message message)
        {
            bool sendResult = false;
            Exception sendMessageException = null;
            try
            {
                transport.Send(message);
                sendResult = true;
            }
            catch (Exception e)
            {
                sendMessageException = e;
                CommonFunction comm = new CommonFunction();
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + sendMessageException,
                                  logpath);
                for (int idx = 1; idx < TransportList.Count; idx++)
                {
                    try
                    {
                        transport = TransportList[idx];
                        transport.Send(message);
                        sendResult = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        comm = new CommonFunction();
                        logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                        comm.LogRecordFun("EXCEPTION",
                                          System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                          logpath);
                        sendMessageException = ex;
                        sendResult = false;
                    }
                }
            }
            if (!sendResult)
            {
                throw sendMessageException;
            }
        }
        private void comboBox_DisplayTransfer_EquipmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CommonFunction comm = new CommonFunction();
                var message_field = "";
                var subject = "";
                var network = "";
                var port = "";
                var deamon = "";
                string[] deamonlist = null;
                Transport transport = null;
                //--- Request initialize message
                if (checkBox_DisplayTransfer_ShowLocation.Checked == true)
                {
                    message_field = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "MESSAGE_FIELD");
                    subject = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "SUBJECT");
                    network = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "NETWORK");
                    port = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "PORT");
                    deamon = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "DEAMON");
                    //transport = new NetTransport(port, network, deamon);
                    deamonlist = deamon.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    transport = CreateNetPort(port, network, deamonlist);
                    string type = string.Empty;
                    string errMessage = string.Empty;
                    DataSet dsType = ServiceHelper.GetService().getTypeBaseOnEquipmentName(comboBox_DisplayTransfer_EquipmentID.Text, ref errMessage);
                    foreach (DataRow datarow in dsType.Tables[0].Rows) type = datarow["type"].ToString();
                    if (comboBox_DisplayTransfer_EquipmentType.Text == "ERACK")
                    {
                        button_Refrush.Visible = false;
                        button_ClearJob.Visible = false;
                        checkBox_DisplayTransfer_Transfer.Enabled = false;
                        comboBox_DisplayTransfer_EquipmentID.Enabled = false;
                        groupBox_DisplayTransfer_Start.Visible = false;
                        groupBox_DisplayTransfer_BatchTransfer.Visible = false;
                        //---
                        if (type == comboBox_DisplayTransfer_EquipmentType.Text)
                        {
                            var message = new TIBCO.Rendezvous.Message();
                            message.SendSubject = subject;
                            message.AddField(message_field, comm.createFormattedXML(comm.QueryERackAllInfo(comboBox_DisplayTransfer_EquipmentID.Text, string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now))));
                            comm.LogRecordFun("TIBCORV", "Send To EAP, " + comm.createFormattedXML(comm.QueryERackAllInfo(comboBox_DisplayTransfer_EquipmentID.Text, string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now))), comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH"));
                            SendTibcoRVMessage(transport, message);
                            TimerClass.delegateEvent += new TimerClass.delegateSetting(setting);
                            TimerClass.initializeTimer(int.Parse(ConfigurationManager.AppSettings["TIMEOUT"].ToString()),
                                                       int.Parse(ConfigurationManager.AppSettings["TIMESPACE"].ToString()));
                        }
                    }
                    else
                    {
                        button_Refrush.Visible = true;
                        button_ClearJob.Visible = true;
                        checkBox_DisplayTransfer_Transfer.Enabled = true;
                        comboBox_DisplayTransfer_EquipmentID.Enabled = true;
                        //---
                        var message = new TIBCO.Rendezvous.Message();
                        message.SendSubject = subject;
                        string equipmentname = string.Empty;
                        if (comboBox_DisplayTransfer_EquipmentID.Text == "ALL" && comboBox_DisplayTransfer_EquipmentID.Items.Count > 1)
                            comboBox_DisplayTransfer_EquipmentID.SelectedIndex = 1;
                        message.AddField(message_field, comm.createFormattedXML(comm.QueryStockerAllInfo(comboBox_DisplayTransfer_EquipmentID.Text, string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now))));
                        comm.LogRecordFun("TIBCORV", "Send To EAP, " + comm.createFormattedXML(comm.QueryStockerAllInfo(comboBox_DisplayTransfer_EquipmentID.Text, string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now))), comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH"));
                        SendTibcoRVMessage(transport, message);
                        TimerClass.delegateEvent += new TimerClass.delegateSetting(setting);
                        TimerClass.initializeTimer(int.Parse(ConfigurationManager.AppSettings["TIMEOUT"].ToString()),
                                                   int.Parse(ConfigurationManager.AppSettings["TIMESPACE"].ToString()));
                    }
                }
                if (tabControlDisplayTransfer_DisplayTransfer.TabPages.Count > 0) tabControlDisplayTransfer_DisplayTransfer.TabPages.RemoveAt(0);
                if (groupBox_DisplayTransfer_Start.Visible == true)
                {
                    label_DisplayTransfer_Source.Text = "";
                    label_DisplayTransfer_Destination.Text = "";
                }
                getEquipmentID(0, false);
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
        private void comboBox_DisplayTransfer_EquipmentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            //---
            label_RobotStatusValue.Text = string.Empty;
            if (groupBox_DisplayTransfer_Start.Visible == true)
            {
                label_DisplayTransfer_Source.Text = string.Empty;
                label_DisplayTransfer_Destination.Text = string.Empty;
            }
            //---
            while (tabControlDisplayTransfer_DisplayTransfer.TabPages.Count > 0)
                tabControlDisplayTransfer_DisplayTransfer.TabPages.RemoveAt(0);
            getLocationinfoBaseOnEquipment(comboBox_DisplayTransfer_EquipmentType.Text.Trim());
        }
        private void textBox_SearchFOUP_TextChanged(object sender, EventArgs e)
        {
            if (textBox_SearchFOUP.Text != "")
            {
                comboBox_DisplayTransfer_EquipmentType.Enabled = false;
                comboBox_DisplayTransfer_EquipmentID.Enabled = false;
            }
            else
            {
                comboBox_DisplayTransfer_EquipmentType.Enabled = true;
                if (comboBox_DisplayTransfer_EquipmentType.Text == "STOCKER") comboBox_DisplayTransfer_EquipmentID.Enabled = true;
            }
        }
        private void label_DisplayTransfer_Source_TextChanged(object sender, EventArgs e)
        {
            if (label_DisplayTransfer_Source.Text != "")
            {
                comboBox_DisplayTransfer_EquipmentType.Enabled = false;
                comboBox_DisplayTransfer_EquipmentID.Enabled = false;
            }
            else
            {
                comboBox_DisplayTransfer_EquipmentType.Enabled = true;
                comboBox_DisplayTransfer_EquipmentID.Enabled = true;
            }
        }
        private void searchLocationBaseOnFOUPID()
        {
            CommonFunction comm = new CommonFunction();
            string message = "";
            try
            {
                if (textBox_SearchFOUP.Text != "")
                {
                    if (comboBox_DisplayTransfer_EquipmentType.Text == "STOCKER")
                    {
                        message = comm.SearchFOUPID(comboBox_DisplayTransfer_EquipmentType.Text, comboBox_DisplayTransfer_EquipmentID.Text, textBox_SearchFOUP.Text);
                        delegateStockerEvent(message);
                    }
                    else if (comboBox_DisplayTransfer_EquipmentType.Text == "ERACK")
                    {
                        message = comm.SearchFOUPID(comboBox_DisplayTransfer_EquipmentType.Text, comboBox_DisplayTransfer_EquipmentID.Text, textBox_SearchFOUP.Text);
                        delegateERackEvent(message);
                    }
                }
                else MessageBox.Show("FOUP ID is null", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        private void button_SearchFOUP_Click(object sender, EventArgs e)
        {
            searchLocationBaseOnFOUPID();
        }
        private void textBox_SearchFOUP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) searchLocationBaseOnFOUPID();
        }
        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            CommonFunction comm = new CommonFunction();
            try
            {
                Process process = Process.GetCurrentProcess();
                Process[] aProcess = Process.GetProcessesByName(process.ProcessName);
                foreach (Process saygoodbye in aProcess) saygoodbye.Kill();
            }
            catch (Exception ex)
            {
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        private void richTextBox_ShowMessage_TextChanged(object sender, EventArgs e)
        {
            fRrichTextBox_ShowMessage_ScrollToCaret();
        }
        private void onlySelectOneItem(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Checked) return;
            for (int i = 0; i < ((CheckedListBox)sender).Items.Count; i++) ((CheckedListBox)sender).SetItemChecked(i, false);
            e.NewValue = CheckState.Checked;
        }
        private void checkedListBox_Config_Step1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            onlySelectOneItem(sender, e);
        }
        private void checkedListBox_Config_Step2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            onlySelectOneItem(sender, e);
        }
        private void checkedListBox_Config_Step3_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            onlySelectOneItem(sender, e);
        }
        private void checkedListBox_Config_Step4_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            onlySelectOneItem(sender, e);
        }
        private void checkedListBox_Config_Step1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonFunction comm = new CommonFunction();
            try
            {
                button_Config_Step5_Confirn.Focus();
                if (checkedListBox_Config_Step1.CheckedItems.Count > 0) groupBox_Config_Step2.Enabled = true;
                else
                {
                    groupBox_Config_Step2.Enabled = false;
                    groupBox_Config_Step3.Enabled = false;
                    groupBox_Config_Step4.Enabled = false;
                    groupBox_Config_Step5.Enabled = false;
                    groupBox_Config_Step6.Enabled = false;
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
        private void checkedListBox_Config_Step2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (checkedListBox_Config_Step2.CheckedItems.Count != 0 && checkedListBox_Config_Step1.SelectedItem.ToString() == "ERACK")
            //    groupBox_Config_Step3.Enabled = true;
            //else groupBox_Config_Step3.Enabled = false;
            CommonFunction comm = new CommonFunction();
            try
            {
                if (checkedListBox_Config_Step2.CheckedItems.Count > 0)
                {
                    groupBox_Config_Step3.Enabled = true;
                    if (checkedListBox_Config_Step2.SelectedItem.ToString().Contains("INSERT"))
                    {
                        textBox_Config_Step4.Visible = true;
                        checkedListBox_Config_Step4.Visible = false;
                    }
                    else
                    {
                        textBox_Config_Step4.Visible = false;
                        checkedListBox_Config_Step4.Visible = true;
                    }
                }
                else
                {
                    groupBox_Config_Step3.Enabled = false;
                    groupBox_Config_Step4.Enabled = false;
                    groupBox_Config_Step5.Enabled = false;
                }
                //---
                checkedListBox_Config_Step6.Items.Clear();
                if (checkedListBox_Config_Step2.SelectedItem.Equals("EDIT LOCATION STATUS"))
                {
                    checkedListBox_Config_Step6.Items.Add("Avaliable");
                    checkedListBox_Config_Step6.Items.Add("Unavaliable");
                    checkedListBox_Config_Step6.Items.Add("N2 Purge");
                    checkedListBox_Config_Step6.Items.Add("Charging");
                }
                else
                {
                    checkedListBox_Config_Step6.Items.Add("Avaliable");
                    checkedListBox_Config_Step6.Items.Add("Unavaliable");
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
        private void checkedListBox_Config_Step3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (checkedListBox_Config_Step3.CheckedItems.Count != 0) groupBox_Config_Step4.Enabled = true;
            //else groupBox_Config_Step4.Enabled = false;
            CommonFunction comm = new CommonFunction();
            try
            {
                if (checkedListBox_Config_Step3.CheckedItems.Count > 0)
                {
                    groupBox_Config_Step4.Enabled = true;
                    string errMessage = "";
                    string type = checkedListBox_Config_Step1.SelectedItem.ToString();
                    string subtype = checkedListBox_Config_Step3.SelectedItem.ToString();
                    DataSet EquipmentBaseOnTypeAndSubtype = ServiceHelper.GetService().QueryEquipmentBaseOnTypeAndSubtype(type, subtype, ref errMessage);
                    checkedListBox_Config_Step4.Items.Clear();
                    foreach (DataRow datarow in EquipmentBaseOnTypeAndSubtype.Tables[0].Rows)
                    {
                        checkedListBox_Config_Step4.Items.Add(datarow["NAME"].ToString());
                    }
                }
                else
                {
                    groupBox_Config_Step4.Enabled = false;
                    groupBox_Config_Step5.Enabled = false;
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
        private void button_Config_Step5_Confirn_Click(object sender, EventArgs e)
        {
            CommonFunction comm = new CommonFunction();
            try
            {
                disableAllGroupBox();
                string errStr = string.Empty, group = string.Empty, function = string.Empty, subtype = string.Empty, name = string.Empty;
                string[] EquipmentList = null;
                int rowCount = 0, colCount = 0;
                try { group = checkedListBox_Config_Step1.SelectedItem.ToString(); } catch { }
                try { function = checkedListBox_Config_Step2.SelectedItem.ToString(); } catch { }
                try { subtype = checkedListBox_Config_Step3.SelectedItem.ToString(); } catch { }
                try
                {
                    if (function == "INSERT EQP") EquipmentList = (!String.IsNullOrEmpty(textBox_Config_Step4.Text.Trim())) ? textBox_Config_Step4.Lines : null;
                    else if (function == "DELETE EQP") name = checkedListBox_Config_Step4.SelectedItem.ToString();
                }
                catch (Exception ex)
                {
                    string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                    comm.LogRecordFun("EXCEPTION",
                                      System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                      logpath);
                }
                if (group == "ERACK")
                {
                    if (subtype == "FOUP CARRIER")
                    {
                        colCount = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "FOUP_COLUMN"));
                        rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "FOUP_ROW"));
                    }
                    else
                    {
                        rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "RETICLE_ROW"));
                        colCount = int.Parse(comm.ReadIni("CONFIG.INI", "ERACK", "RETICLE_COLUMN"));
                    }
                    if (function == "INSERT EQP")
                    {
                        foreach (var equipmentid in EquipmentList)
                        {
                            bool result1 = ServiceHelper.GetService().InsertERackDataToRackCurrentInfo(equipmentid, group, subtype, 1, rowCount, colCount, ref errStr);
                            if (!result1) MessageBox.Show("Insert Data Failed, because " + errStr, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                            {
                                bool result2 = ServiceHelper.GetService().InsertERackDataToRackLocationStatus(equipmentid, group, subtype, 1, rowCount, colCount, ref errStr);
                                if (!result2) MessageBox.Show("Insert Data Failed, because " + errStr, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                else
                                {
                                    bool result3 = ServiceHelper.GetService().InsertERackDataToRackBasicInfo(equipmentid, group, subtype, (rowCount * colCount).ToString(), " ", " ", " ", " ", ref errStr);
                                    if (!result3) MessageBox.Show("Insert Data Failed, because " + errStr, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    if (result1 && result2 && result3)
                                    {
                                        string errMessage = "";
                                        DataSet EquipmentBaseOnTypeAndSubtype = ServiceHelper.GetService().QueryEquipmentBaseOnTypeAndSubtype(checkedListBox_Config_Step1.SelectedItem.ToString(), checkedListBox_Config_Step3.SelectedItem.ToString(), ref errMessage);
                                        checkedListBox_Config_Step4.Items.Clear();
                                        foreach (DataRow datarow in EquipmentBaseOnTypeAndSubtype.Tables[0].Rows)
                                        {
                                            checkedListBox_Config_Step4.Items.Add(datarow["NAME"].ToString());
                                        }
                                        MessageBox.Show("Insert Data Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                        }
                    }
                    else if (function == "DELETE EQP")
                    {
                        bool result1 = ServiceHelper.GetService().deleteERackDataFromRackCurrentInfo(name, ref errStr);
                        bool result2 = ServiceHelper.GetService().deleteERackDataFromRackLocationStatus(name, ref errStr);
                        bool result3 = ServiceHelper.GetService().deleteERackDataFromRackBasicInfo(name, ref errStr);
                        if (result1 && result2 && result3)
                        {
                            string errMessage = "";
                            DataSet EquipmentBaseOnTypeAndSubtype = ServiceHelper.GetService().QueryEquipmentBaseOnTypeAndSubtype(checkedListBox_Config_Step1.SelectedItem.ToString(), checkedListBox_Config_Step3.SelectedItem.ToString(), ref errMessage);
                            checkedListBox_Config_Step4.Items.Clear();
                            foreach (DataRow datarow in EquipmentBaseOnTypeAndSubtype.Tables[0].Rows)
                            {
                                checkedListBox_Config_Step4.Items.Add(datarow["NAME"].ToString());
                            }
                            MessageBox.Show("Delete Data Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else MessageBox.Show("Delete Data Failed, because " + errStr, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (function == "EDIT EQP STATUS")
                    {
                        bool result = ServiceHelper.GetService().updateLocationStatus(checkedListBox_Config_Step4.Text,
                                                                                     "ALL",
                                                                                     (checkedListBox_Config_Step6.Text == "Unavaliable" ? "1" : "0"),
                                                                                     ref errStr);
                        if (result) MessageBox.Show("Update EQP Status Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else MessageBox.Show("Update EQP Status Failed, Equipment " + checkedListBox_Config_Step4.Text + ", Error " + errStr,
                                             "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (function == "EDIT LOCATION STATUS")
                    {
                        bool result = true;
                        foreach (var item in checkedListBox_Config_Step5.CheckedItems)
                        {
                            result = ServiceHelper.GetService().updateLocationStatus(checkedListBox_Config_Step4.Text,
                                                                                     item.ToString(),
                                                                                     (checkedListBox_Config_Step6.Text == "Unavaliable" ? "1" : "0"),
                                                                                     ref errStr);
                            if (!result)
                            {
                                MessageBox.Show("Update Data Failed, Equipment " + checkedListBox_Config_Step4.Text + " Error" + errStr,
                                                "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                        if (result) MessageBox.Show("Update Location Status Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    initialCondition();
                }
                else if (group == "STOCKER")
                {
                    int uselessCount = 0;
                    if (subtype == "FOUP CARRIER")
                    {
                        rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "ROW"));
                        colCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "COLUMN"));
                        uselessCount = 30;
                    }
                    else
                    {
                        rowCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "RETICLE_ROW"));
                        colCount = int.Parse(comm.ReadIni("CONFIG.INI", "STOCKER", "RETICLE_COLUMN"));
                        uselessCount = 24;
                    }
                    if (function == "INSERT EQP")
                    {
                        foreach (var equipmentid in EquipmentList)
                        {

                            bool result1 = ServiceHelper.GetService().InsertStockerDataToRackCurrentInfo(equipmentid, group, subtype, 2, rowCount, colCount, ref errStr);
                            if (!result1) MessageBox.Show("Insert Data Failed, because " + errStr, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                            {
                                bool result2 = ServiceHelper.GetService().InsertStockerDataToRackLocationStatus(equipmentid, group, subtype, 2, rowCount, colCount, ref errStr);
                                if (!result2) MessageBox.Show("Insert Data Failed, because " + errStr, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                else
                                {
                                    bool result3 = ServiceHelper.GetService().InsertStockerDataToRackBasicInfo(equipmentid, group, subtype, (rowCount * colCount * 2 - uselessCount).ToString(), "127.0.0.1", "5000", "0", " ", ref errStr);
                                    if (!result3) MessageBox.Show("Insert Data Failed, because " + errStr, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    if (result1 && result2 && result3)
                                    {
                                        string errMessage = "";
                                        DataSet EquipmentBaseOnTypeAndSubtype = ServiceHelper.GetService().QueryEquipmentBaseOnTypeAndSubtype(checkedListBox_Config_Step1.SelectedItem.ToString(), checkedListBox_Config_Step3.SelectedItem.ToString(), ref errMessage);
                                        checkedListBox_Config_Step4.Items.Clear();
                                        foreach (DataRow datarow in EquipmentBaseOnTypeAndSubtype.Tables[0].Rows)
                                        {
                                            checkedListBox_Config_Step4.Items.Add(datarow["NAME"].ToString());
                                        }
                                        MessageBox.Show("Insert Data Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                        }
                    }
                    else if (function == "DELETE EQP")
                    {
                        bool result1 = ServiceHelper.GetService().deleteERackDataFromRackCurrentInfo(name, ref errStr);
                        bool result2 = ServiceHelper.GetService().deleteERackDataFromRackLocationStatus(name, ref errStr);
                        bool result3 = ServiceHelper.GetService().deleteERackDataFromRackBasicInfo(name, ref errStr);
                        if (result1 && result2 && result3)
                        {
                            string errMessage = "";
                            DataSet EquipmentBaseOnTypeAndSubtype = ServiceHelper.GetService().QueryEquipmentBaseOnTypeAndSubtype(checkedListBox_Config_Step1.SelectedItem.ToString(), checkedListBox_Config_Step3.SelectedItem.ToString(), ref errMessage);
                            checkedListBox_Config_Step4.Items.Clear();
                            foreach (DataRow datarow in EquipmentBaseOnTypeAndSubtype.Tables[0].Rows)
                            {
                                checkedListBox_Config_Step4.Items.Add(datarow["NAME"].ToString());
                            }
                            MessageBox.Show("Delete Data Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else MessageBox.Show("Delete Data Failed, because " + errStr, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (function == "EDIT EQP STATUS")
                    {
                        bool result = ServiceHelper.GetService().updateLocationStatus(checkedListBox_Config_Step4.Text,
                                                                                     "ALL",
                                                                                     (checkedListBox_Config_Step6.Text == "Unavaliable" ? "1" : "0"),
                                                                                     ref errStr);
                        if (result) MessageBox.Show("Update EQP Status Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else MessageBox.Show("Update EQP Status Failed, Equipment " + checkedListBox_Config_Step4.Text + ", Error " + errStr,
                                             "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (function == "EDIT LOCATION STATUS")
                    {
                        bool result = true;
                        string status = string.Empty;
                        if (checkedListBox_Config_Step6.Text == "Avaliable") status = "0";
                        else if (checkedListBox_Config_Step6.Text == "Unavaliable") status = "1";
                        else if (checkedListBox_Config_Step6.Text == "N2 Purge") status = "2";
                        else if (checkedListBox_Config_Step6.Text == "Charging") status = "3";
                        foreach (var item in checkedListBox_Config_Step5.CheckedItems)
                        {
                            result = ServiceHelper.GetService().updateLocationStatus(checkedListBox_Config_Step4.Text,
                                                                                     item.ToString(),
                                                                                     status,
                                                                                     ref errStr);
                            if (!result)
                            {
                                MessageBox.Show("Update Data Failed, Equipment " + checkedListBox_Config_Step4.Text + " Error" + errStr,
                                                "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                        if (result) MessageBox.Show("Update Location Status Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    initialCondition();
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
        private void comboBox_AlarmRecord_EquipmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonFunction comm = new CommonFunction();
            try
            {
                getEquipmentID_AlarmRecord(0, true);
                QueryAlarmRecord(comboBox_AlarmRecord_EquipmentType.Text,
                                 comboBox_AlarmRecord_EquipmentID.Text);
            }
            catch (Exception ex)
            {
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        private void comboBox_AlarmRecord_EquipmentID_SelectedValueChanged(object sender, EventArgs e)
        {
            CommonFunction comm = new CommonFunction();
            try
            {
                groupBox_AlarmRecord_SelectOneEquipment.Focus();
                QueryAlarmRecord(comboBox_AlarmRecord_EquipmentType.Text,
                                 comboBox_AlarmRecord_EquipmentID.Text);
            }
            catch (Exception ex)
            {
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        private void comboBox_FOUPRecord_EquipmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonFunction comm = new CommonFunction();
            try
            {
                getEquipmentID_FOUPRecord(0, true);
                QueryFOUPCurrentRecord(comboBox_FOUPRecord_EquipmentType.Text,
                                       comboBox_FOUPRecord_EquipmentID.Text);
            }
            catch (Exception ex)
            {
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
        }
        private void comboBox_FOUPRecord_EquipmentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryFOUPCurrentRecord(comboBox_FOUPRecord_EquipmentType.Text,
                                   comboBox_FOUPRecord_EquipmentID.Text);
        }
        private void comboBox_FOUPRecord_EquipmentID_SelectedValueChanged(object sender, EventArgs e)
        {
        }
        private void comboBox_FOUPRecord_FOUPID_SelectedIndexChanged(object sender, EventArgs e)
        {
            dateTimePicker_FOUPRecord_End.Focus();
            QueryFOUPHistoryRecord(comboBox_FOUPRecord_EquipmentType.Text,
                                   comboBox_FOUPRecord_EquipmentID.Text,
                                   comboBox_FOUPRecord_FOUPID.Text,
                                   string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_FOUPRecord_Begin.Value),
                                   string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_FOUPRecord_End.Value));
        }
        private void DisableSomthing(bool flag)
        {
            comboBox_FOUPRecord_FOUPID.Enabled = flag;
            //dateTimePicker_FOUPRecord_Begin.Enabled = flag;
            dateTimePicker_FOUPRecord_End.Enabled = flag;
        }
        private void checkBox_FOUPRecord_HistoryData_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_FOUPRecord_HistoryData.Enabled = false;
            CommonFunction comm = new CommonFunction();
            try
            {
                if (checkBox_FOUPRecord_HistoryData.Checked)
                {
                    DisableSomthing(true);
                    groupBox_FOUPRecord.Enabled = false;
                    QueryFOUPHistoryFOUPList(comboBox_FOUPRecord_EquipmentType.Text,
                                             comboBox_FOUPRecord_EquipmentID.Text,
                                             comboBox_FOUPRecord_FOUPID.Text,
                                             string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_FOUPRecord_Begin.Value),
                                             string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_FOUPRecord_End.Value));
                    QueryFOUPHistoryRecord(comboBox_FOUPRecord_EquipmentType.Text,
                                       comboBox_FOUPRecord_EquipmentID.Text,
                                       comboBox_FOUPRecord_FOUPID.Text,
                                       string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_FOUPRecord_Begin.Value),
                                       string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_FOUPRecord_End.Value));
                }
                else
                {
                    DisableSomthing(false);
                    groupBox_FOUPRecord.Enabled = true;
                    setTimePicker();
                    QueryFOUPCurrentRecord(comboBox_FOUPRecord_EquipmentType.Text,
                                           comboBox_FOUPRecord_EquipmentID.Text);
                }
            }
            catch (Exception ex)
            {
                string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                comm.LogRecordFun("EXCEPTION",
                                  System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                  logpath);
            }
            finally
            {
                checkBox_FOUPRecord_HistoryData.Enabled = true;
            }

        }
        private void dateTimePicker_FOUPRecord_End_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker_FOUPRecord_Begin.Value = dateTimePicker_FOUPRecord_End.Value.AddDays(-7);
            QueryFOUPHistoryFOUPList(comboBox_FOUPRecord_EquipmentType.Text,
                                     comboBox_FOUPRecord_EquipmentID.Text,
                                     comboBox_FOUPRecord_FOUPID.Text,
                                     string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_FOUPRecord_Begin.Value),
                                     string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_FOUPRecord_End.Value));
            QueryFOUPHistoryRecord(comboBox_FOUPRecord_EquipmentType.Text,
                                   comboBox_FOUPRecord_EquipmentID.Text,
                                   comboBox_FOUPRecord_FOUPID.Text,
                                   string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_FOUPRecord_Begin.Value),
                                   string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_FOUPRecord_End.Value));
        }
        private void showProcessBar()
        {
            if (!BackGroundWorker.IsBusy)
            {
                BackGroundWorker.RunWorkerAsync(comboBox_DisplayTransfer_EquipmentID.Items.Count);
                progressBar.StartPosition = FormStartPosition.CenterParent;
                progressBar.ShowDialog();
            }
        }
        private void dateTimePicker_Transfer_End_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker_Transfer_Begin.Value = dateTimePicker_Transfer_End.Value.AddDays(-7);
            QueryTransferRecord(comboBox_Transfer_FOUPID.Text,
                                string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_Transfer_Begin.Value),
                                string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_Transfer_End.Value.AddDays(1)));
        }
        private void comboBox_Transfer_FOUPID_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryTransferRecord(comboBox_Transfer_FOUPID.Text,
                                string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_Transfer_Begin.Value),
                                string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_Transfer_End.Value.AddDays(1)));
        }
        private void checkedListBox_Config_Step4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkedListBox_Config_Step4.CheckedItems.Count > 0)
                {
                    if (checkedListBox_Config_Step2.SelectedItem.ToString().Contains("EDIT LOCATION STATUS"))
                    {
                        groupBox_Config_Step5.Enabled = true;
                    }
                    else groupBox_Config_Step5.Enabled = false;
                    string errMessage = string.Empty;
                    DataSet allLocationList = ServiceHelper.GetService().queryLocationList(checkedListBox_Config_Step4.Text, ref errMessage);
                    checkedListBox_Config_Step5.Items.Clear();
                    foreach (DataRow datarow in allLocationList.Tables[0].Rows)
                    {
                        checkedListBox_Config_Step5.Items.Add(datarow["LOCATION_ID"].ToString());
                    }
                }
                if (checkedListBox_Config_Step2.SelectedItem.ToString().Contains("EDIT EQP STATUS"))
                {
                    groupBox_Config_Step6.Enabled = true;
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
        private void checkedListBox_Config_Step5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkedListBox_Config_Step5.CheckedItems.Count > 0)
                {
                    groupBox_Config_Step6.Enabled = true;
                }
                else
                {
                    groupBox_Config_Step6.Enabled = false;
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
        private void checkedListBox_Config_SelectStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string loctionList = string.Empty;
                foreach (var item in checkedListBox_Config_Step5.CheckedItems)
                {
                    if (loctionList == string.Empty) loctionList = "'" + item.ToString() + "'";
                    else loctionList = loctionList + " or " + "'" + item.ToString() + "'";
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
        private void checkedListBox_Config_SelectStatus_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            onlySelectOneItem(sender, e);
        }
        private void button_Config_Step5_Clear_Click(object sender, EventArgs e)
        {
            initialCondition();
        }
        private void initialCondition()
        {
            clearCheckedListBox(checkedListBox_Config_Step1);
            clearCheckedListBox(checkedListBox_Config_Step2);
            clearCheckedListBox(checkedListBox_Config_Step3);
            clearCheckedListBox(checkedListBox_Config_Step4);
            clearCheckedListBox(checkedListBox_Config_Step5);
            clearCheckedListBox(checkedListBox_Config_Step6);
            textBox_Config_Step4.Clear();
            QueryBasicSetting();
            groupBox_Config_Step1.Enabled = true;
            groupBox_Config_Step2.Enabled = false;
            groupBox_Config_Step3.Enabled = false;
            groupBox_Config_Step4.Enabled = false;
            groupBox_Config_Step5.Enabled = false;
            groupBox_Config_Step6.Enabled = false;
        }
        private void disableAllGroupBox()
        {
            groupBox_Config_Step1.Enabled = false;
            groupBox_Config_Step2.Enabled = false;
            groupBox_Config_Step3.Enabled = false;
            groupBox_Config_Step4.Enabled = false;
            groupBox_Config_Step5.Enabled = false;
            groupBox_Config_Step6.Enabled = false;
        }
        private void button_LogOut_Click(object sender, EventArgs e)
        {
            Process process = Process.GetCurrentProcess();
            Process[] aProcess = Process.GetProcessesByName(process.ProcessName);
            foreach (Process saygoodbye in aProcess) saygoodbye.Kill();
        }
        private void checkedListBox_authority_step1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            onlySelectOneItem(sender, e);
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabControl_Authority.TabPages.Count > 0)
                    tabControl_Authority.TabPages.RemoveAt(0);
                TabPage tabpage = new TabPage();
                if (checkedListBox_Authority.Text == "Role Setting")
                {
                    MCSUI.Authority.RoleSetting rolesetting = new MCSUI.Authority.RoleSetting(textBox_Display_LoginUser.Text);
                    rolesetting.Dock = DockStyle.Fill;
                    tabpage.Controls.Add(rolesetting);
                    tabControl_Authority.TabPages.Add(tabpage);
                }
                else if (checkedListBox_Authority.Text == "Binding Authority")
                {
                    MCSUI.Authority.BindingAuthority bindingauthority = new MCSUI.Authority.BindingAuthority(textBox_Display_LoginUser.Text);
                    bindingauthority.Dock = DockStyle.Fill;
                    tabpage.Controls.Add(bindingauthority);
                    tabControl_Authority.TabPages.Add(tabpage);
                }
                else if (checkedListBox_Authority.Text == "Personal Setting")
                {
                    MCSUI.Authority.PersonalSetting personalsetting = new MCSUI.Authority.PersonalSetting(textBox_Display_LoginUser.Text);
                    personalsetting.Dock = DockStyle.Fill;
                    tabpage.Controls.Add(personalsetting);
                    tabControl_Authority.TabPages.Add(tabpage);
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
        private void checkedListBox_Authority_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            onlySelectOneItem(sender, e);
        }
        private void button_TransferOut_Click(object sender, EventArgs e)
        {
            Form_BatchTransferOut batchtransferout = new Form_BatchTransferOut(comboBox_DisplayTransfer_EquipmentID.Text,
                                                                               textBox_Display_LoginUser.Text);
            batchtransferout.Show();
        }
        private void button_N2Purge_Click(object sender, EventArgs e)
        {
            Form_BatchPurgeN2 batchpurgen2 = new Form_BatchPurgeN2(comboBox_DisplayTransfer_EquipmentID.Text,
                                                                   textBox_Display_LoginUser.Text);
            batchpurgen2.Show();
        }
        private void comboBox_N2Purge_FoupID_TabIndexChanged(object sender, EventArgs e)
        {
            QueryN2PurgeRecord(comboBox_N2Purge_FoupID.Text,
                               string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_Transfer_Begin.Value),
                               string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_Transfer_End.Value.AddDays(1)));
        }
        private void comboBox_TaskInfo_TaskType_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryTaskInfo(comboBox_TaskInfo_TaskType.Text,
                          comboBox_TaskInfo_TaskCradle.Text,
                          comboBox_TaskInfo_FOOUPID.Text,
                          comboBox_TaskInfo_TaskStatus.Text,
                          string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_Begin.Value),
                          string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_End.Value.AddDays(1)));
        }
        private void comboBox_TaskInfo_TaskCradle_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryTaskInfo(comboBox_TaskInfo_TaskType.Text,
                          comboBox_TaskInfo_TaskCradle.Text,
                          comboBox_TaskInfo_FOOUPID.Text,
                          comboBox_TaskInfo_TaskStatus.Text,
                          string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_Begin.Value),
                          string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_End.Value.AddDays(1)));
        }
        private void comboBox_TaskInfo_FOOUPID_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryTaskInfo(comboBox_TaskInfo_TaskType.Text,
                          comboBox_TaskInfo_TaskCradle.Text,
                          comboBox_TaskInfo_FOOUPID.Text,
                          comboBox_TaskInfo_TaskStatus.Text,
                          string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_Begin.Value),
                          string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_End.Value.AddDays(1)));
        }
        private void dateTimePicker_TaskInfo_End_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker_TaskInfo_Begin.Value = dateTimePicker_TaskInfo_End.Value.AddDays(-1);
            QueryTaskInfo(comboBox_TaskInfo_TaskType.Text,
                          comboBox_TaskInfo_TaskCradle.Text,
                          comboBox_TaskInfo_FOOUPID.Text,
                          comboBox_TaskInfo_TaskStatus.Text,
                          string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_Begin.Value),
                          string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_End.Value.AddDays(1)));
        }
        private void comboBox_TaskInfo_TaskStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryTaskInfo(comboBox_TaskInfo_TaskType.Text,
                          comboBox_TaskInfo_TaskCradle.Text,
                          comboBox_TaskInfo_FOOUPID.Text,
                          comboBox_TaskInfo_TaskStatus.Text,
                          string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_Begin.Value),
                          string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_End.Value.AddDays(1)));
        }
        private void dataGridView_Config_ShowSomething_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string itemName = string.Empty;
                if (dataGridView_Config_ShowSomething.Rows[dataGridView_Config_ShowSomething.CurrentCell.RowIndex].Cells[0].Value.ToString() == "STOCKER")
                {
                    switch (dataGridView_Config_ShowSomething.CurrentCell.ColumnIndex)
                    {
                        case 4:
                            itemName = "HSMS IP";
                            break;
                        case 5:
                            itemName = "HSMS Port";
                            break;
                        case 6:
                            itemName = "HSMS Device ID";
                            break;
                    }
                    if (itemName != string.Empty)
                    {
                        MCSUI.Form_Edit form_edit = new MCSUI.Form_Edit(dataGridView_Config_ShowSomething.Rows[dataGridView_Config_ShowSomething.CurrentCell.RowIndex].Cells[2].Value.ToString(),
                                                                    itemName);
                        form_edit.delegateEvent += new MCSUI.Form_Edit.delegateSetting(setting);
                        form_edit.Show();
                    }
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
        private void tabControl_Main_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.tabControl_Main.SelectedIndex)
            {
                case 1:
                    getEquipmentType_FOUPRecord(0, true);
                    QueryFOUPCurrentRecord(comboBox_FOUPRecord_EquipmentType.Text, comboBox_FOUPRecord_EquipmentID.Text);
                    break;
                case 2:
                    QueryTaskInfoCondition("ALL", "ALL", "ALL", "ALL",
                                       string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_Begin.Value),
                                       string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_End.Value.AddDays(1)));
                    QueryTaskInfo(comboBox_TaskInfo_TaskType.Text,
                                  comboBox_TaskInfo_TaskCradle.Text,
                                  comboBox_TaskInfo_FOOUPID.Text,
                                  comboBox_TaskInfo_TaskStatus.Text,
                                  string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_Begin.Value),
                                  string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_TaskInfo_End.Value.AddDays(1)));                    
                    break;
                case 3:
                    QueryN2PurgeFOUPIDList("ALL",
                                            string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_N2Purge_Begin.Value),
                                            string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_N2Purge_End.Value.AddDays(1)));
                    QueryN2PurgeRecord("ALL",
                                         string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_N2Purge_Begin.Value),
                                         string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_N2Purge_End.Value.AddDays(1)));
                    break;
                case 4:
                    QueryTransferFOUPIDList("ALL",
                                            string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_Transfer_Begin.Value),
                                            string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_Transfer_End.Value.AddDays(1)));
                    QueryTransferRecord("ALL",
                                        string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_Transfer_Begin.Value),
                                        string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_Transfer_End.Value.AddDays(1)));
                    break;
                case 5:
                    getEquipmentType_AlarmRecord(0, true);
                    QueryAlarmRecord(comboBox_AlarmRecord_EquipmentType.Text, comboBox_AlarmRecord_EquipmentID.Text);
                    break;
            }
        }
        private void label_DisplayTransfer_Destination_TextChanged(object sender, EventArgs e)
        {
            string locationStatus = string.Empty;
            string strResult = string.Empty;
            DataSet dsLocationStatus = ServiceHelper.GetService().getLocationStatus(comboBox_DisplayTransfer_EquipmentID.Text, label_DisplayTransfer_Destination.Text, ref strResult);
            foreach (DataRow datarow in dsLocationStatus.Tables[0].Rows) locationStatus = datarow["statuscode"].ToString();
            if (locationStatus == "3") button_DisplayTransfer_Start.Text = "Charge";
            else
            {
                if (checkBox_CycleTransfer.Checked == true)
                {
                    textBox_CycleTransfer.Enabled = true;
                    button_DisplayTransfer_Start.Text = "Start";
                }
                else
                {
                    textBox_CycleTransfer.Enabled = false;
                    button_DisplayTransfer_Start.Text = "Transfer";
                    button_DisplayTransfer_Clear.Text = "Clear";
                }
            }
        }
        private void button_Refrush_Click(object sender, EventArgs e)
        {
            //DialogResult dialogresult = MessageBox.Show("MCS Server Will Cancel All Transfer Job After Refrushing Data Grid, Do You Want To Continue ?", "MCS Client", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            //if (dialogresult == DialogResult.OK)
            //{
                button_DisplayTransfer_Start.Enabled = true;
                if (CommonFunction.cycletransferlist.Count > 0) CommonFunction.cycletransferlist.Clear();
                CommonFunction comm = new CommonFunction();
                string message_field = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "MESSAGE_FIELD");
                var subject = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "SUBJECT");
                var network = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "NETWORK");
                var port = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "PORT");
                var deamon = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "DEAMON");
                string[] deamonlist = deamon.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                Transport transport = CreateNetPort(port, network, deamonlist);
                var message = new TIBCO.Rendezvous.Message();
                message.SendSubject = subject;
                message.AddField(message_field, comm.createFormattedXML(comm.RefreshStockAllInfo(comboBox_DisplayTransfer_EquipmentID.Text, string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now))));
                comm.LogRecordFun("TIBCORV", "Send To EAP, " + comm.createFormattedXML(comm.RefreshStockAllInfo(comboBox_DisplayTransfer_EquipmentID.Text, string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now))), comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH"));
                SendTibcoRVMessage(transport, message);
            //}
        }
        private void checkBox_CycleTransfer_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_CycleTransfer.Checked == true)
            {
                textBox_CycleTransfer.Enabled = true;
                button_DisplayTransfer_Start.Text = "Start";
                button_DisplayTransfer_Clear.Text = "Cancel";
                try
                {
                    CycleTransferListNode node = CommonFunction.cycletransferlist[comboBox_DisplayTransfer_EquipmentID.Text];
                    label_DisplayTransfer_Source.Text = node.source;
                    label_DisplayTransfer_FoupID.Text = node.foupid;
                    label_DisplayTransfer_Destination.Text = node.destination;
                    textBox_CycleTransfer.Text = node.numberoftimes;
                    button_DisplayTransfer_Start.Enabled = false;
                }
                catch
                {
                    label_DisplayTransfer_Source.Text = string.Empty;
                    label_DisplayTransfer_FoupID.Text = string.Empty;
                    label_DisplayTransfer_Destination.Text = string.Empty;
                    textBox_CycleTransfer.Text = string.Empty;
                    button_DisplayTransfer_Start.Enabled = true;
                }
            }
            else
            {
                textBox_CycleTransfer.Enabled = false;
                button_DisplayTransfer_Start.Text = "Transfer";
                button_DisplayTransfer_Clear.Text = "Clear";
                label_DisplayTransfer_Source.Text = string.Empty;
                label_DisplayTransfer_FoupID.Text = string.Empty;
                label_DisplayTransfer_Destination.Text = string.Empty;
                button_DisplayTransfer_Start.Enabled = true;
                textBox_CycleTransfer.Text = string.Empty;
            }
        }
        private void label_DisplayTransfer_Source_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            label_DisplayTransfer_Source.Text = string.Empty;
            label_DisplayTransfer_FoupID.Text = string.Empty;
        }
        private void label_DisplayTransfer_Destination_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            label_DisplayTransfer_Destination.Text = string.Empty;
        }
        private void comboBox_CycleTransfer_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_DisplayTransfer_Start.Focus();
        }
        private void dateTimePicker_N2Purge_End_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker_N2Purge_Begin.Value = dateTimePicker_N2Purge_End.Value.AddDays(-7);
            QueryN2PurgeFOUPIDList(comboBox_N2Purge_FoupID.Text,
                                   string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_N2Purge_Begin.Value),
                                   string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_N2Purge_End.Value.AddDays(1)));
            QueryN2PurgeRecord(comboBox_N2Purge_FoupID.Text,
                               string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_N2Purge_Begin.Value),
                               string.Format("{0:yyyyMMddHHmmss}", dateTimePicker_N2Purge_End.Value.AddDays(1)));

        }
        private void tabControl_Main_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font fntTab;
            Brush bshBack;
            Brush bshFore;
            if (e.Index == this.tabControl_Main.SelectedIndex)
            {
                fntTab = new Font(e.Font, FontStyle.Bold);
                bshBack = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, SystemColors.Control, SystemColors.Control, System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);
                bshFore = Brushes.Black;
                bshBack = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.LightSlateGray, Color.LightGray, System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);
                //背景顏色為兩色的漸層 bshBack = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.FromArgb(81, 3, 133), Color.FromArgb(81, 3, 133), System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);//背景色為純色
                bshFore = Brushes.Black;//Blue;
            }
            else
            {
                fntTab = e.Font;
                bshBack = new SolidBrush(SystemColors.Control);
                bshFore = new SolidBrush(Color.Black);
                bshBack = new SolidBrush(SystemColors.Control);
                bshFore = new SolidBrush(Color.Black);
            }
            string tabName = this.tabControl_Main.TabPages[e.Index].Text;
            StringFormat sftTab = new StringFormat();
            sftTab.Alignment = StringAlignment.Center;//標籤文字置中
            sftTab.LineAlignment = StringAlignment.Center;
            e.Graphics.FillRectangle(bshBack, e.Bounds);
            Rectangle recTab = e.Bounds;
            recTab = new Rectangle(recTab.X, recTab.Y + 4, recTab.Width, recTab.Height - 4);
            e.Graphics.DrawString(tabName, fntTab, bshFore, recTab, sftTab);
        }

        private void tabControl_Config_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font fntTab;
            Brush bshBack;
            Brush bshFore;
            if (e.Index == this.tabControl_Config.SelectedIndex)
            {
                fntTab = new Font(e.Font, FontStyle.Bold);
                bshBack = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, SystemColors.Control, SystemColors.Control, System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);
                bshFore = Brushes.Black;
                bshBack = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.LightSlateGray, Color.LightGray, System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);
                //背景顏色為兩色的漸層 bshBack = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.FromArgb(81, 3, 133), Color.FromArgb(81, 3, 133), System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);//背景色為純色
                bshFore = Brushes.Black;//Blue;
            }
            else
            {
                fntTab = e.Font;
                bshBack = new SolidBrush(SystemColors.Control);
                bshFore = new SolidBrush(Color.Black);
                bshBack = new SolidBrush(SystemColors.Control);
                bshFore = new SolidBrush(Color.Black);
            }
            string tabName = this.tabControl_Config.TabPages[e.Index].Text;
            StringFormat sftTab = new StringFormat();
            sftTab.Alignment = StringAlignment.Center;//標籤文字置中
            sftTab.LineAlignment = StringAlignment.Center;
            e.Graphics.FillRectangle(bshBack, e.Bounds);
            Rectangle recTab = e.Bounds;
            recTab = new Rectangle(recTab.X, recTab.Y + 4, recTab.Width, recTab.Height - 4);
            e.Graphics.DrawString(tabName, fntTab, bshFore, recTab, sftTab);
        }
        private void button_FOUPRecord_Export_Click(object sender, EventArgs e)
        {
            if (dataGridView_FOUPRecord_Display.Rows.Count == 0)
            {
                MessageBox.Show("Data is empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            //saveFileDialog.CreatePrompt = true;
            saveFileDialog.FileName = "FOUPRecord_" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            saveFileDialog.Title = "Export";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.GetEncoding(-0));
                string strLine = "";
                try
                {
                    //表头
                    for (int i = 0; i < dataGridView_FOUPRecord_Display.ColumnCount; i++)
                    {
                        if (i > 0)
                            strLine += ",";
                        strLine += dataGridView_FOUPRecord_Display.Columns[i].HeaderText;
                    }
                    strLine.Remove(strLine.Length - 1);
                    sw.WriteLine(strLine);
                    strLine = "";
                    //表的内容
                    for (int j = 0; j < dataGridView_FOUPRecord_Display.Rows.Count; j++)
                    {
                        strLine = "";
                        int colCount = dataGridView_FOUPRecord_Display.Columns.Count;
                        for (int k = 0; k < colCount; k++)
                        {
                            if (k > 0 && k < colCount)
                                strLine += ",";
                            if (dataGridView_FOUPRecord_Display.Rows[j].Cells[k].Value == null)
                                strLine += "";
                            else
                            {
                                string cell = dataGridView_FOUPRecord_Display.Rows[j].Cells[k].Value.ToString().Trim();
                                //防止里面含有特殊符号
                                cell = cell.Replace("\"", "\"\"");
                                cell = "\"" + cell + "\"";
                                strLine += cell;
                            }
                        }
                        sw.WriteLine(strLine);
                    }
                    sw.Close();
                    stream.Close();
                    MessageBox.Show("Export " + saveFileDialog.FileName.ToString(), "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Export Fail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void button_TaskInfo_Export_Click(object sender, EventArgs e)
        {
            if (dataGridView_TaskInfo.Rows.Count == 0)
            {
                MessageBox.Show("Data is empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            //saveFileDialog.CreatePrompt = true;
            saveFileDialog.FileName = "TaskInformation_" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            saveFileDialog.Title = "Export";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.GetEncoding(-0));
                string strLine = "";
                try
                {
                    //表头
                    for (int i = 0; i < dataGridView_TaskInfo.ColumnCount; i++)
                    {
                        if (i > 0)
                            strLine += ",";
                        strLine += dataGridView_TaskInfo.Columns[i].HeaderText;
                    }
                    strLine.Remove(strLine.Length - 1);
                    sw.WriteLine(strLine);
                    strLine = "";
                    //表的内容
                    for (int j = 0; j < dataGridView_TaskInfo.Rows.Count; j++)
                    {
                        strLine = "";
                        int colCount = dataGridView_TaskInfo.Columns.Count;
                        for (int k = 0; k < colCount; k++)
                        {
                            if (k > 0 && k < colCount)
                                strLine += ",";
                            if (dataGridView_TaskInfo.Rows[j].Cells[k].Value == null)
                                strLine += "";
                            else
                            {
                                string cell = dataGridView_TaskInfo.Rows[j].Cells[k].Value.ToString().Trim();
                                //防止里面含有特殊符号
                                cell = cell.Replace("\"", "\"\"");
                                cell = "\"" + cell + "\"";
                                strLine += cell;
                            }
                        }
                        sw.WriteLine(strLine);
                    }
                    sw.Close();
                    stream.Close();
                    MessageBox.Show("Export " + saveFileDialog.FileName.ToString(), "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Export Fail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void button_N2Purge_Export_Click(object sender, EventArgs e)
        {
            if (dataGridView_N2Purge.Rows.Count == 0)
            {
                MessageBox.Show("Data is empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            //saveFileDialog.CreatePrompt = true;
            saveFileDialog.FileName = "N2PurgeRecord_" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            saveFileDialog.Title = "Export";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.GetEncoding(-0));
                string strLine = "";
                try
                {
                    //表头
                    for (int i = 0; i < dataGridView_N2Purge.ColumnCount; i++)
                    {
                        if (i > 0)
                            strLine += ",";
                        strLine += dataGridView_N2Purge.Columns[i].HeaderText;
                    }
                    strLine.Remove(strLine.Length - 1);
                    sw.WriteLine(strLine);
                    strLine = "";
                    //表的内容
                    for (int j = 0; j < dataGridView_N2Purge.Rows.Count; j++)
                    {
                        strLine = "";
                        int colCount = dataGridView_N2Purge.Columns.Count;
                        for (int k = 0; k < colCount; k++)
                        {
                            if (k > 0 && k < colCount)
                                strLine += ",";
                            if (dataGridView_N2Purge.Rows[j].Cells[k].Value == null)
                                strLine += "";
                            else
                            {
                                string cell = dataGridView_N2Purge.Rows[j].Cells[k].Value.ToString().Trim();
                                //防止里面含有特殊符号
                                cell = cell.Replace("\"", "\"\"");
                                cell = "\"" + cell + "\"";
                                strLine += cell;
                            }
                        }
                        sw.WriteLine(strLine);
                    }
                    sw.Close();
                    stream.Close();
                    MessageBox.Show("Export " + saveFileDialog.FileName.ToString(), "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Export Fail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button_TransferRecord_Export_Click(object sender, EventArgs e)
        {
            if (dataGridView_TransferRecord_Display.Rows.Count == 0)
            {
                MessageBox.Show("Data is empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            //saveFileDialog.CreatePrompt = true;
            saveFileDialog.FileName = "TransferRecord_" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            saveFileDialog.Title = "Export";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.GetEncoding(-0));
                string strLine = "";
                try
                {
                    //表头
                    for (int i = 0; i < dataGridView_TransferRecord_Display.ColumnCount; i++)
                    {
                        if (i > 0)
                            strLine += ",";
                        strLine += dataGridView_TransferRecord_Display.Columns[i].HeaderText;
                    }
                    strLine.Remove(strLine.Length - 1);
                    sw.WriteLine(strLine);
                    strLine = "";
                    //表的内容
                    for (int j = 0; j < dataGridView_TransferRecord_Display.Rows.Count; j++)
                    {
                        strLine = "";
                        int colCount = dataGridView_TransferRecord_Display.Columns.Count;
                        for (int k = 0; k < colCount; k++)
                        {
                            if (k > 0 && k < colCount)
                                strLine += ",";
                            if (dataGridView_TransferRecord_Display.Rows[j].Cells[k].Value == null)
                                strLine += "";
                            else
                            {
                                string cell = dataGridView_TransferRecord_Display.Rows[j].Cells[k].Value.ToString().Trim();
                                //防止里面含有特殊符号
                                cell = cell.Replace("\"", "\"\"");
                                cell = "\"" + cell + "\"";
                                strLine += cell;
                            }
                        }
                        sw.WriteLine(strLine);
                    }
                    sw.Close();
                    stream.Close();
                    MessageBox.Show("Export " + saveFileDialog.FileName.ToString(), "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Export Fail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void button_AlarmRecord_Export_Click(object sender, EventArgs e)
        {
            if (dataGridView_AlarmRecord_Display.Rows.Count == 0)
            {
                MessageBox.Show("Data is empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            //saveFileDialog.CreatePrompt = true;
            saveFileDialog.FileName = "AlarmRecord_" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            saveFileDialog.Title = "Export";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.GetEncoding(-0));
                string strLine = "";
                try
                {
                    //表头
                    for (int i = 0; i < dataGridView_AlarmRecord_Display.ColumnCount; i++)
                    {
                        if (i > 0)
                            strLine += ",";
                        strLine += dataGridView_AlarmRecord_Display.Columns[i].HeaderText;
                    }
                    strLine.Remove(strLine.Length - 1);
                    sw.WriteLine(strLine);
                    strLine = "";
                    //表的内容
                    for (int j = 0; j < dataGridView_AlarmRecord_Display.Rows.Count; j++)
                    {
                        strLine = "";
                        int colCount = dataGridView_AlarmRecord_Display.Columns.Count;
                        for (int k = 0; k < colCount; k++)
                        {
                            if (k > 0 && k < colCount)
                                strLine += ",";
                            if (dataGridView_AlarmRecord_Display.Rows[j].Cells[k].Value == null)
                                strLine += "";
                            else
                            {
                                string cell = dataGridView_AlarmRecord_Display.Rows[j].Cells[k].Value.ToString().Trim();
                                //防止里面含有特殊符号
                                cell = cell.Replace("\"", "\"\"");
                                cell = "\"" + cell + "\"";
                                strLine += cell;
                            }
                        }
                        sw.WriteLine(strLine);
                    }
                    sw.Close();
                    stream.Close();
                    MessageBox.Show("Export " + saveFileDialog.FileName.ToString(), "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Export Fail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        static int iOperCount = 0;
        internal class MyMessager : IMessageFilter
        {
            public bool PreFilterMessage(ref System.Windows.Forms.Message m)
            {
                if (m.Msg == 0x201 || m.Msg == 256 || m.Msg == 260)
                {
                    iOperCount = 0;
                }
                return false;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            iOperCount++;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fileversioninfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string fileversion = fileversioninfo.FileVersion;
            if (iOperCount < 60) this.Text = " MCS Client, Version " + fileversion + "  [ Idle " + iOperCount + " s ]";
            else this.Text = " MCS Client, Version " + fileversion + "  [ Idle " + iOperCount / 60 + " min " + iOperCount % 60 + " s ]";
            if (iOperCount > 300)
            {
                CommonFunction comm = new CommonFunction();
                try
                {
                    Process process = Process.GetCurrentProcess();
                    Process[] aProcess = Process.GetProcessesByName(process.ProcessName);
                    foreach (Process saygoodbye in aProcess) saygoodbye.Kill();
                }
                catch (Exception ex)
                {
                    string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
                    comm.LogRecordFun("EXCEPTION",
                                      System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + ex.Message,
                                      logpath);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Frm_CurrentJobList currentjoblist = new Frm_CurrentJobList();
            currentjoblist.Show();
        }
        private void button_ClearJob_Click(object sender, EventArgs e)
        {
            DialogResult dialogresult = MessageBox.Show("MCS Server Will Cancel All Transfer Job, Do You Want To Continue ?", "MCS Client", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogresult == DialogResult.OK)
            {
                button_DisplayTransfer_Start.Enabled = true;
                if (CommonFunction.cycletransferlist.Count > 0) CommonFunction.cycletransferlist.Clear();
                CommonFunction comm = new CommonFunction();
                string message_field = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "MESSAGE_FIELD");
                var subject = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "SUBJECT");
                var network = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "NETWORK");
                var port = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "PORT");
                var deamon = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "DEAMON");
                string[] deamonlist = deamon.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                Transport transport = CreateNetPort(port, network, deamonlist);
                var message = new TIBCO.Rendezvous.Message();
                message.SendSubject = subject;
                message.AddField(message_field, comm.createFormattedXML(comm.ClearStockAllTransferJob(comboBox_DisplayTransfer_EquipmentID.Text, string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now))));
                comm.LogRecordFun("TIBCORV", "Send To EAP, " + comm.createFormattedXML(comm.RefreshStockAllInfo(comboBox_DisplayTransfer_EquipmentID.Text, string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now))), comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH"));
                SendTibcoRVMessage(transport, message);
            }
        }
    }
}