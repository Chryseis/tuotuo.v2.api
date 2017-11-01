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
    public partial class T_PROJECT_BLL
    {
        public bool CreateProject(T_PROJECT project,int userID,string userName)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    DbContext.Insert(project);                 
                    var projectMember = new T_PROJECT_MEMBER();
                    projectMember.P_PROJECT_ID = project.ID;
                    projectMember.U_USER_ID = userID;
                    projectMember.T_STATE = 1;
                    projectMember.U_USER_NAME = userName;
                    projectMember.U_USER_EMAIL = project.CREATE_USER_MAIL;
                    projectMember.R_ROLE_CODE = RoleCode.Owner;
                    projectMember.CREATE_USER_MAIL = project.CREATE_USER_MAIL;
                    projectMember.CREATE_TIME = DateTime.Now;
                    DbContext.Insert(projectMember);
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


        public bool DeleteProject(int projectID)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    var projectQuery = new DapperExQuery<T_PROJECT>().AndWhere(s => s.ID, OperationMethod.Equal, projectID);
                    DbContext.Delete<T_PROJECT>(projectQuery.GetSqlQuery(DbContext));
                    var projectMembersQuery = new DapperExQuery<T_PROJECT_MEMBER>().AndWhere(s => s.P_PROJECT_ID, OperationMethod.Equal, projectID);
                    DbContext.Delete<T_PROJECT_MEMBER>(projectMembersQuery.GetSqlQuery(DbContext));

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
