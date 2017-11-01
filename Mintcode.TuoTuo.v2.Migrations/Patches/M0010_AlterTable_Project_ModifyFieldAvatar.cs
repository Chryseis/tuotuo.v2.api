using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Migrations.Patches
{
    [Migration(201705161620)]
    public class M0010_AlterTable_Project_ModifyFieldAvatar : Migration
    {
        public override void Up()
        {
            Alter.Table("t_project")
                .AlterColumn("P_PROJECT_AVATAR")
                .AsString(100);

        }

        public override void Down()
        {
            Alter.Table("t_project")
                .AlterColumn("P_PROJECT_AVATAR")
                .AsString(50);
        }
    }
}
