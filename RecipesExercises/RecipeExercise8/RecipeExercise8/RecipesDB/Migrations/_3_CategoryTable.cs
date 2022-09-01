using FluentMigrator;

namespace RecipesDB.Migrations
{
    [Migration(3)]
    public class _3_CategoryTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table(Tables.Category)
                .WithColumn("id").AsInt64().PrimaryKey().Identity().Indexed()
                .WithColumn("name").AsString(100).NotNullable().Unique()
                .WithColumn("is_active").AsBoolean().NotNullable().WithDefaultValue(false);

        }
    }
}
