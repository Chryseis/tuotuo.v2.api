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
    public partial class T_TASK_BLL
    {
        public int CreateTask(T_TASK task, String userName)
        {
            int taskId = 0;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    DbContext.Insert(task);
                    var taskLog = new T_TASK_LOG();
                    taskLog.T_TASK_ID = task.ID;
                    taskLog.T_STATE = task.T_STATE;
                    taskLog.T_TITLE = task.T_TITLE;
                    taskLog.T_CONENT = task.T_CONENT;
                    taskLog.T_ASSIGNED_NAME = task.T_ASSIGNED_NAME;
                    taskLog.T_ASSIGNED_EMAIL = task.T_ASSIGNED_EMAIL;
                    taskLog.T_TYPE_NAME = task.T_TYPE_NAME;
                    taskLog.T_TIME = task.T_TIME;
                    taskLog.CREATE_USER_NAME = userName;
                    taskLog.CREATE_USER_MAIL = task.CREATE_USER_MAIL;
                    taskLog.CREATE_TIME = task.CREATE_TIME;
                    DbContext.Insert(taskLog);
                    taskId = task.ID;
                }
                catch (Exception e)
                {
                    
                    throw e;
                }
                finally
                {
                    if (taskId>0)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                return taskId;
            }
        }

        public bool ModifyTask(T_TASK task, String userName,String createUserMail,DateTime createTime)
        {
            bool isSucess = true;
            using (var tran = DbContext.DbTransaction)
            {
                try
                {
                    DbContext.Update(task);
                    var taskLog = new T_TASK_LOG();
                    taskLog.T_TASK_ID = task.ID;
                    taskLog.T_STATE = task.T_STATE;
                    taskLog.T_TITLE = task.T_TITLE;
                    taskLog.T_CONENT = task.T_CONENT;
                    taskLog.T_ASSIGNED_NAME = task.T_ASSIGNED_NAME;
                    taskLog.T_ASSIGNED_EMAIL = task.T_ASSIGNED_EMAIL;
                    taskLog.T_TYPE_NAME = task.T_TYPE_NAME;
                    taskLog.T_TIME = task.T_TIME;
                    taskLog.CREATE_USER_NAME = userName;
                    taskLog.CREATE_USER_MAIL = createUserMail;
                    taskLog.CREATE_TIME = createTime;
                    DbContext.Insert(taskLog);
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
