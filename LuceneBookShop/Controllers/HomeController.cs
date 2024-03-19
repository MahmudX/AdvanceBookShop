using System.Diagnostics;
using LuceneBookShop.Data;
using LuceneBookShop.Features.Search;
using Microsoft.AspNetCore.Mvc;
using LuceneBookShop.Models;

namespace LuceneBookShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISearchManager _searchManager;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger,ISearchManager searchManager, ApplicationDbContext context)
    {
        _logger = logger;
        _searchManager = searchManager;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Route("/book/details")]
    public IActionResult Details(Guid id)
    {
        var book = _context.Books.FirstOrDefault(x => x.Id == id);
        return View(book);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}