using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Configuration;
using System.Text;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
}

public class TokenService : ITokenService
{
    IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT")["Key"]));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            issuer: config.GetSection("JWT")["Issuer"],
            audience: config.GetSection("JWT")["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: signinCredentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        return tokenString;
    }


    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
