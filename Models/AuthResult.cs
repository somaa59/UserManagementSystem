using System.IdentityModel.Tokens.Jwt;

namespace UserManagementSystem.Models
{
	public class AuthResult
	{
        public bool Succeeded { get; set; }
		public IEnumerable<string> Errors { get; set; }
		public JwtSecurityToken Token { get; set; }
		public ApplicationUser User { get; set; }
    }
}
