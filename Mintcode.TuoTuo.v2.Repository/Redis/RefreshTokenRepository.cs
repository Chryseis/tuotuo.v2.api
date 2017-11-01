using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.IRepository;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Repository
{
    public class RefreshTokenRepository: IRefreshTokenRepository
    {
        private string prefix = "OAuth:RefreshToken:";

        private IDatabase database;
        public RefreshTokenRepository(IDatabase _database)
        {
            database = _database;
        }

        public async Task<string> GetAccessTokenByRefreshToken(string refreshToken)
        {
            Enforce.ArgumentNotNull<string>(refreshToken,"Refresh Token 不能为null");

            string key = string.Concat(prefix, refreshToken);
            string accessToken = await database.StringGetAsync(key);
            database.KeyDeleteAsync(key);
            return accessToken;
        }

        public async Task<bool> InsertRefreshTokenAndAccessToken(string refreshToken, string accessToken, TimeSpan? expiry)
        {
            Enforce.ArgumentNotNull<string>(refreshToken, "Refresh Token 不能为null");
            Enforce.ArgumentNotNull<string>(accessToken, "Access Token 不能为null");

            string key = string.Concat(prefix, refreshToken);

            bool result= await this.database.StringSetAsync(key, accessToken, expiry);

            return result;
        }
    }
}
