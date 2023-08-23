using Microsoft.EntityFrameworkCore;

namespace GumbyBot.Database
{
    public class GumbyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Leadership> Leadership { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=gumbybot.db;Version=3;");
        }

    }
}