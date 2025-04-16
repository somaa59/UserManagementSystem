using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UserManagementSystem.Services
{
	public interface IJwtService
	{
		JwtSecurityToken GenerateToken(IEnumerable<Claim> claims);
		string WriteToken (JwtSecurityToken token);	
	}
}
