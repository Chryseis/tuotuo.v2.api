using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public interface IEmailService
    {
        Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true);
       
    }
}
