﻿using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;

namespace TraceWinResources
{
    public class Coordinator
    {
        public Config config = null;

        public Coordinator(Config config) {
            this.config = config;
        }
        
        public void TraceJob()
        {
            
            List<string> execs = new List<string>();
            List<string> dirs = new List<string>();

            foreach(string e in config.GetInspectPath())
            {
                dirs.AddRange(FileHelper.GetFilesRecursive(@e));
            }
            
            
            foreach (string p in dirs)
            {
                if (p.Contains(".exe"))
                {
                    string e = FileHelper.ExtractFileName(p);

                    execs.Add(e);
                    //FileHelper.WriteLog("INFO: Scanned FS = " + e);
                }
                
            }

            Tracer tracer = new Tracer(config);
            while (true)
            {
                tracer.DoTrace(execs, FileHelper.GetLogged(config.GetResultPath()));
                Thread.Sleep(config.GetInterval() * 1000);
            }
        }
    }
}
