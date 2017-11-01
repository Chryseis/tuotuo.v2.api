using System;
using System.Xml.Serialization;

namespace Mintcode.TuoTuo.v2.DFS.Config
{
    [Serializable]
    public class FastDfsServer
    {
        [XmlAttribute]
        public string IpAddress { get; set; }

        [XmlAttribute]
        public int Port { get; set; }
    }
}