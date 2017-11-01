using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;


namespace Mintcode.TuoTuo.v2.Migrations.Patches
{
    /// <summary>
    /// 修改user_team_member
    /// </summary>
     [Migration(201705161621)]
    public class M0011_AlterTable_TeamMember_AddFiled_UserNameAndUserMail : Migration
    {
         public override void Up()
         {
             Alter.Table("t_team_member")
                 .AddColumn("U_USER_NAME")
                 .AsString(50);
             Alter.Table("t_team_member")
                 .AddColumn("U_USER_EMAIL")
                 .AsString(100);
             Delete.Column("U_USER_ROLE_CODE").FromTable("t_team_member");
             Alter.Table("t_team_member").AddColumn("R_USER_ROLE_CODE");
             Alter.Table("t_team_member").AddColumn("T_STATE");
         }

         public override void Down()
         {
               Delete.Column("U_USER_NAME").FromTable("t_team_member");

               Delete.Column("U_USER_EMAIL").FromTable("t_team_member");

               Delete.Column("R_USER_ROLE_CODE").FromTable("t_team_member");
               Alter.Table("t_team_member").AddColumn("U_USER_ROLE_CODE");
               Delete.Column("T_STATE").FromTable("t_team_member");
         }
    }
}
