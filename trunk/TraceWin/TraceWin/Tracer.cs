using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TraceWinResources
{
    class Tracer
    {
        public string resultPath = null;
        public Tracer(string resultPath) {
            this.resultPath = resultPath;
        }

        public void DoTrace(List<string> traceableList, List<string> excList){

            Process[] process = Process.GetProcesses();

            for (int i = 0; i < process.Length; i++)
            {
                try
                {
                    
                    if (traceableList.Contains(process[i].MainModule.FileName))
                    {
                        foreach (ProcessModule module in process[i].Modules)
                        {
                            string pattern = process[i].MainModule.FileName + " ; " + module.FileName;
                            if (excList == null || !excList.Contains(pattern))
                            {
                                Console.WriteLine(pattern);
                                FileHelper.WriteLine2File(pattern, resultPath);

                            }
                        }
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
                

            }
        
        }

    }
}
