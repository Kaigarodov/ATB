using FluentMigrator;
using FluentMigrator.Postgres;

namespace Dal.Migrations;

[Migration(20230807000000)]
public class Migration_20230807000000 : Migration
{
    public override void Down()
    {
        Delete.Table("users");
    }

    public override void Up()
    {
        Create.Table("users")
            .WithColumn("Id").AsInt32().PrimaryKey()
            .WithColumn("Email").AsString(150).Unique().NotNullable()
            .WithColumn("Phone").AsString(11).Unique().NotNullable()
            .WithColumn("Password").AsString(20).NotNullable()
            .WithColumn("LastLogin").AsDateTime().Nullable();
    }
}