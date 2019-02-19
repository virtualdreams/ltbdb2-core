using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ltbdb.Core.Models;

namespace ltbdb.Core.Services
{
	public class MySqlContext : DbContext
	{
		public MySqlContext(DbContextOptions options)
			: base(options)
		{ }

		public DbSet<Book> Book { get; set; }

		public DbSet<Story> Story { get; set; }

		public DbSet<Tag> Tag { get; set; }
	}
}