using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TraceWinResources
{
    public class XmlUtils
    {
        private List<string> inspectPath = null;

        public Config readConfig(string xmlPath)
        {

            inspectPath = new List<string>();
            Config config = new Config();

            if (xmlPath == null) xmlPath = Constants.CONFIG_FILE_PATH;

            XmlTextReader reader = new XmlTextReader(xmlPath);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "config")
                        {
                            config.SetResultPath(reader.GetAttribute(0));
                            //TODO: add exception handling
                            config.SetInterval(Int32.Parse(reader.GetAttribute(1)));
                            //TODO: add exception handling
                            config.SetVerbose(Boolean.Parse(reader.GetAttribute(2)));

                        }
                        else if (reader.Name == "inspect")
                        {
                           inspectPath.Add(reader.GetAttribute(0));
                        }
                        break;

                        
                }
            }
            config.SetInspectPath(inspectPath);
            return config;
        }
    }
}
