﻿using FluentMigrator;

namespace RecipesDB.Migrations
{
    [Migration(5)]
    public class _0005_IngredientTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table(Tables.Ingredient)
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("recipe_id").AsInt32().NotNullable().ForeignKey(Tables.Recipe, "id")
                .WithColumn("name").AsString(1000).NotNullable()
                .WithColumn("is_active").AsBoolean().NotNullable().WithDefaultValue(true);
        }
    }   
}
