using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public interface ITokenSecurity
    {
        string encrypt<T>(T payload);

        T decrypt<T>(string token);
    }
}
