using FluentMigrator;

namespace RecipesDB.Migrations
{
    [Migration(6)]
    public class _6_InstructionTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table(Tables.Instruction)
                .WithColumn("id").AsInt32().PrimaryKey().Identity().Indexed()
                .WithColumn("recipe_id").AsInt32().NotNullable().ForeignKey(Tables.Recipe, "id")
                .WithColumn("name").AsString(1000).NotNullable()
                .WithColumn("is_active").AsBoolean().NotNullable().WithDefaultValue(true);
        }
    }
}
