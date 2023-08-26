using Microsoft.EntityFrameworkCore;

namespace GumbyBot.Services
{
	
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

		public static string ConnectionString => @"Data Source=GumbyBot.db";
		public DbSet<Models.Currency> Currency { get; set; }
	}
}
