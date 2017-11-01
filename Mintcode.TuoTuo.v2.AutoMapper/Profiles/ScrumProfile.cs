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
    public class ScrumProfile : Profile
    {
        public ScrumProfile()
        {
            CreateMap<ReleaseInfoModel, T_RELEASE>()
                  .ForMember(dist => dist.ID, opt => opt.Condition(src => src.ID > 0))
                  .ForMember(dest => dest.T_TEAM_ID, opt => opt.MapFrom(src => src.teamID))
                  .ForMember(dest => dest.R_NAME, opt => opt.MapFrom(src => src.releaseName))
                  .ForMember(dest => dest.R_SUMMARY, opt => opt.MapFrom(src => src.releaseSummary))
                  .ForMember(dest => dest.CREATE_USER_MAIL, opt => opt.MapFrom(src => src.createUserMail))
                  .ForMember(dest => dest.CREATE_TIME, opt => opt.MapFrom(src => src.createTime));


            CreateMap<T_RELEASE, ReleaseInfoModel>()
                 .ForMember(dist => dist.ID, opt => opt.MapFrom(src => src.ID))
                 .ForMember(dest => dest.teamID, opt => opt.MapFrom(src => src.T_TEAM_ID))
                 .ForMember(dest => dest.releaseName, opt => opt.MapFrom(src => src.R_NAME))
                 .ForMember(dest => dest.releaseSummary, opt => opt.MapFrom(src => src.R_SUMMARY))
                 .ForMember(dest => dest.createUserMail, opt => opt.MapFrom(src => src.CREATE_USER_MAIL))
                 .ForMember(dest => dest.createTime, opt => opt.MapFrom(src => src.CREATE_TIME))
                 .ForMember(dest => dest.createTimestamp, opt => opt.MapFrom(src => src.CREATE_TIME.ToTimeStamp()));


            CreateMap<SprintInfoModel, T_RELEASE_SPRINT>()
                  .ForMember(dist => dist.ID, opt => opt.Condition(src => src.ID > 0))
                  .ForMember(dest => dest.R_RELEASE_ID, opt => opt.MapFrom(src => src.releaseID))
                  .ForMember(dest => dest.R_START_TIME, opt => opt.MapFrom(src => src.startTime))
                  .ForMember(dest => dest.R_END_TIME, opt => opt.MapFrom(src => src.endTime))
                   .ForMember(dest => dest.R_NO, opt => opt.MapFrom(src => src.no))
                  .ForMember(dest => dest.R_STATE, opt => opt.MapFrom(src => src.state))
                  .ForMember(dest => dest.CREATE_USER_MAIL, opt => opt.MapFrom(src => src.createUserMail))
                  .ForMember(dest => dest.CREATE_TIME, opt => opt.MapFrom(src => src.createTime));

            CreateMap<T_RELEASE_SPRINT, SprintInfoModel>()
                 .ForMember(dist => dist.ID, opt => opt.MapFrom(src => src.ID))
                 .ForMember(dest => dest.releaseID, opt => opt.MapFrom(src => src.R_RELEASE_ID))
                 .ForMember(dest => dest.startTime, opt => opt.MapFrom(src => src.R_START_TIME))
                 .ForMember(dest => dest.startTimestamp, opt => opt.MapFrom(src => src.R_START_TIME.ToTimeStamp()))
                 .ForMember(dest => dest.endTime, opt => opt.MapFrom(src => src.R_END_TIME))
                 .ForMember(dest => dest.endTimestamp, opt => opt.MapFrom(src => src.R_END_TIME.ToTimeStamp()))
                 .ForMember(dest => dest.no, opt => opt.MapFrom(src => src.R_NO))
                 .ForMember(dest => dest.state, opt => opt.MapFrom(src => src.R_STATE))
                 .ForMember(dest => dest.createUserMail, opt => opt.MapFrom(src => src.CREATE_USER_MAIL))
                 .ForMember(dest => dest.createTime, opt => opt.MapFrom(src => src.CREATE_TIME))
                 .ForMember(dest => dest.createTimestamp, opt => opt.MapFrom(src => src.CREATE_TIME.ToTimeStamp()));
        }
    }
}
