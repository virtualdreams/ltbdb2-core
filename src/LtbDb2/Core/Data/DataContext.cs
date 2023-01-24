using LtbDb.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LtbDb.Core.Data
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions options)
			: base(options)
		{ }

		public DbSet<Book> Book { get; set; }

		public DbSet<Story> Story { get; set; }

		public DbSet<Tag> Tag { get; set; }
	}
}