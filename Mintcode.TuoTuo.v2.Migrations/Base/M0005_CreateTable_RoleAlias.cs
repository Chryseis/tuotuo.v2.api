using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Base
{
    [Migration(201703301728)]
    public class M0005_CreateTable_RoleAlias : Migration
    {
        public override void Up()
        {
            Create.Table("t_role_alias")
             .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity().WithColumnDescription("ID")
             .WithColumn("RA_TEAM_ID").AsInt32().NotNullable().WithColumnDescription("团队ID")
             .WithColumn("RA_ROLE_CODE").AsString(50).NotNullable().WithColumnDescription("角色CODE")
             .WithColumn("RA_ROLE_ALIAS_NAME").AsString(50).NotNullable().WithColumnDescription("角色别名")
             .WithColumn("CREATE_USER_MAIL").AsString(100).NotNullable().WithColumnDescription("创建人姓名")
             .WithColumn("CREATE_TIME").AsDateTime().NotNullable().WithColumnDescription("创建时间");
        }

        public override void Down()
        {
            Delete.Table("t_role_alias");
        }
    }
}
