using FluentMigrator;

namespace RecipesDB.Migrations
{
    [Migration(6)]
    public class _6_InstructionTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table(Tables.Instruction)
                .WithColumn("id").AsInt64().PrimaryKey().Identity().Indexed()
                .WithColumn("recipe_id").AsInt64().NotNullable().ForeignKey(Tables.Recipe, "id")
                .WithColumn("name").AsString(1000).NotNullable();
        }
    }
}
