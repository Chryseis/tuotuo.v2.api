using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace Mintcode.TuoTuo.v2.Migrations.Patches
{
    [Migration(201705181122)]
    public class M0012_AlterTable_Team_AddFiled_Summary_Avatar : Migration
    {
        public override void Up()
        {
            Alter.Table("t_team")
                .AddColumn("T_TEAM_SUMMARY")
                .AsString(255);
            Alter.Table("t_team")
                .AddColumn("T_TEAM_AVATAR")
                .AsString(100);
        }

         public override void Down()
         {
             Delete.Column("T_TEAM_SUMMARY").FromTable("t_team");

             Delete.Column("T_TEAM_AVATAR").FromTable("t_team");

         }
    }
}
