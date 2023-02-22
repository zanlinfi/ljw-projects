using EntityClass;
using Microsoft.EntityFrameworkCore;
namespace EFCore.Db
{
    public class BookEFDbContext : DbContext
    {
        
        public DbSet<Book> Books { get; set; }

        public BookEFDbContext(DbContextOptions<BookEFDbContext> options) : base(options) { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

    }
}
