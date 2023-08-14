using FluentMigrator;
using FluentMigrator.Postgres;

namespace Dal.Migrations;

[Migration(20230807000000)]
public class Migration_20230807000000 : Migration
{
    public override void Down()
    {
        Delete.Sequence("user_id_seq");
        Delete.Table("users");
    }

    public override void Up()
    {
        Create.Sequence("user_id_seq");
        
        Create.Table("users")
            .WithColumn("Id").AsInt32().PrimaryKey().Unique()
            .WithColumn("FIO").AsString(250).NotNullable()
            .WithColumn("Email").AsString(150).Unique().NotNullable()
            .WithColumn("Phone").AsString(11).Unique().NotNullable()
            .WithColumn("Password").AsString(20).NotNullable()
            .WithColumn("LastLogin").AsDateTime().Nullable();
    }
}