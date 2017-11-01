using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Base
{
    [Migration(201705240933)]
    public class M0014_CreateTable_Release : Migration
    {
        public override void Up()
        {
            Create.Table("t_release")
                .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("T_TEAM_ID").AsInt32().NotNullable().WithColumnDescription("团队ID")          
                .WithColumn("R_NAME").AsString(50).NotNullable().WithColumnDescription("周期名称")          
                .WithColumn("R_SUMMARY").AsString(255).Nullable().WithColumnDescription("周期简介")
                .WithColumn("CREATE_USER_MAIL").AsString(100).NotNullable().WithColumnDescription("创建人")
                .WithColumn("CREATE_TIME").AsDateTime().NotNullable().WithColumnDescription("创建时间");

        }

        public override void Down()
        {
            Delete.Table("t_release");
        }
    }
}
