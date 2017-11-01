using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public interface IRelationAccountRepository
    {
        Task<bool> InsertRelationAccountModel(string token, RelationAccountModel relationAccountModel, TimeSpan? expiry);


        Task<RelationAccountModel> GetRelationAccountModel(string token);
    }
}
