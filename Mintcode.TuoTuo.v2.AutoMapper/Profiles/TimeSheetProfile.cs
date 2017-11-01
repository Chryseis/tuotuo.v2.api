using AutoMapper;
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
    public class TimeSheetProfile : Profile
    {
        public TimeSheetProfile()
        {
            CreateMap<T_TIME_SHEET, TimeSheetInfoModel>()
             .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
             .ForMember(dest => dest.userID, opt => opt.MapFrom(src => src.TS_USER_ID))
             .ForMember(dest => dest.userMail, opt => opt.MapFrom(src => src.TS_USER_MAIL))
             .ForMember(dest => dest.userName, opt => opt.MapFrom(src => src.TS_USER_NAME))
             .ForMember(dest => dest.timeSheetDate, opt => opt.MapFrom(src => src.TS_DATE))
             .ForMember(dest => dest.timeSheetTimeStamp, opt => opt.MapFrom(src => src.TS_TIMESTAMP))
             .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.TS_STATUS))
             .ForMember(dest => dest.submitTime, opt => opt.MapFrom(src => src.TS_SUBMIT_TIME))
             .ForMember(dest => dest.approvalUserID, opt => opt.MapFrom(src => src.TS_APPROVAL_USER_ID))
             .ForMember(dest => dest.approvalUserMail, opt => opt.MapFrom(src => src.TS_APPROVAL_USER_MAIL))
             .ForMember(dest => dest.approvalUserName, opt => opt.MapFrom(src => src.TS_APPROVAL_USER_NAME))
             .ForMember(dest => dest.approvalTime, opt => opt.MapFrom(src => src.TS_APPROVAL_TIME))
             .ForMember(dest => dest.approvalResult, opt => opt.MapFrom(src => src.TS_APPROVAL_RESULT))
             .ForMember(dest => dest.approvalComment, opt => opt.MapFrom(src => src.TS_APPROVAL_COMMENT))
             .ForMember(dest => dest.createUserMail, opt => opt.MapFrom(src => src.CREATE_USER_MAIL))
             .ForMember(dest => dest.createTime, opt => opt.MapFrom(src => src.CREATE_TIME))
             .ForMember(dest => dest.createTimestamp, opt => opt.MapFrom(src => src.CREATE_TIME.ToTimeStamp()));

            CreateMap<TimeSheetInfoModel, T_TIME_SHEET>()
            .ForMember(dest => dest.ID, opt => opt.Condition(src => src.ID > 0))
            .ForMember(dest => dest.TS_USER_ID, opt => opt.MapFrom(src => src.userID))
            .ForMember(dest => dest.TS_USER_MAIL, opt => opt.MapFrom(src => src.userMail))
            .ForMember(dest => dest.TS_USER_NAME, opt => opt.MapFrom(src => src.userName))
            .ForMember(dest => dest.TS_DATE, opt => opt.MapFrom(src => src.timeSheetDate))
            .ForMember(dest => dest.TS_TIMESTAMP, opt => opt.MapFrom(src => src.timeSheetTimeStamp))
            .ForMember(dest => dest.TS_STATUS, opt => opt.MapFrom(src => src.status))
            .ForMember(dest => dest.TS_SUBMIT_TIME, opt => opt.MapFrom(src => src.submitTime))
            .ForMember(dest => dest.TS_APPROVAL_USER_ID, opt => opt.MapFrom(src => src.approvalUserID))
            .ForMember(dest => dest.TS_APPROVAL_USER_MAIL, opt => opt.MapFrom(src => src.approvalUserMail))
            .ForMember(dest => dest.TS_APPROVAL_USER_NAME, opt => opt.MapFrom(src => src.approvalUserName))
            .ForMember(dest => dest.TS_APPROVAL_TIME, opt => opt.MapFrom(src => src.approvalTime))
            .ForMember(dest => dest.TS_APPROVAL_RESULT, opt => opt.MapFrom(src => src.approvalResult))
            .ForMember(dest => dest.TS_APPROVAL_COMMENT, opt => opt.MapFrom(src => src.approvalComment))
            .ForMember(dest => dest.CREATE_USER_MAIL, opt => opt.MapFrom(src => src.createUserMail))
            .ForMember(dest => dest.CREATE_TIME, opt => opt.MapFrom(src => src.createTime));



            CreateMap<T_TIME_SHEET_TASK, TimeSheetTaskModel>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.sheetID, opt => opt.MapFrom(src => src.TS_ID))
                .ForMember(dest => dest.projectID, opt => opt.MapFrom(src => src.P_ID))
                .ForMember(dest => dest.projectName, opt => opt.MapFrom(src => src.P_NAME))
                .ForMember(dest => dest.detail, opt => opt.MapFrom(src => src.TST_DETAIL))
                .ForMember(dest => dest.time, opt => opt.MapFrom(src => src.TST_TIME))
                .ForMember(dest => dest.userID, opt => opt.MapFrom(src => src.TST_USER_ID))
                .ForMember(dest => dest.userMail, opt => opt.MapFrom(src => src.TST_USER_MAIL))
                .ForMember(dest => dest.userName, opt => opt.MapFrom(src => src.TST_USER_NAME))
                .ForMember(dest => dest.createUserMail, opt => opt.MapFrom(src => src.CREATE_USER_MAIL))
                .ForMember(dest => dest.createTime, opt => opt.MapFrom(src => src.CREATE_TIME))
                .ForMember(dest => dest.createTimestamp, opt => opt.MapFrom(src => src.CREATE_TIME.ToTimeStamp()));




            CreateMap<TimeSheetTaskModel, T_TIME_SHEET_TASK>()
                .ForMember(dest => dest.ID, opt => opt.Condition(src => src.ID > 0))
                .ForMember(dest => dest.TS_ID, opt => opt.MapFrom(src => src.sheetID))
                .ForMember(dest => dest.P_ID, opt => opt.MapFrom(src => src.projectID))
                .ForMember(dest => dest.P_NAME, opt => opt.MapFrom(src => src.projectName))
                .ForMember(dest => dest.TST_DETAIL, opt => opt.MapFrom(src => src.detail))
                .ForMember(dest => dest.TST_TIME, opt => opt.MapFrom(src => src.time))
                .ForMember(dest => dest.TST_USER_ID, opt => opt.MapFrom(src => src.userID))
                .ForMember(dest => dest.TST_USER_MAIL, opt => opt.MapFrom(src => src.userMail))
                .ForMember(dest => dest.TST_USER_NAME, opt => opt.MapFrom(src => src.userName))
                .ForMember(dest => dest.CREATE_USER_MAIL, opt => opt.MapFrom(src => src.createUserMail))
                .ForMember(dest => dest.CREATE_TIME, opt => opt.MapFrom(src => src.createTime));

        }
    }
}
