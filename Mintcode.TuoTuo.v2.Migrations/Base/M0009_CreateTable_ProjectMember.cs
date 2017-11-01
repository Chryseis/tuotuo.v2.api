using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Base
{
    [Migration(201705091640)]
    public class M0009_CreateTable_ProjectMember : Migration
    {
        public override void Up()
        {
            Create.Table("t_project_member")
                .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity().WithColumnDescription("主键")
                .WithColumn("P_PROJECT_ID").AsInt32().NotNullable().WithColumnDescription("项目ID")
                .WithColumn("U_USER_ID").AsInt32().NotNullable().WithColumnDescription("项目成员ID")
                .WithColumn("U_USER_ROLE_CODE").AsString(50).NotNullable().WithColumnDescription("成员角色")
                .WithColumn("P_TAGS").AsString(255).Nullable().WithColumnDescription("标签")
                .WithColumn("T_STATE").AsInt32().NotNullable().WithColumnDescription("0 已邀请 1 已同意")
                .WithColumn("U_USER_NAME").AsString(50).Nullable().WithColumnDescription("项目成员名字")
                .WithColumn("U_USER_EMAIL").AsString(50).NotNullable().WithColumnDescription("项目成员邮箱")
                .WithColumn("CREATE_USER_MAIL").AsString(100).NotNullable().WithColumnDescription("创建人")
                .WithColumn("CREATE_TIME").AsDateTime().NotNullable().WithColumnDescription("创建时间");
        }

        public override void Down()
        {
            Delete.Table("t_project_member");
        }
    }
}
