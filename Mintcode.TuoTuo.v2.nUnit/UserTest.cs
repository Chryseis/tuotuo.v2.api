using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Mintcode.TuoTuo.v2.nUnit
{
    [TestFixture]
    public class UserTest
    {
        [TestCase(3,1)]
        [TestCase(2,2)]
        public void SumTest(int x,int y)
        {
            Assert.AreEqual(4, Add(x,y));
        }

        private int Add(int x, int y)
        {
            return x + y;
        }
    }
}
