using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrudApi.Models;
using CrudApi.ViewModels;

namespace CrudApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BooksController : ControllerBase
	{
		private readonly CrudApiContext _context;

		public BooksController(CrudApiContext context)
		{
			_context = context;
		}

		// GET: api/Books
		[HttpGet]
		public async Task<ActionResult<IEnumerable<BookView>>> GetBooks()
		{
			return await _context.Books
				.Select(book => BookToView(_context, book))
				.ToListAsync();
		}

		// GET: api/Books/5
		[HttpGet("{id}")]
		public async Task<ActionResult<BookView>> GetBook(long id)
		{
			var book = await _context.Books.FindAsync(id);
			if (book == null)
			{
				return NotFound();
			}

			return BookToView(_context, book);
		}

		// PUT: api/Books/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutBook(long id, BookView bookView)
		{
			if (id != bookView.Id)
			{
				return BadRequest();
			}

			var book = ViewToBook(_context, bookView);

			_context.Entry(book).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!BookExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Books
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<BookView>> PostBook(BookView bookView)
		{
			var book = ViewToBook(_context, bookView);
			_context.Books.Add(book);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetBook), new { id = book.Id }, BookToView(_context, book));
		}

		// DELETE: api/Books/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBook(long id)
		{
			var book = await _context.Books.FindAsync(id);
			if (book == null)
			{
				return NotFound();
			}

			_context.Books.Remove(book);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool BookExists(long id)
		{
			return _context.Books.Any(e => e.Id == id);
		}

		private static BookView BookToView(in CrudApiContext context, Book book)
		{
			return new BookView()
			{
				Id = book.Id,
				AuthorName = context.Authors.FirstOrDefault(a => a.Id == book.AuthorId)?.AuthorName,
				BookName = book.BookName
			};
		}

		private static Book ViewToBook(in CrudApiContext context, BookView bookView)
		{
			return new Book()
			{
				Id = bookView.Id,
				AuthorId = context.Authors.FirstOrDefault(a => a.AuthorName == bookView.AuthorName)?.Id,
				BookName = bookView.BookName
			};
		}
	}
}
