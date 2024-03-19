using LuceneBookShop.Data;
using LuceneBookShop.Features.Search;
using LuceneBookShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace LuceneBookShop.Api;

[ApiController]
[Route("api/[controller]/[action]")]
public class BookController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ISearchManager _searchManager;

    public BookController(ApplicationDbContext context, ISearchManager searchManager)
    {
        _context = context;
        _searchManager = searchManager;
    }

    [HttpPost]
    public IActionResult CreateBook(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
        _searchManager.AddToIndex(new SearchableBook(book));
        return Ok();
    }

    [HttpPost]
    public IActionResult CreateBooks(List<Book> books)
    {
        _context.Books.AddRange(books);
        _context.SaveChanges();
        _searchManager.AddToIndex(books.Select(x => new SearchableBook(x)).ToList());
        return Ok();
    }

    [HttpPost]
    public IActionResult IndexAll()
    {
        _searchManager.Clear();
        _searchManager.AddToIndex(_context.Books.Select(x => new SearchableBook(x)).ToList());
        return Ok();
    }

    [HttpGet]
    public IActionResult GetBooks()
    {
        var books = _context.Books.ToList();
        return Ok(books);
    }

    [HttpGet]
    public IActionResult TSearchBook(string query)
    {
        List<Book> books = _context.Books.Where(x => x.Title.Contains(query) || x.Author.Contains(query)).ToList();
        return Ok(books);
    }

    
    [HttpGet]
    public IActionResult SearchBook(string query)
    {
        List<SearchResult> searchResults = _searchManager.Search(query.ToLowerInvariant()).ToList();
        foreach (SearchResult book in searchResults)
        {
            book.Parse(x =>
            {
                book.Author = x.Get(nameof(SearchableBook.Author));
                book.Title = x.Get(nameof(SearchableBook.Title));
                book.Id = new Guid(x.Get(nameof(SearchableBook.Id)));
                book.ImageUrl = x.Get(nameof(SearchableBook.ImageUrl));
                book.Price = decimal.Parse(x.Get(nameof(SearchableBook.Price)));
            });
        }
        return Ok(searchResults.Select(sr => new
        {
            sr.Title, sr.Author, sr.Price, sr.Id, sr.ImageUrl
        }));
    }
}