﻿using FluentMigrator;

namespace RecipesDB.Migrations
{
    [Migration(1)]
    public class _1_UserTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table(Tables.User)
                .WithColumn("id").AsInt32().PrimaryKey().Identity().Indexed()
                .WithColumn("username").AsString(100).NotNullable().Unique()
                .WithColumn("password").AsString(100).NotNullable()
                .WithColumn("is_active").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("refresh_token").AsString().Nullable()
                .WithColumn("refresh_token_expiry").AsDateTime().Nullable();
        }
    }
}