using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Base
{
    [Migration(201705240942)]
    public class M0016_CreateTable_Backlog : Migration
    {
        public override void Up()
        {
            Create.Table("t_backlog")
                .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("T_TEAM_ID").AsInt32().NotNullable().WithColumnDescription("团队ID")
                .WithColumn("P_PROJECT_ID").AsInt32().NotNullable().WithColumnDescription("没有填0")
                .WithColumn("P_PROJECT_NAME").AsString(50).Nullable().WithColumnDescription("项目名称")
                .WithColumn("R_SPRINT_ID").AsInt32().NotNullable().WithColumnDescription("没有填0")
                .WithColumn("B_TITLE").AsString(50).NotNullable().WithColumnDescription("标题")
                .WithColumn("B_CONTENT").AsString(2000).Nullable().WithColumnDescription("内容")
                .WithColumn("B_STANDARD").AsString(2000).Nullable().WithColumnDescription("评审标准")
                .WithColumn("B_STATE").AsInt32().NotNullable().WithColumnDescription("1 new 2 inprogerss 3 done 4 remove 5 fail")
                .WithColumn("B_LEVEL").AsInt32().Nullable().WithColumnDescription("优先级")
                .WithColumn("B_ASSIGNED_NAME").AsString(50).Nullable().WithColumnDescription("负责人")
                .WithColumn("B_ASSIGNED_EMAIL").AsString(100).Nullable().WithColumnDescription("负责人邮箱")
                .WithColumn("CREATE_USER_MAIL").AsString(100).NotNullable().WithColumnDescription("创建人")
                .WithColumn("CREATE_TIME").AsDateTime().NotNullable().WithColumnDescription("创建时间");

        }

        public override void Down()
        {
            Delete.Table("t_backlog");
        }
    }
}
