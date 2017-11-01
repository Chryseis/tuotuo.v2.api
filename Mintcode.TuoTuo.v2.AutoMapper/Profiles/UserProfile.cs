using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Mintcode.TuoTuo.v2.Webapi;
using Mintcode.TuoTuo.v2.Model;
using Mintcode.TuoTuo.v2.IRepository;
using Mintcode.TuoTuo.v2.Infrastructure;

namespace Mintcode.TuoTuo.v2.AutoMapper
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<T_USER, UserInfoModel>()
                .ForMember(dest => dest.userID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.userName, opt => opt.MapFrom(src => src.U_USER_NAME))
                .ForMember(dest => dest.userTrueName, opt => opt.MapFrom(src => src.U_USER_TRUE_NAME))
                .ForMember(dest => dest.userLevel, opt => opt.MapFrom(src => src.U_LEVEL))
                .ForMember(dest => dest.sex, opt => opt.MapFrom(src => src.U_SEX))
                .ForMember(dest => dest.mail, opt => opt.MapFrom(src => src.U_EMAIL))
                .ForMember(dest => dest.mobile, opt => opt.MapFrom(src => src.U_MOBILE))
                .ForMember(dest => dest.userAvatar, opt => opt.MapFrom(src => src.U_AVATAR))
                .ForMember(dest => dest.userStatus, opt => opt.MapFrom(src => src.U_STATUS))
                .ForMember(dest => dest.lastLoginTime, opt => opt.MapFrom(src => src.U_LAST_LOGIN_TIME))
                .ForMember(dest => dest.lastLoginTimestamp, opt => opt.MapFrom(src => src.U_LAST_LOGIN_TIME.ToTimeStamp()))
                .ForMember(dest => dest.createTime, opt => opt.MapFrom(src => src.CREATE_TIME))
                .ForMember(dest => dest.createTimestamp, opt => opt.MapFrom(src => src.CREATE_TIME.ToTimeStamp()));


            CreateMap<UserInfoModel, T_USER>()
                .ForMember(dest => dest.ID, opt => opt.Condition(src => src.userID>0))
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.userID))
                .ForMember(dest => dest.U_USER_NAME, opt => opt.MapFrom(src => src.userName))
                .ForMember(dest => dest.U_USER_TRUE_NAME, opt => opt.MapFrom(src => src.userTrueName))
                .ForMember(dest => dest.U_LEVEL, opt => opt.MapFrom(src => src.userLevel))
                .ForMember(dest => dest.U_PASSWORD, opt => opt.Condition(src => !string.IsNullOrEmpty(src.password)))
                .ForMember(dest => dest.U_SEX, opt => opt.MapFrom(src => src.sex))
                .ForMember(dest => dest.U_EMAIL, opt => opt.MapFrom(src => src.mail))
                .ForMember(dest => dest.U_MOBILE, opt => opt.MapFrom(src => src.mobile))
                 .ForMember(dest => dest.U_AVATAR, opt => opt.MapFrom(src => src.userAvatar))
                .ForMember(dest => dest.U_STATUS, opt => opt.MapFrom(src => src.userStatus))
                .ForMember(dest => dest.U_LAST_LOGIN_TIME, opt => opt.MapFrom(src => src.lastLoginTime))
                .ForMember(dest => dest.CREATE_TIME, opt => opt.MapFrom(src => src.createTime));
     
            CreateMap<T_THIRD_PARTY, ThirdPartyInfoModel>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.userID, opt => opt.MapFrom(src => src.U_USER_ID))
                .ForMember(dest => dest.thirdPartyID, opt => opt.MapFrom(src => src.T_THIRD_PARTY_ID))
                .ForMember(dest => dest.from, opt => opt.MapFrom(src => src.T_FROM)) 
                .ForMember(dest => dest.createTime, opt => opt.MapFrom(src => src.CREATE_TIME))
                .ForMember(dest => dest.createTimestamp, opt => opt.MapFrom(src => src.CREATE_TIME.ToTimeStamp()));


            CreateMap<ThirdPartyInfoModel, T_THIRD_PARTY>()
                .ForMember(dest => dest.ID, opt => opt.Condition(src => src.ID > 0))
                .ForMember(dest => dest.U_USER_ID, opt => opt.MapFrom(src => src.userID))
                .ForMember(dest => dest.T_THIRD_PARTY_ID, opt => opt.MapFrom(src => src.thirdPartyID))
                .ForMember(dest => dest.T_FROM, opt => opt.MapFrom(src => src.from))
                .ForMember(dest => dest.CREATE_TIME, opt => opt.MapFrom(src => src.createTime));




        }
    }
}
