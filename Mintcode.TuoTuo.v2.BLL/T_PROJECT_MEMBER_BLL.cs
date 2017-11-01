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
    public partial class T_PROJECT_MEMBER_BLL
    {
        public bool TransformProject(int projectID, string oldOwnerMail, string newOwnerMail)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    var oldProjectOwnerQuery = new DapperExQuery<T_PROJECT_MEMBER>()
                        .AndWhere(s => s.P_PROJECT_ID, OperationMethod.Equal, projectID)
                        .AndWhere(s => s.U_USER_EMAIL, OperationMethod.Equal, oldOwnerMail)
                        .AndWhere(s => s.R_ROLE_CODE, OperationMethod.Equal, RoleCode.Owner);
                    var oldProjectOwner = DbContext.SingleOrDefault<T_PROJECT_MEMBER>(oldProjectOwnerQuery.GetSqlQuery(DbContext));

                    var newProjectOwnerQuery = new DapperExQuery<T_PROJECT_MEMBER>()
                        .AndWhere(s => s.P_PROJECT_ID, OperationMethod.Equal, projectID)
                        .AndWhere(s => s.U_USER_EMAIL, OperationMethod.Equal, newOwnerMail);
                    var newProjectOwner = DbContext.SingleOrDefault<T_PROJECT_MEMBER>(newProjectOwnerQuery.GetSqlQuery(DbContext));
                    if (oldProjectOwner != null && newProjectOwner != null)
                    {
                        oldProjectOwner.R_ROLE_CODE = RoleCode.Member;
                        newProjectOwner.R_ROLE_CODE = RoleCode.Owner;
                        DbContext.Update(oldProjectOwner);
                        DbContext.Update(newProjectOwner);
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

        public bool InviteMembers(int projectID, List<T_USER> userList, List<T_PROJECT_MEMBER> memberList)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    DbContext.InsertBatch(userList);
                    var insertedUserList=DbContext.QueryData<T_USER>(
                        new DapperExQuery<T_USER>().AndWhere(s => s.U_EMAIL, OperationMethod.In, userList
                        .Select(s => s.U_EMAIL).ToArray())
                        .GetSqlQuery(DbContext));
                    foreach (var member in memberList)
                    {
                        if (insertedUserList.Exists(s=>s.U_EMAIL.Equals(member.U_USER_EMAIL)))
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

        public bool UpdateProjectMemberStatus(string mail, int status)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {

                    var memberList = DbContext.QueryData<T_PROJECT_MEMBER>(
                        new DapperExQuery<T_PROJECT_MEMBER>().AndWhere(s => s.U_USER_EMAIL, OperationMethod.Equal, mail
                       )
                        
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
