using System;
using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class LogServiceContext : DbContext
    {
        public DbSet<Log> LogsTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=Test;Trusted_Connection=True;");
        }
    }
}
