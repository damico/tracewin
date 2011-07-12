using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TraceWinResources
{
    class XmlUtils
    {
        public Config readConfig(string xmlPath)
        {
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
                            config.SetInspectPath(reader.GetAttribute(0));
                            config.SetResultPath(reader.GetAttribute(1));
                            //TODO: add exception handling
                            config.SetInterval(Int32.Parse(reader.GetAttribute(2)));
                            //TODO: add exception handling
                            config.SetVerbose(Boolean.Parse(reader.GetAttribute(3)));
                            
                        }
                        break;

                        
                }
            }
            return config;
        }
    }
}
