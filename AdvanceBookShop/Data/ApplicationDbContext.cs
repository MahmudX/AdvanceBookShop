using AdvanceBookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvanceBookShop.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Book> Books { get; set; }
}