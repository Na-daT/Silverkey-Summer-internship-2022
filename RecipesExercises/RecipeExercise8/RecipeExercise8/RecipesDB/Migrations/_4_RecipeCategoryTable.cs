using FluentMigrator;

namespace RecipesDB.Migrations
{
    [Migration(4)]
    public class _4_RecipeCategoryTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table(Tables.RecipeCategory)
                .WithColumn("id").AsInt32().PrimaryKey().Identity().Indexed()
                .WithColumn("recipe_id").AsInt32().NotNullable().ForeignKey(Tables.Recipe, "id")
                .WithColumn("category_id").AsInt32().NotNullable().ForeignKey(Tables.Category, "id")
                .WithColumn("is_active").AsBoolean().NotNullable().WithDefaultValue(true);

        }
    }
}
