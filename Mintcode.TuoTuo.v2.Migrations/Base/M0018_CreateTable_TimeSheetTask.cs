using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Base
{
    [Migration(201706131614)]
    public class M0018_CreateTable_TimeSheetTask : Migration
    {
        public override void Up()
        {
            Create.Table("t_time_sheet_task")
              .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity().WithColumnDescription("ID")
              .WithColumn("TS_ID").AsInt32().NotNullable().WithColumnDescription("Sheet ID")
              .WithColumn("P_ID").AsInt32().NotNullable().WithColumnDescription("项目ID")
              .WithColumn("P_NAME").AsString(50).NotNullable().WithColumnDescription("项目名称")
              .WithColumn("TST_DETAIL").AsString().Nullable().WithColumnDescription("工作详情")
              .WithColumn("TST_TIME").AsDecimal().NotNullable().WithColumnDescription("工作时长")
              .WithColumn("TST_USER_ID").AsInt32().NotNullable().WithColumnDescription("用户ID")
              .WithColumn("TST_USER_MAIL").AsString(50).NotNullable().WithColumnDescription("用户邮箱")
              .WithColumn("TST_USER_NAME").AsString(50).NotNullable().WithColumnDescription("用户姓名")
              .WithColumn("CREATE_USER_MAIL").AsString(100).NotNullable().WithColumnDescription("创建人邮箱")
              .WithColumn("CREATE_TIME").AsDateTime().NotNullable().WithColumnDescription("创建时间");


        }

        public override void Down()
        {
            Delete.Table("t_time_sheet_task");
        }
    }
}
