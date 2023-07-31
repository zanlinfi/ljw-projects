using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Db
{
    public class MyDesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdEFDbContext>
    {
        public IdEFDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<IdEFDbContext> builder = new();
            string connStr = "Server=.; Database=Demo5_Feb; Trusted_Connection=True; Encrypt=True; TrustServerCertificate=True";
            builder.UseSqlServer(connStr);
            return new IdEFDbContext(builder.Options);
        }
    }
}
