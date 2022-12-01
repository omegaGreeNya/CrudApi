using System.Diagnostics.Eventing.Reader;

namespace CrudApi.Models
{
	public class Book
	{
		public long Id { get; set; }

		public long? AuthorId { get; set; }

		public string? BookName { get; set; }
	}
}