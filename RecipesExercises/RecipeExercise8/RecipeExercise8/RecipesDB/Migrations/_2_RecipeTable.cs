using FluentMigrator;

namespace RecipesDB.Migrations
{
    [Migration(2)]
    public class _2_RecipeTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table(Tables.Recipe)
                .WithColumn("id").AsInt32().PrimaryKey().Identity().Indexed()
                .WithColumn("title").AsString(100).NotNullable().Unique()                
                .WithColumn("is_active").AsBoolean().NotNullable().WithDefaultValue(true);
        }
    }
}
