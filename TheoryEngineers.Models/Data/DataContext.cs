using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheoryEngineers.Models.Models;

namespace TheoryEngineers.Models.Data
{
    public class DataContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, 
        UserLogin, RoleClaim, UserToken>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
