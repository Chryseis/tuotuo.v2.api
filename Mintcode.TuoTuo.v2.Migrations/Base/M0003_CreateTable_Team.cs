using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Base
{
    [Migration(201703301719)]
    public class M0003_CreateTable_Team : Migration
    {
        public override void Up()
        {
            Create.Table("t_team")
             .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity()
             .WithColumn("T_TEAM_CODE").AsString(50).NotNullable().WithColumnDescription("团队code")
             .WithColumn("T_TEAM_NAME").AsString(50).NotNullable().WithColumnDescription("团队名称")
             .WithColumn("T_STATUS").AsInt32().NotNullable().WithColumnDescription("团队状态(0=未启用,1=启用)")
             .WithColumn("CREATE_USER_MAIL").AsString(100).NotNullable().WithColumnDescription("创建人姓名")
             .WithColumn("CREATE_TIME").AsDateTime().NotNullable().WithColumnDescription("创建时间");
        }

        public override void Down()
        {
            Delete.Table("t_team");
        }
    }
}
