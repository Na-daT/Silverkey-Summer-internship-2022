public class LoginModel
{
    public string Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string RefreshTokenExpiryTime { get; set; }

    public LoginModel()
    {
        Id = Guid.NewGuid().ToString();
    }

    public LoginModel(string id, string userName, string password, string refreshToken, string refreshTokenExpiryTime)
    {
        Id = id;
        UserName = userName;
        Password = password;
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
    }
}

