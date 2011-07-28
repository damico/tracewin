using System;
using System.Collections.Generic;

using System.Text;
using TraceWinResources;
using System.Threading;

namespace TCW
{
    class Program
    {
        static void Main(string[] args)
        {
            String emsg = "Invalid xml config file";
            XmlUtils xmlUtil = new XmlUtils();
            Config config = null;
            try
            {
                config = xmlUtil.readConfig(args[0]);
            }
            catch (Exception e1)
            {
                FileHelper.WriteLog("ERROR = " + emsg + "[" + e1.Message + "]");
                Console.WriteLine("ERROR = " + emsg + "[" + e1.Message + "]");
            }

            if (config != null)
            {
                try
                {
                    Coordinator worker = new Coordinator(config);
                    Thread jobThread = new Thread(new ThreadStart(worker.TraceJob));
                    jobThread.Start();
                }
                catch (Exception e2)
                {
                    FileHelper.WriteLog("ERROR = " + emsg + "[" + e2.Message + "]");
                    Console.WriteLine("ERROR = " + emsg + "[" + e2.Message + "]");
                }
            }
            else 
            {
                FileHelper.WriteLog("ERROR = " + emsg);
                Console.WriteLine("ERROR = " + emsg);
            }
        }
    }
}
