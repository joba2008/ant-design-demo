using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Net;
using System.Drawing;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Configuration;
using TIBCO.Rendezvous;

namespace MCSUI
{
    public class CommonFunction
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
                                                             string key,
                                                             string val,
                                                           string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                                                          string key,
                                                          string def,
                                                          StringBuilder retVal,
                                                          int size,
                                                          string filePath);
        //public static LinkedList<CycleTransferListNode> cycletransferlist = new LinkedList<CycleTransferListNode>();
        public static Dictionary<string, CycleTransferListNode> cycletransferlist = new Dictionary<string, CycleTransferListNode>();
        public string ReadIni(string file_name, string section_name, string item_name)
        {
            try
            {
                //Process oProcess = Process.GetCurrentProcess();
                //String eqptid = oProcess.ProcessName.ToString();
                //StringBuilder rValofFrom_ini = new StringBuilder(5863);
                //int lenofIni_file = GetPrivateProfileString(section_name,
                //                                             item_name,
                //                                             "",
                //                                             rValofFrom_ini,
                //                                             5863,
                //                                             String.Format(@"{0}\{1}", System.AppDomain.CurrentDomain.BaseDirectory, file_name));
                //return rValofFrom_ini.ToString();
                string getInfo = "";
                //if (section_name == "LOGPATH" && item_name == "LOGPATH") getInfo = System.AppDomain.CurrentDomain.BaseDirectory + @"\Logs";
                //else getInfo = ConfigurationManager.AppSettings[section_name + "_" + item_name].ToString();
                getInfo = ConfigurationManager.AppSettings[section_name + "_" + item_name].ToString();
                return getInfo;
            }
            catch
            {
                return "";
            }
        }
        public void LogRecordFun(string LogAction, string LogStr, string LogPath)
        {
            try
            {
                string LogName, LogName_Delete, LogTime;
                LogTime = string.Format("[{0:yyyy/MM/dd HH:mm:ss.fff}] ", DateTime.Now);
                LogName = "Log_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".Log";
                LogName_Delete = "Log_" + String.Format("{0:yyyyMMdd}", DateTime.Now.AddDays(-8)) + ".Log";
                //--- Checking directory exist or not
                if (!Directory.Exists(LogPath + @"\" + LogAction + @"\"))
                {
                    Directory.CreateDirectory(LogPath + @"\" + LogAction + @"\");
                }
                using (StreamWriter StrWrite = new StreamWriter(Path.Combine(LogPath + @"\" + LogAction + @"\", LogName), true))
                {
                    StrWrite.WriteLine(LogTime + LogStr);
                    StrWrite.Close();
                    StrWrite.Dispose();
                }
                System.IO.File.Delete(Path.Combine(LogPath + @"\" + LogAction + @"\", LogName_Delete));
            }
            catch
            { }
        }
        public string StringToBinary(string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }
        public string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();
            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.ASCII.GetString(byteList.ToArray());
        }
        public double GetDelay(DateTime t_begin, DateTime t_end)
        {
            try
            {
                return (t_end.Subtract(t_begin)).Seconds;
            }
            catch
            {
                return -1;
            }
        }
        //for erack
        public Bitmap conbinePictureTexts(string fileName, string texts)
        {
            try
            {
                //string picturePath = String.Format(@"{0}\FIGURES\{1}", System.AppDomain.CurrentDomain.BaseDirectory, fileName.Trim());
                //var fontconverter = new FontConverter();
                //Bitmap bitmap = new Bitmap(picturePath);
                Bitmap bitmap = null;
                switch (fileName.Trim())
                {
                    //case "BL.png":
                    //    bitmap = Properties.Resources.BL;
                    //    break;
                    //case "BR.png":
                    //    bitmap = Properties.Resources.BR;
                    //    break;
                    //case "empty.png":
                    //    bitmap = Properties.Resources.empty;
                    //    break;
                    case "erack_empty.png":
                        bitmap = Properties.Resources.stocker_empty;
                        break;
                    case "erack_exist.png":
                        bitmap = Properties.Resources.stocker_exist;
                        break;
                    //case "HL.png":
                    //    bitmap = Properties.Resources.HL;
                    //    break;
                    //case "HR.png":
                    //    bitmap = Properties.Resources.HR;
                    //    break;
                    //case "loadport.png":
                    //    bitmap = Properties.Resources.loadport;
                    //    break;
                    //case "stocker_empty.png":
                    //    bitmap = Properties.Resources.stocker_empty;
                    //    break;
                    //case "stocker_exist.png":
                    //    bitmap = Properties.Resources.stocker_empty;
                    //    break;
                }
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                int textsize = int.Parse(ReadIni("CONFIG.INI", "DISPLAYBOARD", "SIZE"));
                float location_x = float.Parse(ReadIni("CONFIG.INI", "DISPLAYBOARD", "LOCATION_X"));
                float location_y = float.Parse(ReadIni("CONFIG.INI", "DISPLAYBOARD", "LOCATION_Y"));
                string font = ReadIni("CONFIG.INI", "DISPLAYBOARD", "FONT");
                Font objFont = new Font(font, textsize, FontStyle.Regular);
                SolidBrush solidbrush = new SolidBrush(Color.Black);
                graphics.DrawString(texts, objFont, solidbrush, new PointF(location_x, location_y));
                MemoryStream memorystream = new MemoryStream();
                bitmap.Save(memorystream, System.Drawing.Imaging.ImageFormat.Gif);
                return bitmap;
            }
            catch
            {
                return null;
            }
        }
        public Bitmap conbinePictureTexts_Stocker(string fileName, string texts)
        {
            try
            {
                //string picturePath = String.Format(@"{0}\FIGURES\{1}", System.AppDomain.CurrentDomain.BaseDirectory, fileName.Trim());
                //var fontconverter = new FontConverter();
                //Bitmap bitmap = new Bitmap(picturePath);
                Bitmap bitmap = null;
                switch (fileName.Trim())
                {
                    case "stocker_empty.png":
                        bitmap = Properties.Resources.stocker_empty;
                        break;
                    case "stocker_exist.png":
                        bitmap = Properties.Resources.stocker_exist;
                        break;
                }
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                int textsize = int.Parse(ReadIni("CONFIG.INI", "DISPLAYBOARD", "SIZE_STOCKER"));
                float location_x = float.Parse(ReadIni("CONFIG.INI", "DISPLAYBOARD", "LOCATION_X_STOCKER"));
                float location_y = float.Parse(ReadIni("CONFIG.INI", "DISPLAYBOARD", "LOCATION_Y_STOCKER"));
                string font = ReadIni("CONFIG.INI", "DISPLAYBOARD", "FONT");
                Font objFont = new Font(font, textsize, FontStyle.Regular);
                SolidBrush solidbrush = new SolidBrush(Color.Black);
                graphics.DrawString(texts, objFont, solidbrush, new PointF(location_x, location_y));
                MemoryStream memorystream = new MemoryStream();
                bitmap.Save(memorystream, System.Drawing.Imaging.ImageFormat.Gif);
                return bitmap;
            }
            catch
            {
                return null;
            }
        }
        //for stocker
        public Image getPicture(string fileName)
        {
            try
            {
                return Image.FromFile(String.Format(@"{0}\FIGURES\{1}", System.AppDomain.CurrentDomain.BaseDirectory, fileName.Trim()));
            }
            catch
            {
                return null;
            }
        }
        public string sendLocationToMainForm(string location, string foupid, string visiable)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "SENDLOCATIONTOMAINFORM";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = "";
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = "";
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                XmlNode LOCATION = doc.CreateElement("LOCATION");
                LOCATION.InnerXml = location;
                Body.AppendChild(LOCATION);
                XmlNode FOUPID = doc.CreateElement("FOUPID");
                FOUPID.InnerXml = foupid;
                Body.AppendChild(FOUPID);
                XmlNode VISIABLE = doc.CreateElement("VISIABLE");
                VISIABLE.InnerXml = visiable;
                Body.AppendChild(VISIABLE);
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch 
            {
                return "";
            }
        }
        public string clearTextBoxSearchFOUP()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "CLEARTEXTBOXSEARCHFOUP";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = "";
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = "";
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
                //--- 
                root.AppendChild(Header);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string SearchFOUPID(string equipmentType, string equipmentID, string foupID)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "SEARCHFOUPID";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = "";
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = "";
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                XmlNode EQUIPMENTTYPE = doc.CreateElement("EQUIPMENTTYPE");
                EQUIPMENTTYPE.InnerXml = equipmentType;
                Body.AppendChild(EQUIPMENTTYPE);
                XmlNode EQUIPMENTID = doc.CreateElement("EQUIPMENTID");
                EQUIPMENTID.InnerXml = equipmentID;
                Body.AppendChild(EQUIPMENTID);
                XmlNode FOUPID = doc.CreateElement("FOUPID");
                FOUPID.InnerXml = foupID;
                Body.AppendChild(FOUPID);
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string updateBasicInfo(string transactionID)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "UPDATEBASICINFO";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = "";
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = transactionID;
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string ShowMessage(string transactionID, string equipmentID, string statusDescription)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "SHOWMESSAGE";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = "";
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = transactionID;
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                XmlNode EQUIPMENTNAME = doc.CreateElement("EQUIPMENTNAME");
                EQUIPMENTNAME.InnerXml = equipmentID;
                Body.AppendChild(EQUIPMENTNAME);
                XmlNode STATUSDESCRIPTION = doc.CreateElement("STATUSDESCRIPTION");
                STATUSDESCRIPTION.InnerXml = statusDescription;
                Body.AppendChild(STATUSDESCRIPTION);
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string ShowAlarmMessage(string transactionID, string equipmentID, string equipmentType, string statusDescription)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "SHOWALARMMESSAGE";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = "";
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = transactionID;
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                XmlNode EQUIPMENTNAME = doc.CreateElement("EQUIPMENTNAME");
                EQUIPMENTNAME.InnerXml = equipmentID;
                Body.AppendChild(EQUIPMENTNAME);
                XmlNode EQUIPMENTTYPE = doc.CreateElement("EQUIPMENTTYPE");
                EQUIPMENTTYPE.InnerXml = equipmentType;
                Body.AppendChild(EQUIPMENTTYPE);
                XmlNode STATUSDESCRIPTION = doc.CreateElement("STATUSDESCRIPTION");
                STATUSDESCRIPTION.InnerXml = statusDescription;
                Body.AppendChild(STATUSDESCRIPTION);
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string QueryERackAllInfo(string equipmentName, string transactionID)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "QueryERackAllInfo";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = "";
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = transactionID;
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string CreateTransferJob(string transactionID, string machineID, string carrierID,
                                        string sourceLocation, string destinationLocation, string username)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "Transfer";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = machineID;
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = transactionID;
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                XmlNode MACHINENAME = doc.CreateElement("MACHINENAME");
                MACHINENAME.InnerXml = machineID;
                Body.AppendChild(MACHINENAME);
                XmlNode USERNAME = doc.CreateElement("USERNAME");
                USERNAME.InnerXml = username;
                Body.AppendChild(USERNAME);
                XmlNode CARRIERNAME = doc.CreateElement("CARRIERNAME");
                CARRIERNAME.InnerXml = carrierID;
                Body.AppendChild(CARRIERNAME);
                XmlNode FROMLOCATION = doc.CreateElement("FROMLOCATION");
                FROMLOCATION.InnerXml = sourceLocation;
                Body.AppendChild(FROMLOCATION);
                XmlNode TOLOCATION = doc.CreateElement("TOLOCATION");
                TOLOCATION.InnerXml = destinationLocation;
                Body.AppendChild(TOLOCATION);
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string CreateChargeJob(string transactionID, string machineID, string carrierID,
                                        string sourceLocation, string destinationLocation, string username)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "Charge";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = machineID;
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = transactionID;
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                XmlNode MACHINENAME = doc.CreateElement("MACHINENAME");
                MACHINENAME.InnerXml = machineID;
                Body.AppendChild(MACHINENAME);
                XmlNode USERNAME = doc.CreateElement("USERNAME");
                USERNAME.InnerXml = username;
                Body.AppendChild(USERNAME);
                XmlNode CARRIERNAME = doc.CreateElement("CARRIERNAME");
                CARRIERNAME.InnerXml = carrierID;
                Body.AppendChild(CARRIERNAME);
                XmlNode FROMLOCATION = doc.CreateElement("LOCATION");
                FROMLOCATION.InnerXml = sourceLocation;
                Body.AppendChild(FROMLOCATION);
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string CreateCycleTransferJob(string transactionID, string machineID, string carrierID,
                                        string sourceLocation, string destinationLocation, string username, string times)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "StartCycleTransfer";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = machineID;
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = transactionID;
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                XmlNode MACHINENAME = doc.CreateElement("MACHINENAME");
                MACHINENAME.InnerXml = machineID;
                Body.AppendChild(MACHINENAME);
                XmlNode USERNAME = doc.CreateElement("USERNAME");
                USERNAME.InnerXml = username;
                Body.AppendChild(USERNAME);
                XmlNode CARRIERNAME = doc.CreateElement("CARRIERNAME");
                CARRIERNAME.InnerXml = carrierID;
                Body.AppendChild(CARRIERNAME);
                XmlNode FROMLOCATION = doc.CreateElement("FROMLOCATION");
                FROMLOCATION.InnerXml = sourceLocation;
                Body.AppendChild(FROMLOCATION);
                XmlNode TOLOCATION = doc.CreateElement("TOLOCATION");
                TOLOCATION.InnerXml = destinationLocation;
                Body.AppendChild(TOLOCATION);
                XmlNode TIMES = doc.CreateElement("TIMES");
                TIMES.InnerXml = times;
                Body.AppendChild(TIMES);
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string CancelCycleTransferJob(string transactionID, string machineID, string carrierID,
                                        string sourceLocation, string destinationLocation, string username)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "CancelCycleTransfer";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = machineID;
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = transactionID;
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                XmlNode MACHINENAME = doc.CreateElement("MACHINENAME");
                MACHINENAME.InnerXml = machineID;
                Body.AppendChild(MACHINENAME);
                XmlNode USERNAME = doc.CreateElement("USERNAME");
                USERNAME.InnerXml = username;
                Body.AppendChild(USERNAME);
                XmlNode CARRIERNAME = doc.CreateElement("CARRIERNAME");
                CARRIERNAME.InnerXml = carrierID;
                Body.AppendChild(CARRIERNAME);
                XmlNode FROMLOCATION = doc.CreateElement("FROMLOCATION");
                FROMLOCATION.InnerXml = sourceLocation;
                Body.AppendChild(FROMLOCATION);
                XmlNode TOLOCATION = doc.CreateElement("TOLOCATION");
                TOLOCATION.InnerXml = destinationLocation;
                Body.AppendChild(TOLOCATION);
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string QueryStockerAllInfo(string stockname, string transactionID)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "QueryStockAllInfo";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = stockname;
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = transactionID;
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                XmlNode STOCKNAME = doc.CreateElement("STOCKNAME");
                STOCKNAME.InnerXml = stockname;
                Body.AppendChild(STOCKNAME);
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string RefreshStockAllInfo(string stockname, string transactionID)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "RefreshStockAllInfo";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = stockname;
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = transactionID;
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                XmlNode STOCKNAME = doc.CreateElement("STOCKNAME");
                STOCKNAME.InnerXml = stockname;
                Body.AppendChild(STOCKNAME);
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string ClearStockAllTransferJob(string stockname, string transactionID)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "ClearStockAllTransferJob";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = stockname;
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = transactionID;
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                XmlNode STOCKNAME = doc.CreateElement("STOCKNAME");
                STOCKNAME.InnerXml = stockname;
                Body.AppendChild(STOCKNAME);
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string RecordUserID(string UserID)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateElement("Message");
                XmlNode Header = doc.CreateElement("Header");
                XmlNode MESSAGENAME = doc.CreateElement("MESSAGENAME");
                MESSAGENAME.InnerXml = "RecordUserID";
                Header.AppendChild(MESSAGENAME);
                XmlNode SHOPNAME = doc.CreateElement("SHOPNAME");
                SHOPNAME.InnerXml = "";
                Header.AppendChild(SHOPNAME);
                XmlNode H_MACHINENAME = doc.CreateElement("MACHINENAME");
                H_MACHINENAME.InnerXml = "";
                Header.AppendChild(H_MACHINENAME);
                XmlNode TRANSACTIONID = doc.CreateElement("TRANSACTIONID");
                TRANSACTIONID.InnerXml = "";
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
                //--- 
                root.AppendChild(Header);
                XmlNode Body = doc.CreateElement("Body");
                XmlNode USERID = doc.CreateElement("USERID");
                USERID.InnerXml = UserID;
                Body.AppendChild(USERID);
                root.AppendChild(Body);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        public string EncodeBase64(string message)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = message;
            }
            return encode;
        }
        public string DecodeBase64(string message)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(message);
            try
            {
                decode = Encoding.GetEncoding("utf-8").GetString(bytes);
            }
            catch
            {
                decode = message;
            }
            return decode;
        }
        public string createFormattedXML(string unformattedXML)
        {
            try
            {
                System.Xml.XmlDocument xmldocument = new System.Xml.XmlDocument();
                xmldocument.LoadXml(unformattedXML);
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                using (System.Xml.XmlTextWriter xmltextwriter = new System.Xml.XmlTextWriter(stringwriter))
                {
                    xmltextwriter.Indentation = 2;
                    xmltextwriter.Formatting = System.Xml.Formatting.Indented;
                    xmldocument.WriteContentTo(xmltextwriter);
                    xmltextwriter.Close();
                }
                return stringwriter.ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}