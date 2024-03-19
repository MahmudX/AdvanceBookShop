using LuceneBookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace LuceneBookShop.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Book> Books { get; set; }
}