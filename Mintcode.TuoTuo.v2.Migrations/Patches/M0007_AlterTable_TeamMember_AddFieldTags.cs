using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Patches
{
    [Migration(201705091630)]
    public class M0007_AlterTable_TeamMember_AddFieldTags : Migration
    {
        public override void Up()
        {
            Alter.Table("t_team_member")
                .AddColumn("TAGS")
                .AsString(255)
                .Nullable()
                .WithColumnDescription("标签");
                
        }

        public override void Down()
        {
            Delete.Column("TAGS").FromTable("t_team_member");
        }
    }
}
