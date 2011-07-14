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

            for (int i = 0; i < process.Length; i++)
            {
                try
                {
                    
                    if (traceableList.Contains(process[i].MainModule.FileName))
                    {
                        foreach (ProcessModule module in process[i].Modules)
                        {
                            string pattern = process[i].MainModule.FileName + "; " + module.FileName;
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
                        }
                    }
                }
                catch (Exception e) {
                    FileHelper.WriteLog("ERROR = " + e.Message);
                }
                

            }
        
        }

    }
}
