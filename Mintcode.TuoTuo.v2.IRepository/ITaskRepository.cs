using Mintcode.TuoTuo.v2.IRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.IRepository
{
    public interface ITaskRepository
    {
        int CreateTask(int backLogID, int teamID, string title, string content, string assignedEmail, string typeName, int time, int state, string currentUserMail);
       

        bool ModifyTask(int taskId, int projectID, int teamID, string title, string content, string assignedEmail, string typeName, int time, int state, string currentUserMail);

        bool ModifyTaskState(int taskID, int teamID, int state, string currentUserMail);

        List<TaskInfoModel> GetTaskList(List<int> backLogIDList);

        TaskRepoModel getTaskInfo(int taskID);

        List<TaskLogModel> getTaskLog(int taskID);

        List<TaskInfoModel> getTaskList(int backLogID);

        List<TaskInfoModel> getMyCompleteTaskList(String currentUserMail, DateTime startTime, DateTime endTime);
    }
}
