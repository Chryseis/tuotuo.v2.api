using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Patches
{
    [Migration(201706191614)]
    public class M0020_AlterTable_User_AddFiled_Avatar : Migration
    {
        public override void Up()
        {
            Alter.Table("t_user")
                .AddColumn("U_AVATAR")
                .AsString(100)
                .Nullable()
                .WithColumnDescription("头像");

        }

        public override void Down()
        {
            Delete.Column("U_AVATAR").FromTable("t_user");
        }
    }
}
