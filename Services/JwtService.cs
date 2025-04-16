using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserManagementSystem.Services
{
	public class JwtService : IJwtService
	{
		public JwtSecurityToken GenerateToken(IEnumerable<Claim> claims)
		{
			//head + Payload +Signeture
			var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("User1Mangement2System3@Version2!"));
			SigningCredentials signincred = 
				new SigningCredentials(SignInKey,SecurityAlgorithms.HmacSha256);

			JwtSecurityToken token = new JwtSecurityToken( 
					issuer: "https://localhost:44367/",
					audience: "https://localhost:44367/",
					claims:claims,
					expires: DateTime.Now.AddHours(1),
					signingCredentials:signincred
				);

			return token;
		}

		public string WriteToken(JwtSecurityToken token)
		{
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
