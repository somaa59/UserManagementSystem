using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Models;

namespace UserManagementSystem.Data
{
    public class AppDbContext :IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options)
            : base(options) { }
    }
}
