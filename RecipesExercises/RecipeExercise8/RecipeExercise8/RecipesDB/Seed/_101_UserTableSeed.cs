using FluentMigrator;
using Microsoft.AspNetCore.Identity;
using RecipesDB.Migrations;


namespace RecipesDB.Seed
{
    public class User
    {
        public int Id { get; set; } 
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

    }

    [Migration(11)]
    public class _101_UserTableSeed : Migration
    {
        public static PasswordHasher<User> hasher = new();
        public static List<User> users = new ()
        {
            new User
            {
                Id = 1,
                Username = "nada",
                Password = hasher.HashPassword(new User(), "p@ssword"),
                IsActive = true,
                RefreshToken = null,
                RefreshTokenExpiry = null
            },
            new User
            {
                Id = 2,
                Username = "test1",
                Password = hasher.HashPassword(new User(), "p@ssword"),
                IsActive = true,
                RefreshToken = null,
                RefreshTokenExpiry = null
            },
            new User
            {
                Id = 3,
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
                        id = u.Id,
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
            //Delete.FromTable(Tables.User).AllRows();
        }
    }
}
