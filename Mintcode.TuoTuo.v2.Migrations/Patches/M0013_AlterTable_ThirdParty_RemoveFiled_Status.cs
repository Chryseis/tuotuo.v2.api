using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Patches
{
    [Migration(201705231454)]
    public class M0013_AlterTable_ThirdParty_RemoveFiled_Status : Migration
    {
        public override void Up()
        {
            Delete.Column("t_third_party").FromTable("T_STATUS");
        }

        public override void Down()
        {
            Alter.Table("t_third_party")
                 .AddColumn("T_STATUS")
                 .AsInt32()
                 .NotNullable()
                 .WithColumnDescription("绑定状态 0=未绑定 1=绑定");

        }

    }
}
