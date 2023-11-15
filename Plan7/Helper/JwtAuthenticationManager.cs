using System.Security.Claims;
using DB.Model.UserInfo;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Plan7.Helper;

/// <summary>
/// 
/// </summary>
public class JwtAuthenticationManager
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public JwtAuthenticationManager(IConfiguration configuration) => _configuration = configuration;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string? GenerateJwtToken(User user)
    {
        if (!double.TryParse(_configuration["JWT:ValidTimeLive"], out var liveTime)) 
            return null;
        
        var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(liveTime);
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"] ?? "Kd1vnu^9$6@0266@lY9ZHs9Gn#db3d82S%Sg$aa5A"));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Name, user.FFL, ClaimValueTypes.String),
            new("UserId", user.Id.ToString(), ClaimValueTypes.String),
            new("Profession", user!.Profession!.Title, ClaimValueTypes.String),
            new("Login", user.ProfessionNumber, ClaimValueTypes.String),
            new(ClaimsIdentity.DefaultRoleClaimType, user!.Role!.Title, ClaimValueTypes.String),
        });

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = tokenExpiryTimeStamp,
            SigningCredentials = credentials,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"]
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);

        return token;

    }
}