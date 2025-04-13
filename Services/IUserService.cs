using UserManagementSystem.Models;

namespace UserManagementSystem.Services
{
	public interface IUserService
	{
		Task<ApplicationUser> FindUserByEmailAsync(string email);
		
	}
}
