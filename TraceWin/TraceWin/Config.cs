using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraceWinResources
{
    class Config
    {
        private string inspectPath = null;
        private string resultPath = null;
        private int interval = -1;
        private bool verbose = false;

        public void SetInspectPath(string inspectPath) { this.inspectPath = inspectPath;  }
        public void SetResultPath(string resultPath) { this.resultPath = resultPath; }
        public void SetInterval(int interval) { this.interval = interval; }
        public void SetVerbose(bool verbose) { this.verbose = verbose; }

        public string GetInspectPath() { return inspectPath;  }
        public string GetResultPath() { return resultPath; }
        public int GetInterval() { return interval; }
        public bool GetVerbose() { return verbose; }
    }
}
