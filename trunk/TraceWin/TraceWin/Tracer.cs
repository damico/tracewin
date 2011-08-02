using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using VmcController.Services;
using System.IO;
using TraceWin;

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

            if (excList == null) excList = new List<string>();

            Process[] process = Process.GetProcesses();

            string pattern = "empty";
            string currentProc = "empty";
            string procDetails = "empty";

            for (int i = 0; i < process.Length; i++)
            {
                try
                {
                    currentProc = (process[i].MainModule.FileName).ToLower();
                    procDetails = "Threads: "+process[i].Threads.Count + " : PID: " + process[i].MainModule.ModuleName;

                    if (traceableList.Contains( FileHelper.ExtractFileName(currentProc) ))
                    {

                        pattern = currentProc + GetExeDllType(currentProc);
                        excList = SendPattern(pattern, excList, config);

                        IEnumerator<FileSystemInfo> enumFsi = DetectOpenFiles.GetOpenFilesEnumerator(process[i].Id);
                        FileSystemInfo element = null;
                        while (enumFsi.MoveNext())
                        {
                            element = enumFsi.Current;
                            pattern = currentProc + "; " + element.FullName;
                            if (element.Extension.Length > 0) excList = SendPattern(pattern, excList, config);
                        }

                        foreach (ProcessModule module in process[i].Modules)
                        {
                            pattern = currentProc + "; " + module.FileName;
                            excList = SendPattern(pattern, excList, config);
                            pattern = "empty";
                        }
                    }
                }
                catch (Exception e)
                {
                    FileHelper.WriteLog("ERROR = " + e.Message + " | Pattern: " + pattern + " | CurrentProc: " + currentProc + " | ProcDetails: " + procDetails);

                    if (currentProc != null && currentProc.Length > 1) 
                    {
                        pattern = currentProc + GetExeDllType(currentProc);
                        excList = SendPattern(pattern, excList, config);
                    }
                    procDetails = "empty";
                }
            }
        
        }

        private List<string> SendPattern(string pattern, List<string> excList, Config config)
        {
            if (!excList.Contains(pattern))
            {
                FileHelper.WriteLine2File(pattern, config.GetResultPath());
                excList.Add(pattern);
            }

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

            return excList;
        }

        public string GetExeDllType(String filePath)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(";;");
            if (DetectBinaryType.IsDotNetExecutable(filePath)) sb.Append(".NET / ");
            if (DetectBinaryType.RunsAs64Bit(filePath)) sb.Append("64bits / ");
            sb.Append(DetectBinaryType.GetExeDllMachineType(filePath));

            return sb.ToString();
        
        }
    }
}
