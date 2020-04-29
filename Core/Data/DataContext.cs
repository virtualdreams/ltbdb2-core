using Microsoft.EntityFrameworkCore;
using ltbdb.Core.Models;

namespace ltbdb.Core.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions options)
			: base(options)
		{ }

		public DbSet<Book> Book { get; set; }

		public DbSet<Story> Story { get; set; }

		public DbSet<Tag> Tag { get; set; }
	}
}