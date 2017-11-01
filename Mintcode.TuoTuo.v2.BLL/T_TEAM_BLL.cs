using Mintcode.TuoTuo.v2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
using Mintcode.TuoTuo.v2.Common;

namespace Mintcode.TuoTuo.v2.BLL
{
    public partial class T_TEAM_BLL
    {
        public int CreateTeam(T_TEAM team, int userID, string userMail, string userName)
        {
            int id = 0;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    DbContext.Insert(team);
                    var teamMember = new T_TEAM_MEMBER();
                    teamMember.T_TEAM_ID = team.ID;
                    teamMember.U_USER_ID = userID;
                    teamMember.U_USER_NAME = userName;
                    teamMember.U_USER_EMAIL = userMail;
                    teamMember.R_USER_ROLE_CODE = RoleCode.Owner;
                    teamMember.CREATE_USER_MAIL = userMail;
                    teamMember.CREATE_TIME = DateTime.Now;
                    teamMember.T_STATE = 1;
                    DbContext.Insert(teamMember);
                    id = team.ID;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (id>0)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                return id;
            }
        }


        public bool DeleteTeam(int teamID)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    var teamQuery = new DapperExQuery<T_TEAM>().AndWhere(s => s.ID, OperationMethod.Equal, teamID);
                    DbContext.Delete<T_TEAM>(teamQuery.GetSqlQuery(DbContext));
                    var teamMembersQuery = new DapperExQuery<T_TEAM_MEMBER>().AndWhere(s => s.T_TEAM_ID, OperationMethod.Equal, teamID);
                    DbContext.Delete<T_TEAM_MEMBER>(teamMembersQuery.GetSqlQuery(DbContext));

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
