using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using TraceWinResources;
using System.Threading;

namespace TraceWinService
{
    public partial class Service1 : ServiceBase
    {
        Thread jobThread = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            
            XmlUtils xmlUtil = new XmlUtils();
            Config config = null;
            try
            {
                config = xmlUtil.readConfig(null);
            
            } catch(Exception e) {
                FileHelper.WriteLog("ERROR = " + e.Message);
            }

            if (config != null)
            {
                try
                {
                    Coordinator worker = new Coordinator(config);
                    jobThread = new Thread(new ThreadStart(worker.TraceJob));
                    jobThread.Start();
                    FileHelper.WriteLog("INFO = TraceWinService started.");
                } catch(Exception e) {
                    FileHelper.WriteLog("ERROR = " + e.Message);
                }
            }
        }

        protected override void OnStop()
        {
            jobThread.Abort();
            FileHelper.WriteLog("INFO = TraceWinService aborted.");
            
        }
    }
}
