using System.ComponentModel.DataAnnotations;

public record UserDto(string UserName, string Password);

public record UserModel
{
   // public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
    //public byte[] PasswordHash { get; set; }
    //public byte[] PasswordSalt { get; set; }
}

public interface IUserRepositoryService
{
    UserDto GetUser(UserModel userModel);
}

public class UserRepositoryService : IUserRepositoryService
{
    private List<UserDto> _users => new()
    {
        new("Nada","p@ssword"), new("Admin","p@ssword")
    };
    public UserDto GetUser(UserModel userModel)
    {
        return _users.FirstOrDefault(x => string.Equals(x.UserName, userModel.UserName) && string.Equals(x.Password, userModel.Password));
    }
}