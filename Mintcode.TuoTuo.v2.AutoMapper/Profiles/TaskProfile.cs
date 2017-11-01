using AutoMapper;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.IRepository.Models;
using Mintcode.TuoTuo.v2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.AutoMapper.Profiles
{
    public class TaskProfile: Profile


    {

        public TaskProfile()
        {
            CreateMap<T_TASK, TaskInfoModel>()
                .ForMember(dist => dist.taskID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dist => dist.backLogID, opt => opt.MapFrom(src => src.T_BACKLOG_ID))
                .ForMember(dist => dist.projectID, opt => opt.MapFrom(src => src.P_PROJECT_ID))
                .ForMember(dist => dist.projectName, opt => opt.MapFrom(src => src.P_PROJECT_NAME))
                .ForMember(dist => dist.content, opt => opt.MapFrom(src => src.T_CONENT))
                .ForMember(dist => dist.title, opt => opt.MapFrom(src => src.T_TITLE))
                .ForMember(dist => dist.assignedName, opt => opt.MapFrom(src => src.T_ASSIGNED_NAME))
                .ForMember(dist => dist.assignedEmail, opt => opt.MapFrom(src => src.T_ASSIGNED_EMAIL))
                .ForMember(dist => dist.typeName, opt => opt.MapFrom(src => src.T_TYPE_NAME))
                .ForMember(dist => dist.time, opt => opt.MapFrom(src => src.T_TIME))
                .ForMember(dist => dist.state, opt => opt.MapFrom(src => src.T_STATE))
                .ForMember(dist => dist.currentUserMail, opt => opt.MapFrom(src => src.CREATE_USER_MAIL))
                .ForMember(dist => dist.createTime, opt => opt.MapFrom(src => src.CREATE_TIME))
                .ForMember(dist => dist.createTimestamp, opt => opt.MapFrom(src => src.CREATE_TIME.ToTimeStamp()));

            CreateMap<TaskInfoModel, T_TASK>()
               .ForMember(dist => dist.ID, opt => opt.MapFrom(src => src.taskID))
               .ForMember(dist => dist.T_BACKLOG_ID, opt => opt.MapFrom(src => src.backLogID))
               .ForMember(dist => dist.P_PROJECT_ID, opt => opt.MapFrom(src => src.projectID))
               .ForMember(dist => dist.P_PROJECT_NAME, opt => opt.MapFrom(src => src.projectName))
               .ForMember(dist => dist.T_CONENT, opt => opt.MapFrom(src => src.content))
               .ForMember(dist => dist.T_TITLE, opt => opt.MapFrom(src => src.title))
               .ForMember(dist => dist.T_ASSIGNED_NAME, opt => opt.MapFrom(src => src.assignedName))
               .ForMember(dist => dist.T_ASSIGNED_EMAIL, opt => opt.MapFrom(src => src.assignedEmail))
               .ForMember(dist => dist.T_TYPE_NAME, opt => opt.MapFrom(src => src.typeName))
               .ForMember(dist => dist.T_TIME, opt => opt.MapFrom(src => src.time))
               .ForMember(dist => dist.T_STATE, opt => opt.MapFrom(src => src.state))
               .ForMember(dist => dist.CREATE_USER_MAIL, opt => opt.MapFrom(src => src.currentUserMail))
               .ForMember(dist => dist.CREATE_TIME, opt => opt.MapFrom(src => src.createTime));

            CreateMap<T_TASK_LOG, TaskLogModel>()
               .ForMember(dist => dist.taskLogID, opt => opt.MapFrom(src => src.ID))
               .ForMember(dist => dist.taskID, opt => opt.MapFrom(src => src.T_TASK_ID))
               .ForMember(dist => dist.content, opt => opt.MapFrom(src => src.T_CONENT))
               .ForMember(dist => dist.title, opt => opt.MapFrom(src => src.T_TITLE))
               .ForMember(dist => dist.assignedName, opt => opt.MapFrom(src => src.T_ASSIGNED_NAME))
               .ForMember(dist => dist.assignedEmail, opt => opt.MapFrom(src => src.T_ASSIGNED_EMAIL))
               .ForMember(dist => dist.typeName, opt => opt.MapFrom(src => src.T_TYPE_NAME))
               .ForMember(dist => dist.time, opt => opt.MapFrom(src => src.T_TIME))
               .ForMember(dist => dist.state, opt => opt.MapFrom(src => src.T_STATE))
               .ForMember(dist => dist.createUserName, opt => opt.MapFrom(src => src.CREATE_USER_NAME))
               .ForMember(dist => dist.currentUserMail, opt => opt.MapFrom(src => src.CREATE_USER_MAIL))
               .ForMember(dist => dist.createTime, opt => opt.MapFrom(src => src.CREATE_TIME))
               .ForMember(dist => dist.createTimestamp, opt => opt.MapFrom(src => src.CREATE_TIME.ToTimeStamp()));

            CreateMap<TaskLogModel, T_TASK_LOG>()
             .ForMember(dist => dist.ID, opt => opt.MapFrom(src => src.taskLogID))
             .ForMember(dist => dist.T_TASK_ID, opt => opt.MapFrom(src => src.taskID))
             .ForMember(dist => dist.T_CONENT, opt => opt.MapFrom(src => src.content))
             .ForMember(dist => dist.T_TITLE, opt => opt.MapFrom(src => src.title))
             .ForMember(dist => dist.T_ASSIGNED_NAME, opt => opt.MapFrom(src => src.assignedName))
             .ForMember(dist => dist.T_ASSIGNED_EMAIL, opt => opt.MapFrom(src => src.assignedEmail))
             .ForMember(dist => dist.T_TYPE_NAME, opt => opt.MapFrom(src => src.typeName))
             .ForMember(dist => dist.T_TIME, opt => opt.MapFrom(src => src.time))
             .ForMember(dist => dist.T_STATE, opt => opt.MapFrom(src => src.state))
             .ForMember(dist => dist.CREATE_USER_NAME, opt => opt.MapFrom(src => src.createUserName))
             .ForMember(dist => dist.CREATE_USER_MAIL, opt => opt.MapFrom(src => src.currentUserMail))
             .ForMember(dist => dist.CREATE_TIME, opt => opt.MapFrom(src => src.createTime));
        }
    }
}
