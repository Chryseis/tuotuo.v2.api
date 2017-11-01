using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mintcode.TuoTuo.v2.Unit
{
    public class Unit
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Theory(int value)
        {
            Assert.True(IsOdd(value));
        }


        private int Add(int x, int y)
        {
            return x + y;
        }

        private bool IsOdd(int value)
        {
            return value % 2 == 1;
        }
    }
}
