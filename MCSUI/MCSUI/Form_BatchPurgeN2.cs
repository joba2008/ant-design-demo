using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using TIBCO.Rendezvous;
using System.Diagnostics;

namespace MCSUI
{
    public partial class Form_BatchPurgeN2 : Form
    {
        public Form_BatchPurgeN2()
        {
            InitializeComponent();
        }
        public Form_BatchPurgeN2(string equipmentid)
        {
            InitializeComponent();
            string errMessage = string.Empty;
            label_BatchPurgeN2_EQPID.Text = equipmentid;
            DataSet allRecord = ServiceHelper.GetService().getRelationofLocationandFOUPID(equipmentid, ref errMessage);
            dataGridView_batchPurgeN2.DataSource = allRecord.Tables[0];
            dataGridView_batchPurgeN2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView_batchPurgeN2.DefaultCellStyle.Font = new Font("Consolas", 11);
        }
        public Form_BatchPurgeN2(string equipmentid, string username)
        {
            InitializeComponent();
            string errMessage = string.Empty;
            label_BatchPurgeN2_EQPID.Text = equipmentid;
            DataSet allRecord = ServiceHelper.GetService().getRelationofLocationandFOUPID(equipmentid, ref errMessage);
            dataGridView_batchPurgeN2.DataSource = allRecord.Tables[0];
            dataGridView_batchPurgeN2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView_batchPurgeN2.DefaultCellStyle.Font = new Font("Consolas", 11);
            label_batchPurgeN2_UserName.Text = username;
        }
        private void button_BatchPurgeN2_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "PurgeN2";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = label_BatchPurgeN2_EQPID.Text;
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);
                Header.AppendChild(TRANSACTIONID);
                XmlNode ORIGINALSOURCESUBJECTNAME = doc.CreateElement("ORIGINALSOURCESUBJECTNAME");
                ORIGINALSOURCESUBJECTNAME.InnerXml = "";
                Header.AppendChild(ORIGINALSOURCESUBJECTNAME);
                XmlNode SOURCESUBJECTNAME = doc.CreateElement("SOURCESUBJECTNAME");
                SOURCESUBJECTNAME.InnerXml = "";
                Header.AppendChild(SOURCESUBJECTNAME);
                XmlNode TARGETSUBJECTNAME = doc.CreateElement("TARGETSUBJECTNAME");
                TARGETSUBJECTNAME.InnerXml = "";
                Header.AppendChild(TARGETSUBJECTNAME);
                XmlNode EVENTUSER = doc.CreateElement("EVENTUSER");
                EVENTUSER.InnerXml = "";
                Header.AppendChild(EVENTUSER);
                XmlNode EVENTCOMMENT = doc.CreateElement("EVENTCOMMENT");
                EVENTCOMMENT.InnerXml = "";
                Header.AppendChild(EVENTCOMMENT);
                XmlNode RETURNCODE = doc.CreateElement("RETURNCODE");
                RETURNCODE.InnerXml = "";
                Header.AppendChild(RETURNCODE);
                XmlNode RETURNMESSAGE = doc.CreateElement("RETURNMESSAGE");
                RETURNMESSAGE.InnerXml = "";
                Header.AppendChild(RETURNMESSAGE);
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                XmlNode MACHINENAME = doc.CreateElement("MACHINENAME");
                MACHINENAME.InnerXml = label_BatchPurgeN2_EQPID.Text;
                Body.AppendChild(MACHINENAME);
                XmlNode USERNAME = doc.CreateElement("USERNAME");
                USERNAME.InnerXml = label_batchPurgeN2_UserName.Text;
                Body.AppendChild(USERNAME);
                XmlNode CARRIERLIST = doc.CreateElement("CARRIERLIST");
                for (int index = 0; index < dataGridView_batchPurgeN2.RowCount; index++)
                {
                    if (dataGridView_batchPurgeN2.Rows[index].DefaultCellStyle.BackColor == Color.LightGreen &&
                        dataGridView_batchPurgeN2.Rows[index].Cells[1].Value.ToString() != "")
                    {
                        XmlNode CARRIER = doc.CreateElement("CARRIER");
                        XmlNode CARRIERNAME = doc.CreateElement("CARRIERNAME");
                        CARRIERNAME.InnerText = dataGridView_batchPurgeN2.Rows[index].Cells[1].Value.ToString();
                        CARRIER.AppendChild(CARRIERNAME);
                        XmlNode LOCATION = doc.CreateElement("LOCATION");
                        LOCATION.InnerText = dataGridView_batchPurgeN2.Rows[index].Cells[0].Value.ToString();
                        CARRIER.AppendChild(LOCATION);
                        CARRIERLIST.AppendChild(CARRIER);
                    }
                }
                Body.AppendChild(CARRIERLIST);
                root.AppendChild(Body);
                doc.AppendChild(root);
                //---
                CommonFunction comm = new CommonFunction();
                string message_field = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "MESSAGE_FIELD");
                string subject = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "SUBJECT");
                string network = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "NETWORK");
                string port = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "PORT");
                string deamon = comm.ReadIni("CONFIG.INI", "TIBCORV_TO_EAP", "DEAMON");
                string[] deamonlist = deamon.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                Transport transport = CreateNetPort(port, network, deamonlist);
                var message = new TIBCO.Rendezvous.Message();
                message.SendSubject = subject;
                message.AddField(message_field, doc.InnerXml);
                comm.LogRecordFun("TIBCORV", "Send To EAP, " + comm.createFormattedXML(doc.InnerXml), comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH"));
                SendTibcoRVMessage(transport, message);
                this.Close();
                MessageBox.Show("MCS Client Send Command 'Purge' To MCS Server Complete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void dataGridView_batchPurgeN2_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView_batchPurgeN2.Rows[dataGridView_batchPurgeN2.CurrentCell.RowIndex].DefaultCellStyle.BackColor == Color.LightGreen)
                    dataGridView_batchPurgeN2.Rows[dataGridView_batchPurgeN2.CurrentCell.RowIndex].DefaultCellStyle.BackColor = Color.White;
                else
                    dataGridView_batchPurgeN2.Rows[dataGridView_batchPurgeN2.CurrentCell.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
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
        private void button_BatchPurgeN2_Search_Click(object sender, EventArgs e)
        {
            Search();
        }
        private void textBox_BatchPurgeN2_FOUPID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
            }
        }
        private void Search()
        {
            try
            {
                if (textBox_BatchPurgeN2_FOUPID.Text.Trim() != "")
                {
                    for (int index = 0; index < dataGridView_batchPurgeN2.RowCount; index++)
                    {
                        if (dataGridView_batchPurgeN2.Rows[index].Cells[1].Value.ToString() == textBox_BatchPurgeN2_FOUPID.Text.Trim())
                        {
                            dataGridView_batchPurgeN2.Rows[index].DefaultCellStyle.BackColor = Color.LightGreen;
                            if (index - 2 < 0) dataGridView_batchPurgeN2.FirstDisplayedScrollingRowIndex = 0;
                            else dataGridView_batchPurgeN2.FirstDisplayedScrollingRowIndex = index - 2;
                        }
                    }
                    textBox_BatchPurgeN2_FOUPID.Text = "";
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
    }
}
