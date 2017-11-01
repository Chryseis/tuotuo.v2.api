using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Base
{
    [Migration(201703301709)]
    public class M0002_CreateTable_Role : Migration
    {
        public override void Up()
        {
            Create.Table("t_role")
             .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity().WithColumnDescription("角色ID")
             .WithColumn("R_CODE").AsString(20).NotNullable().WithColumnDescription("角色CODE")
             .WithColumn("R_NAME").AsString(50).NotNullable().WithColumnDescription("角色名称")
             .WithColumn("CREATE_USER_MAIL").AsString(100).NotNullable().WithColumnDescription("创建人姓名")
             .WithColumn("CREATE_TIME").AsDateTime().NotNullable().WithColumnDescription("创建时间");

            Insert.IntoTable("t_role").Row(new { R_CODE = "OWNER", R_NAME = "拥有者", CREATE_USER_MAIL = "allenfeng@mintcode.com", CREATE_TIME = "2017-03-30 13:30:35" });
            Insert.IntoTable("t_role").Row(new { R_CODE = "MANAGER", R_NAME = "管理者", CREATE_USER_MAIL = "allenfeng@mintcode.com", CREATE_TIME = "2017-03-30 13:30:57" });
            Insert.IntoTable("t_role").Row(new { R_CODE = "MEMBER", R_NAME = "成员", CREATE_USER_MAIL = "allenfeng@mintcode.com", CREATE_TIME = "2017-03-30 13:31:16" });
        }

        public override void Down()
        {
            Delete.Table("t_role");
        }
    }
}
