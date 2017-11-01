using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Mintcode.TuoTuo.v2.DFS.Config
{
    [Serializable]
    public class FastDfsConfig
    {
        public FastDfsConfig()
        {
            FastDfsServer = new List<FastDfsServer>();
        }

        [XmlAttribute]
        public string GroupName { get; set; }

        [XmlElement]
        public List<FastDfsServer> FastDfsServer { get; set; }
    }
}
