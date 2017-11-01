using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Base
{
    [Migration(201705240939)]
    public class M0015_CreateTable_ReleaseSprint : Migration
    {
        public override void Up()
        {
            Create.Table("t_release_sprint")
                .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("R_RELEASE_ID").AsInt32().NotNullable().WithColumnDescription("迭代ID")
                .WithColumn("R_START_TIME").AsDateTime().NotNullable().WithColumnDescription("开始时间")
                .WithColumn("R_END_TIME").AsDateTime().NotNullable().WithColumnDescription("结束时间")
                .WithColumn("R_STATE").AsInt32().NotNullable().WithColumnDescription("0 默认状态 1 当前正在使用")
                .WithColumn("CREATE_USER_MAIL").AsString(100).NotNullable().WithColumnDescription("创建人")
                .WithColumn("CREATE_TIME").AsDateTime().NotNullable().WithColumnDescription("创建时间");

        }

        public override void Down()
        {
            Delete.Table("t_release_sprint");
        }
    }
}
