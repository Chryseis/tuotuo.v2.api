using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mintcode.Zeus.Public.Data;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Model;

namespace Mintcode.TuoTuo.v2.BLL
{
    public partial class T_TEAM_MEMBER_BLL
    {
         public bool TransformTeam(int teamID,string oldOwnerMail,string newOwnerMail)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    var oldTeamOwnerQuery = new DapperExQuery<T_TEAM_MEMBER>()
                        .AndWhere(s => s.T_TEAM_ID, OperationMethod.Equal, teamID)
                        .AndWhere(s=>s.U_USER_EMAIL,OperationMethod.Equal,oldOwnerMail)
                        .AndWhere(s=>s.R_USER_ROLE_CODE,OperationMethod.Equal,RoleCode.Owner);
                    var oldTeamOwner=DbContext.SingleOrDefault<T_TEAM_MEMBER>(oldTeamOwnerQuery.GetSqlQuery(DbContext));

                    var newTeamOwnerQuery = new DapperExQuery<T_TEAM_MEMBER>()
                        .AndWhere(s => s.T_TEAM_ID, OperationMethod.Equal, teamID)
                        .AndWhere(s => s.U_USER_EMAIL, OperationMethod.Equal, newOwnerMail);
                    var newTeamOwner = DbContext.SingleOrDefault<T_TEAM_MEMBER>(newTeamOwnerQuery.GetSqlQuery(DbContext));
                    if (oldTeamOwner != null && newTeamOwner != null)
                    {
                        oldTeamOwner.R_USER_ROLE_CODE = RoleCode.Member;
                        newTeamOwner.R_USER_ROLE_CODE = RoleCode.Owner;
                        DbContext.Update(oldTeamOwner);
                        DbContext.Update(newTeamOwner);
                        isSucess = true;
                    }
                    else
                    {
                        isSucess = false;
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

         public bool InviteMembers(int teamID, List<T_USER> userList, List<T_TEAM_MEMBER> memberList)
         {
             bool isSucess = true;
             using (var tran = DbContext.DbTransaction)
             {
                 try
                 {
                     DbContext.InsertBatch(userList);
                     var insertedUserList = DbContext.QueryData<T_USER>(
                         new DapperExQuery<T_USER>().AndWhere(s => s.U_EMAIL, OperationMethod.In, userList
                         .Select(s => s.U_EMAIL).ToArray())
                         .GetSqlQuery(DbContext));
                     foreach (var member in memberList)
                     {
                         if (insertedUserList.Exists(s => s.U_EMAIL.Equals(member.U_USER_EMAIL)))
                         {
                             var user = insertedUserList.Single(s => s.U_EMAIL.Equals(member.U_USER_EMAIL));
                             member.U_USER_ID = user.ID;
                         }
                     }
                     DbContext.InsertBatch(memberList);
                     isSucess = true;
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

         public bool UpdateTeamMemberStatus(string mail, int status)
         {
             bool isSucess = true;
             using (var tran = DbContext.DbTransaction)
             {
                 try
                 {

                     var memberList = DbContext.QueryData<T_TEAM_MEMBER>(
                         new DapperExQuery<T_TEAM_MEMBER>().AndWhere(s => s.U_USER_EMAIL, OperationMethod.Equal, mail)
                         .GetSqlQuery(DbContext));
                     foreach (var member in memberList)
                     {
                         member.T_STATE = status;
                         DbContext.Update(member);
                     }

                     isSucess = true;
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
