using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Base
{
    [Migration(201703301514)]
    public class M0001_CreateTable_User : Migration
    {
        public override void Up()
        {
            Create.Table("t_user")
                .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity().WithColumnDescription("ID")
                .WithColumn("U_USER_NAME").AsString(50).Nullable().WithColumnDescription("用户名")
                .WithColumn("U_USER_TRUE_NAME").AsString(50).Nullable().WithColumnDescription("用户真实姓名")
                .WithColumn("U_LEVEL").AsInt32().NotNullable().WithColumnDescription("用户级别(0=普通用户 1=待扩展)")
                .WithColumn("U_PASSWORD").AsString(100).Nullable().WithColumnDescription("用户密码")
                .WithColumn("U_SEX").AsInt32().Nullable().WithColumnDescription("用户性别 0=男 1=女")
                .WithColumn("U_EMAIL").AsString(100).NotNullable().WithColumnDescription("用户邮箱")
                .WithColumn("U_STATUS").AsInt32().NotNullable().WithColumnDescription("用户状态")
                .WithColumn("U_LAST_LOGIN_TIME").AsDateTime().NotNullable().WithColumnDescription("最后登陆时间")
                .WithColumn("CREATE_TIME").AsDateTime().NotNullable().WithColumnDescription("创建时间");
        }

        public override void Down()
        {
            Delete.Table("t_user");
        }
    }
}
