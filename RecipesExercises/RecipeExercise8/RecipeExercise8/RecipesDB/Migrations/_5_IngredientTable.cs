using FluentMigrator;

namespace RecipesDB.Migrations
{
    [Migration(5)]
    public class _5_IngredientTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table(Tables.Ingredient)
                .WithColumn("id").AsInt64().PrimaryKey().Identity().Indexed()
                .WithColumn("recipe_id").AsInt64().NotNullable().ForeignKey(Tables.Recipe, "id")
                .WithColumn("name").AsString(1000).NotNullable();
        }
    }   
}
