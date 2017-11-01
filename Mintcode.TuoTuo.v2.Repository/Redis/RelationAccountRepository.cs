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
    public class RelationAccountRepository: IRelationAccountRepository
    {
        private string prefix = "OAuth:RelationAccount:";

        private IDatabase database;
        public RelationAccountRepository(IDatabase _database)
        {
            database = _database;
        }
        public async Task<bool> InsertRelationAccountModel(string relationtoken, RelationAccountModel relationAccountModel, TimeSpan? expiry)
        {
            Enforce.ArgumentNotNull<string>(relationtoken, "Relation Token 不能为null");
            Enforce.ArgumentNotNull<RelationAccountModel>(relationAccountModel, "RelationAccountModel 不能为null");

            string key = string.Concat(prefix, relationtoken);
            string value = Newtonsoft.Json.JsonConvert.SerializeObject(relationAccountModel);
            bool  result=await database.StringSetAsync(key, value, expiry);
            return result;
        }


        public async Task<RelationAccountModel> GetRelationAccountModel(string relationtoken)
        {
            Enforce.ArgumentNotNull<string>(relationtoken, "Relation Token 不能为null");

            RelationAccountModel relationAccountModel = null;

            string key = string.Concat(prefix, relationtoken);

            string model = await this.database.StringGetAsync(key);

            //await this.database.KeyDeleteAsync(key);

            if (!string.IsNullOrEmpty(model))
            {
                relationAccountModel = Newtonsoft.Json.JsonConvert.DeserializeObject<RelationAccountModel>(model);
            }

            return relationAccountModel;
        }

    }
}
