using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Base
{
    [Migration(201703301723)]
    public class M0004_CreateTable_TeamMember : Migration
    {
        public override void Up()
        {
            Create.Table("t_team_member")
             .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity().WithColumnDescription("ID")
             .WithColumn("T_TEAM_ID").AsInt32().NotNullable().WithColumnDescription("团队ID")
             .WithColumn("U_USER_ID").AsInt32().NotNullable().WithColumnDescription("用户ID")
             .WithColumn("U_USER_ROLE_CODE").AsString(50).NotNullable().WithColumnDescription("成员角色CODE")
             .WithColumn("CREATE_USER_MAIL").AsString(100).NotNullable().WithColumnDescription("创建人姓名")
             .WithColumn("CREATE_TIME").AsDateTime().NotNullable().WithColumnDescription("创建时间");
        }

        public override void Down()
        {
            Delete.Table("t_team_member");
        }
    }
}
