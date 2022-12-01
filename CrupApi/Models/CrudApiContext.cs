using Microsoft.EntityFrameworkCore;

namespace CrudApi.Models
{
	public class CrudApiContext : DbContext
	{
		public CrudApiContext(DbContextOptions<CrudApiContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}
		public DbSet<Book> Books { get; set; } = null!;
		public DbSet<Author> Authors { get; set; } = null!;
	}
}
