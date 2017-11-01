using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Base
{
    [Migration(201705091637)]
    public class M0008_CreateTable_Project : Migration
    {
        public override void Up()
        {
            Create.Table("t_project")
           .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity().WithColumnDescription("主键")
           .WithColumn("P_PROJECT_NAME").AsString(50).NotNullable().WithColumnDescription("项目名称")
           .WithColumn("P_PROJECT_SUMMARY").AsString(255).Nullable().WithColumnDescription("项目简介")
           .WithColumn("P_PROJECT_AVATAR").AsString(50).Nullable().WithColumnDescription("项目图片")
           .WithColumn("CREATE_USER_MAIL").AsString(100).NotNullable().WithColumnDescription("创建人姓名")
           .WithColumn("CREATE_TIME").AsDateTime().NotNullable().WithColumnDescription("创建时间");
        }

        public override void Down()
        {
            Delete.Table("t_project");
        }
    }
}
