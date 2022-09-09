using FluentMigrator;
using Microsoft.AspNetCore.Identity;
using RecipesDB.Migrations;


namespace RecipesDB.Seed
{
    public record User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

    }

    [Migration(11)]
    public class _0101_UserTableSeed : Migration
    {
        public static PasswordHasher<User> hasher = new();
        public static List<User> users = new()
        {
            new User
            {
                Username = "nada",
                Password = hasher.HashPassword(new User(), "p@ssword"),
                IsActive = true,
                RefreshToken = null,
                RefreshTokenExpiry = null
            },
            new User
            {
                Username = "test1",
                Password = hasher.HashPassword(new User(), "p@ssword"),
                IsActive = true,
                RefreshToken = null,
                RefreshTokenExpiry = null
            },
            new User
            {
                Username = "test2",
                Password = hasher.HashPassword(new User(), "p@ssword"),
                IsActive = true,
                RefreshToken = null,
                RefreshTokenExpiry = null
            }
        };

        public override void Up()
        {
            foreach (var u in users)
            {
                Insert.IntoTable(Tables.User)
                    .Row(new
                    {
                        username = u.Username,
                        password = u.Password,
                        is_active = u.IsActive,
                        refresh_token = u.RefreshToken,
                        refresh_token_expiry = u.RefreshTokenExpiry
                    });
            }
        }

        public override void Down()
        {
        }
    }
}
