using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Base
{
    [Migration(201706131612)]
    public class M0017_CreateTable_TimeSheet : Migration
    {
        public override void Up()
        {

            Create.Table("t_time_sheet")
               .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity().WithColumnDescription("ID")
               .WithColumn("TS_USER_ID").AsInt32().NotNullable().WithColumnDescription("用户ID")
               .WithColumn("TS_USER_MAIL").AsString(50).NotNullable().WithColumnDescription("用户邮箱")
               .WithColumn("TS_USER_NAME").AsString(50).NotNullable().WithColumnDescription("用户姓名")
               .WithColumn("TS_DATE").AsDateTime().NotNullable().WithColumnDescription("TimeSheet日期")
               .WithColumn("TS_TIMESTAMP").AsInt64().NotNullable().WithColumnDescription("TimeSheet日期时间戳")
               .WithColumn("TS_STATUS").AsInt32().NotNullable().WithColumnDescription("TimeSheet状态，0=待填写，1=已提交，2=已审核")
               .WithColumn("TS_SUBMIT_TIME").AsDateTime().Nullable().WithColumnDescription("TimeSheet提交时间")
               .WithColumn("TS_APPROVAL_USER_ID").AsInt32().Nullable().WithColumnDescription("审核用户ID")
               .WithColumn("TS_APPROVAL_USER_MAIL").AsString(50).Nullable().WithColumnDescription("审核用户邮箱")
               .WithColumn("TS_APPROVAL_USER_NAME").AsString(50).Nullable().WithColumnDescription("审核用户姓名")
               .WithColumn("TS_APPROVAL_TIME").AsDateTime().Nullable().WithColumnDescription("审核时间")
               .WithColumn("TS_APPROVAL_RESULT").AsInt32().Nullable().WithColumnDescription("审核结果，0=疑问，1=合格，2=优秀")
               .WithColumn("TS_APPROVAL_COMMENT").AsString(200).Nullable().WithColumnDescription("评论")
               .WithColumn("CREATE_USER_MAIL").AsString(100).NotNullable().WithColumnDescription("创建人邮箱")
               .WithColumn("CREATE_TIME").AsDateTime().NotNullable().WithColumnDescription("创建时间");

        }

        public override void Down()
        {
            Delete.Table("t_time_sheet");
        }
    }
}
