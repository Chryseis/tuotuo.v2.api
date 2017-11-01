using AutoMapper;
using Mintcode.TuoTuo.v2.Infrastructure;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.AutoMapper
{
    public class BackLogProfile: Profile
    {
        public BackLogProfile()
        {
            CreateMap<T_BACKLOG, BacklogInfoModel>()
                  .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                  .ForMember(dest => dest.teamID, opt => opt.MapFrom(src => src.T_TEAM_ID))
                  .ForMember(dest => dest.projectID, opt => opt.MapFrom(src => src.P_PROJECT_ID))
                  .ForMember(dest => dest.projectName, opt => opt.MapFrom(src => src.P_PROJECT_NAME))
                  .ForMember(dest => dest.sprintID, opt => opt.MapFrom(src =>src.R_SPRINT_ID))
                  .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.B_TITLE))
                  .ForMember(dest => dest.content, opt => opt.MapFrom(src => src.B_CONTENT))
                  .ForMember(dest => dest.standard, opt => opt.MapFrom(src => src.B_STANDARD))
                  .ForMember(dest => dest.state, opt => opt.MapFrom(src => src.B_STATE))
                  .ForMember(dest => dest.level, opt => opt.MapFrom(src => src.B_LEVEL))
                  .ForMember(dest => dest.assignUserName, opt => opt.MapFrom(src => src.B_ASSIGNED_NAME))
                  .ForMember(dest => dest.assignUserMail, opt => opt.MapFrom(src => src.B_ASSIGNED_EMAIL))
                  .ForMember(dest => dest.createUserMail, opt => opt.MapFrom(src => src.CREATE_USER_MAIL))
                  .ForMember(dest => dest.createTime, opt => opt.MapFrom(src => src.CREATE_TIME))
                  .ForMember(dest => dest.createTimestamp, opt => opt.MapFrom(src => src.CREATE_TIME.ToTimeStamp())); 


            CreateMap<BacklogInfoModel, T_BACKLOG>()
                  .ForMember(dest => dest.ID, opt => opt.Condition(src => src.ID>0))
                  .ForMember(dest => dest.T_TEAM_ID, opt => opt.MapFrom(src => src.teamID))
                  .ForMember(dest => dest.P_PROJECT_ID, opt => opt.MapFrom(src => src.projectID))
                  .ForMember(dest => dest.P_PROJECT_NAME, opt => opt.MapFrom(src => src.projectName))
                  .ForMember(dest => dest.R_SPRINT_ID, opt => opt.MapFrom(src => src.sprintID))
                  .ForMember(dest => dest.B_TITLE, opt => opt.MapFrom(src => src.title))
                  .ForMember(dest => dest.B_CONTENT, opt => opt.MapFrom(src => src.content))
                  .ForMember(dest => dest.B_STANDARD, opt => opt.MapFrom(src => src.standard))
                  .ForMember(dest => dest.B_STATE, opt => opt.MapFrom(src => src.state))
                  .ForMember(dest => dest.B_LEVEL, opt => opt.MapFrom(src => src.level))
                  .ForMember(dest => dest.B_ASSIGNED_NAME, opt => opt.MapFrom(src => src.assignUserName))
                  .ForMember(dest => dest.B_ASSIGNED_EMAIL, opt => opt.MapFrom(src => src.assignUserMail))
                  .ForMember(dest => dest.CREATE_USER_MAIL, opt => opt.MapFrom(src => src.createUserMail))
                  .ForMember(dest => dest.CREATE_TIME, opt => opt.MapFrom(src => src.createTime));
        }
    }
}
