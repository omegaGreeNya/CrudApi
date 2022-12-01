using System.Diagnostics.Eventing.Reader;

namespace CrudApi.ViewModels
{
	public class BookView
	{
		public long Id { get; set; }

		public string? AuthorName { get; set; }

		public string? BookName { get; set; }
	}
}