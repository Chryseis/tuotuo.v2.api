using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// 根据Refresh Token从Redis中获取AccessToken
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<string> GetAccessTokenByRefreshToken(string refreshToken);

        /// <summary>
        /// 插入Refresh Token和AccessToken到Redis中
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="accessToken"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> InsertRefreshTokenAndAccessToken(string refreshToken,string accessToken, TimeSpan? expiry);
    }
}
