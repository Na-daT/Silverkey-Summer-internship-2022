using FluentMigrator;

namespace RecipesDB.Migrations
{
    [Migration(3)]
    public class _0003_CategoryTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table(Tables.Category)
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("name").AsString(100).NotNullable().Unique()
                .WithColumn("is_active").AsBoolean().NotNullable().WithDefaultValue(true);

        }
    }
}
