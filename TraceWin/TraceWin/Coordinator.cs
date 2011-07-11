using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TraceWinResources
{
    class Coordinator
    {
        public Config config = null;

        public Coordinator(Config config) {
            this.config = config;
        }
        
        public void TraceJob()
        {
            
            List<string> execs = new List<string>();
            List<string> dirs = FileHelper.GetFilesRecursive(@config.GetInspectPath());
            foreach (string p in dirs)
            {
                if (p.Contains(".exe")) execs.Add(p);
            }

            Tracer tracer = new Tracer(config.GetResultPath());
            while (true)
            {
                tracer.DoTrace(execs, FileHelper.GetLogged(config.GetResultPath()));
                Thread.Sleep(config.GetInterval() * 1000);
            }
        }
    }
}
