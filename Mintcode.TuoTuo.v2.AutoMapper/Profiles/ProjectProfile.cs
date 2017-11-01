using AutoMapper;
using Mintcode.TuoTuo.v2.Common;
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
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<T_PROJECT_MEMBER, ProjectMemberModel>()
               .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
               .ForMember(dest => dest.projectID, opt => opt.MapFrom(src => src.P_PROJECT_ID))
               .ForMember(dest => dest.userID, opt => opt.MapFrom(src => src.U_USER_ID))
               .ForMember(dest => dest.roleCode, opt => opt.MapFrom(src => src.R_ROLE_CODE))
               .ForMember(dest => dest.tags, opt => opt.MapFrom(src => this.generateTagsArrayFromString(src.P_TAGS)))
               .ForMember(dest => dest.state, opt => opt.MapFrom(src => src.T_STATE))
               .ForMember(dest => dest.memberName, opt => opt.MapFrom(src => src.U_USER_NAME))
               .ForMember(dest => dest.memberMail, opt => opt.MapFrom(src => src.U_USER_EMAIL))
               .ForMember(dest => dest.createUserMail, opt => opt.MapFrom(src => src.CREATE_USER_MAIL))
               .ForMember(dest => dest.createTime, opt => opt.MapFrom(src => src.CREATE_TIME))
               .ForMember(dest => dest.createTimestamp,opt=>opt.MapFrom(src=>src.CREATE_TIME.ToTimeStamp()));

            CreateMap<ProjectMemberModel, T_PROJECT_MEMBER>()
                .ForMember(dist => dist.ID, opt => opt.Condition(src => src.ID > 0))
                .ForMember(dist => dist.P_PROJECT_ID, opt => opt.MapFrom(src => src.projectID))
                .ForMember(dist => dist.U_USER_ID, opt => opt.MapFrom(src => src.userID))
                .ForMember(dist => dist.R_ROLE_CODE, opt => opt.MapFrom(src => src.roleCode))
                .ForMember(dist => dist.P_TAGS, opt => opt.MapFrom(src => this.generateTagsStringFromArray(src.tags)))
                .ForMember(dist => dist.T_STATE, opt => opt.MapFrom(src => src.state))
                .ForMember(dist => dist.U_USER_NAME, opt => opt.MapFrom(src => src.memberName))
                .ForMember(dist => dist.U_USER_EMAIL, opt => opt.MapFrom(src => src.memberMail))
                .ForMember(dist => dist.CREATE_USER_MAIL, opt => opt.MapFrom(src => src.createUserMail))
                .ForMember(dist => dist.CREATE_TIME, opt => opt.MapFrom(src => src.createTime));


            CreateMap<T_PROJECT, ProjectInfoModel>()
                .ForMember(dist=>dist.projectID,opt=>opt.MapFrom(src=>src.ID))
                .ForMember(dist => dist.projectName,opt=>opt.MapFrom(src=>src.P_PROJECT_NAME))
                .ForMember(dist => dist.projectSummary, opt => opt.MapFrom(src => src.P_PROJECT_SUMMARY))
                .ForMember(dist => dist.projectAvatar, opt => opt.MapFrom(src => src.P_PROJECT_AVATAR))
                .ForMember(dist => dist.createUserMail, opt => opt.MapFrom(src => src.CREATE_USER_MAIL))
                .ForMember(dist => dist.createTime, opt => opt.MapFrom(src => src.CREATE_TIME))
                .ForMember(dest => dest.createTimestamp, opt => opt.MapFrom(src => src.CREATE_TIME.ToTimeStamp()));

            CreateMap<ProjectInfoModel, T_PROJECT>()
               .ForMember(dist => dist.ID, opt => opt.Condition(src => src.projectID>0))
               .ForMember(dist => dist.ID, opt => opt.MapFrom(src => src.projectID))
               .ForMember(dist => dist.P_PROJECT_NAME, opt => opt.MapFrom(src => src.projectName))
               .ForMember(dist => dist.P_PROJECT_SUMMARY, opt => opt.MapFrom(src => src.projectSummary))
               .ForMember(dist => dist.P_PROJECT_AVATAR, opt => opt.MapFrom(src => src.projectAvatar))
               .ForMember(dist => dist.CREATE_USER_MAIL, opt => opt.MapFrom(src => src.createUserMail))
               .ForMember(dist => dist.CREATE_TIME, opt => opt.MapFrom(src => src.createTime));


            CreateMap<T_PROJECT_MEMBER, Role>()
                 .ForMember(dist => dist.roleType, opt => opt.MapFrom(src=> RoleType.Project))
                 .ForMember(dist => dist.relationID, opt => opt.MapFrom(src => src.P_PROJECT_ID))
                 .ForMember(dist => dist.roleCode, opt => opt.MapFrom(src => src.R_ROLE_CODE));

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
