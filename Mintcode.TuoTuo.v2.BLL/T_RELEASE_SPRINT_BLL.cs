using Dapper;
using Mintcode.TuoTuo.v2.Model;
using Mintcode.Zeus.Public.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.BLL
{
    public partial class T_RELEASE_SPRINT_BLL
    {
        public bool SetCurrentSprint(int teamID,int sprintID)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    string updateAllSql= string.Format("update t_release_sprint set R_STATE={0} where R_RELEASE_ID in (select ID from t_release where T_TEAM_ID={1})", 0, teamID);
                    this.DbContext.DbConnecttion.Execute(updateAllSql, null, tran, null, null);
                    string updateOneSql = string.Format("update t_release_sprint set R_STATE={0} where  ID = {1}", 1, sprintID);
                    this.DbContext.DbConnecttion.Execute(updateOneSql, null, tran, null, null);
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


        public bool DeleteSprint(DapperExQuery<T_RELEASE_SPRINT> deleteQuery,T_RELEASE_SPRINT modifySprint)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    bool deleteResult=DbContext.Delete<T_RELEASE_SPRINT>(deleteQuery.GetSqlQuery(DbContext));
                    if (deleteResult && modifySprint != null)
                    {
                        this.Update(modifySprint);
                    }
                   
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
