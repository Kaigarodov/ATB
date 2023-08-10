using FluentMigrator;
using FluentMigrator.Postgres;

namespace Dal.Migrations;

[Migration(20230808000000)]
public class Migration_20230808000000 : Migration
{
    public override void Down()
    {
        throw new NotImplementedException();
    }

    public override void Up()
    {
        Create.Sequence("user_id_seq");
        Alter.Table("users").AlterColumn("LastLogin").AsDateTime().Nullable();
    }
}