using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Collections;

namespace MCSUI
{
    public class TimerClass
    {
        private static Timer aTimer;
        public delegate void delegateSetting(string message);
        public static event delegateSetting delegateEvent;
        public static void initializeTimer(int timeout, int timespace)
        {
            TimerClass.aTimer = new Timer(timeout*timespace);
            TimerClass.aTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            TimerClass.aTimer.Enabled = true;
            TimerClass.aTimer.Start();
        }
        public static void DestroyTimer()
        {
            TimerClass.aTimer.Close();
        }
        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CommonFunction comm = new CommonFunction();
            //delegateEvent(comm.ShowAlarmMessage(string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now), "", "ALL", 
            //                                    comm.EncodeBase64("EAP does not reply any initialize message, please check EAP alive or not")));
            string logpath = comm.ReadIni("CONFIG.INI", "LOGPATH", "LOGPATH");
            comm.LogRecordFun("EXCEPTION", "EAP does not reply any initialize message, please check EAP alive or not", logpath);
            DestroyTimer();
        }
    }
}
