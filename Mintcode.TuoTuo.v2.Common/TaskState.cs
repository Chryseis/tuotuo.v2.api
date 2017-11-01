using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Common
{
    public class TaskState
    {

        public const int NEW = 1;


        public const int INPROGRESS = 2;


        public const int DONE = 3;


        public const int REMOVE = 4;

        public static bool CheckState(int state)
        {
            bool result = false;
            switch (state)
            {
                case TaskState.NEW:
                case TaskState.INPROGRESS:
                case TaskState.DONE:
                case TaskState.REMOVE:
                    result = true;
                    break;
                default: result = false; break;
            }
            return result;
        }
    }
}
