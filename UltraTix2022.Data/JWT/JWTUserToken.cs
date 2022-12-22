using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;

namespace UltraTix2022.API.UltraTix2022.Data.JWT
{
    public class JWTUserToken
    {
        public static string GenerateJWTTokenUser(UserTokenModel user)
        {
            JwtSecurityToken? tokenUser = null;

            if (user == null || user.Role == null)
            {
                return "";
            }

            if (user.Role != null) //Admin || manager || show staff || ticket inspector || user || artist
            {
                tokenUser = new JwtSecurityToken(
                issuer: "https://securetoken.google.com/untratix-s2022",
                audience: "untratix-s2022",
                claims: new[] {
                 //Id
                 new Claim(Commons.JWTClaimID, user.Id.ToString()),
                 //fullname
                 new Claim(Commons.JWTClaimName, user.Name),
                 //Avatar
                 new Claim (Commons.JWTClaimEmail, user.Email),
                 //Role Id
                 new Claim(Commons.JWTClaimRoleID, user.RoleId.ToString()),
                 //Role
                 new Claim(ClaimTypes.Role, user.Role),
                },
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ultratix202212345678")),
                        algorithm: SecurityAlgorithms.HmacSha256
                        )
                );
            }
            return new JwtSecurityTokenHandler().WriteToken(tokenUser);
        }
    }
}
