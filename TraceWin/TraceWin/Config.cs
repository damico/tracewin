using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraceWinResources
{
    public class Config
    {
        private List<string> inspectPath = null;
        private string resultPath = null;
        private int interval = -1;
        private bool verbose = false;
        private string udpserver = null;
        private string udpport = null;

        public void SetInspectPath(List<string> inspectPath) { this.inspectPath = inspectPath; }
        public void SetResultPath(string resultPath) { this.resultPath = resultPath; }
        public void SetInterval(int interval) { this.interval = interval; }
        public void SetVerbose(bool verbose) { this.verbose = verbose; }
        public void SetUdpServer(string udpserver) { this.udpserver = udpserver; }
        public void SetUdpPort(string udpport) { this.udpport = udpport; }

        public List<string> GetInspectPath() { return inspectPath; }
        public string GetResultPath() { return resultPath; }
        public int GetInterval() { return interval; }
        public bool GetVerbose() { return verbose; }
        public string GetUdpServer() { return udpserver; }
        public string GetUdpPort() { return udpport; }
    }
}
