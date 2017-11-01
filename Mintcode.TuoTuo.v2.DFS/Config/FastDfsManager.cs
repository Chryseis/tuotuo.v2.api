using System.Configuration;

namespace Mintcode.TuoTuo.v2.DFS.Config
{
    public sealed class FastDfsManager
    {
        public static FastDfsConfig GetConfigSection(string sectionName = "fastdfs")
        {
            return ConfigurationManager.GetSection(sectionName) as FastDfsConfig;
        }
    }
}
