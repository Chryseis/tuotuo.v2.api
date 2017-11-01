using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Common
{
    public class BacklogState
    {

        public const int NEW = 1;


        public const int INPROGRESS = 2;

 
        public const int DONE = 3;

        public const int REMOVE = 4;

        public const int FAIL = 5;


        public static bool CheckState(int state)
        {
            bool result = false;
            switch (state)
            {
                case BacklogState.NEW:
                case BacklogState.INPROGRESS:
                case BacklogState.DONE:
                case BacklogState.REMOVE:
                case BacklogState.FAIL:
                    result = true;
                    break;
                default: result = false; break;
            }
            return result;
        }
    }
}
