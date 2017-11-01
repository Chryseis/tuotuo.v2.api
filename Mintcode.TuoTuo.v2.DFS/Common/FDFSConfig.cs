using System.Text;

namespace Mintcode.TuoTuo.v2.DFS.Common
{
    public class FDFSConfig
    {
        public static int StorageMaxConnection = 20;
        public static int TrackerMaxConnection = 10;
        public static int ConnectionTimeout = 5; //Second
        public static int ConnectionLifeTime = 3600;
        public static Encoding Charset = Encoding.UTF8;
    }
}