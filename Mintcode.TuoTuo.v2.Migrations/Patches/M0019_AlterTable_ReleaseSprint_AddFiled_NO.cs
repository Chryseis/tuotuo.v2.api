using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Patches
{
    [Migration(201706151614)]
    public class M0019_AlterTable_ReleaseSprint_AddFiled_NO : Migration
    {

        public override void Up()
        {
            Alter.Table("t_release_sprint")
                .AddColumn("R_NO")
                .AsInt32()
                .Nullable()
                .WithColumnDescription("排序号");

        }

        public override void Down()
        {
            Delete.Column("R_NO").FromTable("t_release_sprint");
        }
    }
}
