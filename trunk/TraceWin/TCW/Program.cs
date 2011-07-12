using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceWinResources;
using System.Threading;

namespace TCW
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlUtils xmlUtil = new XmlUtils();
            Config config = xmlUtil.readConfig(args[0]);
            Coordinator worker = new Coordinator(config);
            Thread jobThread = new Thread(new ThreadStart(worker.TraceJob));
            jobThread.Start();

        }
    }
}
