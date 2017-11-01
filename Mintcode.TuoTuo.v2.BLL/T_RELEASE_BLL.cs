using Mintcode.TuoTuo.v2.Model;
using Mintcode.Zeus.Public.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.BLL
{
    public partial class T_RELEASE_BLL
    {
        public bool DeleteRelease(int releaseID)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    var releaseQuery = new DapperExQuery<T_RELEASE>().AndWhere(s => s.ID, OperationMethod.Equal, releaseID);
                    DbContext.Delete<T_RELEASE>(releaseQuery.GetSqlQuery(DbContext));
                    var releaseSprintsQuery = new DapperExQuery<T_RELEASE_SPRINT>().AndWhere(s => s.R_RELEASE_ID, OperationMethod.Equal, releaseID);
                    DbContext.Delete<T_RELEASE_SPRINT>(releaseSprintsQuery.GetSqlQuery(DbContext));

                }
                catch (Exception e)
                {
                    isSucess = false;
                    throw e;
                }
                finally
                {
                    if (isSucess)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                return isSucess;
            }
        }

    }
}
