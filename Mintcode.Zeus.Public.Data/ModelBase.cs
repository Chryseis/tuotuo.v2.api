using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.Zeus.Public.Data
{
    /// <summary>
    /// 所有的实体类需要继承的对象
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// 需要修改的字段
        /// </summary>
        public IList<string> Fields = new List<string>();

    }
}
