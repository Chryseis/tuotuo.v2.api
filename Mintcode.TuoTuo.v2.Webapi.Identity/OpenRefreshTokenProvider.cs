using Microsoft.Owin.Security.Infrastructure;
using Mintcode.TuoTuo.v2.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi.Identity
{
    public class OpenRefreshTokenProvider : AuthenticationTokenProvider
    {
        public IRefreshTokenRepository refreshTokenRepository;
        public OpenRefreshTokenProvider(IRefreshTokenRepository _refreshTokenRepository)
        {
            refreshTokenRepository = _refreshTokenRepository;
        }
        public override async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            string tokenValue = Guid.NewGuid().ToString("n");
            TimeSpan? expireTimeSpan = null;
            if (context.Ticket.Properties.IssuedUtc.HasValue && context.Ticket.Properties.ExpiresUtc.HasValue)
            {
                expireTimeSpan = context.Ticket.Properties.ExpiresUtc.Value - context.Ticket.Properties.IssuedUtc.Value;
                expireTimeSpan = expireTimeSpan.Value.Add(expireTimeSpan.Value); 
            }
            string newAccessTicket = context.SerializeTicket();

            //将refresh Token和newAccessTicket存储到redis中
            if (await refreshTokenRepository.InsertRefreshTokenAndAccessToken(tokenValue, newAccessTicket, expireTimeSpan))
            {
                context.SetToken(tokenValue);
            }
        }

        public override async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            //从redis中获取新的Token
            string value = await refreshTokenRepository.GetAccessTokenByRefreshToken(context.Token);
            if (!string.IsNullOrEmpty(value))
            {
                context.DeserializeTicket(value);
            }
        }
    }
}
