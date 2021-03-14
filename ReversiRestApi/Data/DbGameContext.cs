using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Data
{
    public class DbGameContext : DbContext
    {
        private readonly ILoggerFactory logger = LoggerFactory.Create(config => config.AddConsole());

        public DbGameContext(DbContextOptions<DbGameContext> options) : base(options) { }

        public DbSet<DbGame> DbGames { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(logger);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbGame>(g =>
            {
                g.HasKey(g => g.ID);
                g.Property(g => g.ID).ValueGeneratedOnAdd();
            });
        }
    }
}
