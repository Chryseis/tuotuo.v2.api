using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.BLL
{
    public  partial class T_BACKLOG_BLL
    {
        public bool SetBackLogsSprint(int teamID, int sprintID, List<int> backLogIDs)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    string updateSql ="update t_backlog set R_SPRINT_ID=@springID where T_TEAM_ID = @teamID and ID in (@IDs)";
                    this.DbContext.DbConnecttion.Execute(updateSql, new
                    {
                        teamID= teamID,
                        springID = sprintID,
                        IDs= backLogIDs
                    }, tran, null, null);           
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
