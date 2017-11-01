using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Base
{
    [Migration(201703301730)]
    public class M0006_CreateTable_ThirdParty : Migration
    {
        public override void Up()
        {
            Create.Table("t_third_party")
             .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity().WithColumnDescription("ID")
             .WithColumn("U_USER_ID").AsInt32().NotNullable().WithColumnDescription("用户ID")
             .WithColumn("T_THIRD_PARTY_ID").AsString(100).NotNullable().WithColumnDescription("第三方ID")
             .WithColumn("T_FROM").AsString(50).NotNullable().WithColumnDescription("来源")
             .WithColumn("T_STATUS").AsInt32().NotNullable().WithColumnDescription("绑定状态 0=未绑定 1=绑定")
             .WithColumn("CREATE_TIME").AsDateTime().NotNullable().WithColumnDescription("创建时间");
        }

        public override void Down()
        {
            Delete.Table("t_third_party");
        }
    }
}
