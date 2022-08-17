//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//public interface ITokenService
//{
//    string BuildToken(string key, string issuer, string audience, UserDto user);
//}
//public class TokenService : ITokenService
//{
//    private TimeSpan ExpiryDuration = new TimeSpan(0, 30, 0);
//    public string BuildToken(string key, string issuer, string audience, UserDto user)
//    {
//        var claims = new[]
//        {
//            new Claim(ClaimTypes.Name, user.UserName),
//            new Claim(ClaimTypes.NameIdentifier,
//            Guid.NewGuid().ToString())
//        };
//        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
//        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
//        var tokenDescriptor = new JwtSecurityToken(issuer, audience, claims,
//        expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
//        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
//    }
//}
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
    //ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}

public class TokenService : ITokenService
{
    IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    //string myKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JWT")["Key"];
    //string _issuer = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JWT")["Issuer"];
    //string _audience = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JWT")["Audience"];
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

    //public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    //{
    //    var tokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
    //        ValidateIssuer = false,
    //        ValidateIssuerSigningKey = true,
    //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(myKey)),
    //        ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
    //    };
    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    SecurityToken securityToken;
    //    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
    //    var jwtSecurityToken = securityToken as JwtSecurityToken;
    //    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
    //        throw new SecurityTokenException("Invalid token");
    //    return principal;
    //}
}
