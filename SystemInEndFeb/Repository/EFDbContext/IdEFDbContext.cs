using EntityClass;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Db
{
    public class IdEFDbContext : IdentityDbContext<User, Role, long>
    {
        public IdEFDbContext(DbContextOptions<IdEFDbContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
