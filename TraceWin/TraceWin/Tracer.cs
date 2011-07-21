using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;

namespace TraceWinResources
{
    class Tracer
    {
        public Config config = null;
        public Tracer(Config config) {
            this.config = config;
        }

        public void SendThruUDP(string log, IPAddress serverAddr)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint endPoint = new IPEndPoint(serverAddr, Int32.Parse(config.GetUdpPort()));

            log = "; "+FileHelper.GenResultFileName() + "; " + log;

            byte[] send_buffer = Encoding.ASCII.GetBytes(log);

            sock.SendTo(send_buffer, endPoint);
            Console.ReadLine();
        }

        public void DoTrace(List<string> traceableList, List<string> excList){

            Process[] process = Process.GetProcesses();

            string pattern = "empty";
            string currentProc = "empty";
            string procDetails = "empty";

            for (int i = 0; i < process.Length; i++)
            {
                try
                {
                    currentProc = (process[i].MainModule.FileName).ToLower();
                    procDetails = "Threads: "+process[i].Threads.Count + " : " + process[i].MainModule.ModuleName + " : " + process[i].GetType();

                    //FileHelper.WriteLog("INFO: Scanned PROC = " + FileHelper.ExtractFileName(currentProc));



                    if (traceableList.Contains( FileHelper.ExtractFileName(currentProc) ))
                    { 
                        
                        foreach (ProcessModule module in process[i].Modules)
                        {
                            pattern = process[i].MainModule.FileName + "; " + module.FileName;
                            if (excList == null || !excList.Contains(pattern))
                            {
                                //FileHelper.WriteLog("INFO = TraceWinService started.");
                                FileHelper.WriteLine2File(pattern, config.GetResultPath());

                                IPAddress serverAddr = null;
                                if (config.GetUdpServer().Length > 6)
                                {
                                    try
                                    {
                                        serverAddr = IPAddress.Parse(config.GetUdpServer());
                                        SendThruUDP(pattern, serverAddr);
                                    }
                                    catch (Exception e)
                                    {
                                        //FileHelper.WriteLog("ERROR = Problem sending data through UDP to " + config.GetUdpServer()+ ": " + e.Message);
                                    }
                                }

                            }
                            pattern = "empty";
                        }
                    }
                }
                catch (Exception e)
                {
                    FileHelper.WriteLog("ERROR = " + e.Message + " | Pattern: " + pattern + " | CurrentProc: " + currentProc + " | ProcDetails: " + procDetails);
                    if (currentProc.Length > 1 && (excList == null || !excList.Contains(currentProc + "; "))) FileHelper.WriteLine2File(currentProc + "; ", config.GetResultPath());
                    procDetails = "empty";
                }
            }
        
        }

    }
}
