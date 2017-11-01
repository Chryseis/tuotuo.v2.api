using AutoMapper;
using Mintcode.TuoTuo.v2.Common;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.IRepository.Models;
using Mintcode.TuoTuo.v2.Model;
using Mintcode.TuoTuo.v2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.AutoMapper
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<T_TEAM_MEMBER, TeamMemberModel>()
              .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
              .ForMember(dest => dest.teamID, opt => opt.MapFrom(src => src.T_TEAM_ID))
              .ForMember(dest => dest.userID, opt => opt.MapFrom(src => src.U_USER_ID))
              .ForMember(dest => dest.roleCode, opt => opt.MapFrom(src => src.R_USER_ROLE_CODE))
              .ForMember(dest => dest.tags, opt => opt.MapFrom(src => this.generateTagsArrayFromString(src.TAGS)))
              .ForMember(dest => dest.state, opt => opt.MapFrom(src => src.T_STATE))
              .ForMember(dest => dest.memberName, opt => opt.MapFrom(src => src.U_USER_NAME))
              .ForMember(dest => dest.memberMail, opt => opt.MapFrom(src => src.U_USER_EMAIL))
              .ForMember(dest => dest.createUserMail, opt => opt.MapFrom(src => src.CREATE_USER_MAIL))
              .ForMember(dest => dest.createTime, opt => opt.MapFrom(src => src.CREATE_TIME))
              .ForMember(dest => dest.createTimestamp, opt => opt.MapFrom(src => src.CREATE_TIME.ToTimeStamp()));

            CreateMap<TeamMemberModel, T_TEAM_MEMBER>()
                .ForMember(dist => dist.ID, opt => opt.Condition(src => src.ID > 0))
                .ForMember(dist => dist.T_TEAM_ID, opt => opt.MapFrom(src => src.teamID))
                .ForMember(dist => dist.U_USER_ID, opt => opt.MapFrom(src => src.userID))
                .ForMember(dist => dist.R_USER_ROLE_CODE, opt => opt.MapFrom(src => src.roleCode))
                .ForMember(dist => dist.TAGS, opt => opt.MapFrom(src => this.generateTagsStringFromArray(src.tags)))
                .ForMember(dist => dist.T_STATE, opt => opt.MapFrom(src => src.state))
                .ForMember(dist => dist.U_USER_NAME, opt => opt.MapFrom(src => src.memberName))
                .ForMember(dist => dist.U_USER_EMAIL, opt => opt.MapFrom(src => src.memberMail))
                .ForMember(dist => dist.CREATE_USER_MAIL, opt => opt.MapFrom(src => src.createUserMail))
                .ForMember(dist => dist.CREATE_TIME, opt => opt.MapFrom(src => src.createTime));

            CreateMap<T_TEAM, TeamInfoModel>()
              .ForMember(dist => dist.teamID, opt => opt.MapFrom(src => src.ID))
              .ForMember(dist => dist.teamName, opt => opt.MapFrom(src => src.T_TEAM_NAME))
              .ForMember(dist => dist.teamSummary, opt => opt.MapFrom(src => src.T_TEAM_SUMMARY))
              .ForMember(dist => dist.teamAvatar, opt => opt.MapFrom(src => src.T_TEAM_AVATAR))
              .ForMember(dist => dist.createUserMail, opt => opt.MapFrom(src => src.CREATE_USER_MAIL))
              .ForMember(dist => dist.createTime, opt => opt.MapFrom(src => src.CREATE_TIME))
              .ForMember(dest => dest.createTimestamp, opt => opt.MapFrom(src => src.CREATE_TIME.ToTimeStamp()));

            CreateMap<T_TEAM_MEMBER, Role>()
            .ForMember(dist => dist.roleType, opt => opt.MapFrom(src => RoleType.Team))
            .ForMember(dist => dist.relationID, opt => opt.MapFrom(src => src.T_TEAM_ID))
            .ForMember(dist => dist.roleCode, opt => opt.MapFrom(src => src.R_USER_ROLE_CODE));
        }

        private List<string> generateTagsArrayFromString(string tags)
        {
            var ret = new List<string>();
            if (!string.IsNullOrEmpty(tags))
            {
                ret = tags.Split(',').ToList();
            }
            return ret;
        }

        private string generateTagsStringFromArray(List<string> tags)
        {
            var ret = string.Empty;
            if (tags != null && tags.Count > 0)
            {
                ret = string.Join(",", tags.ToArray());
            }
            return ret;
        }
    }
}
