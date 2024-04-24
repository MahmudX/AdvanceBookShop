namespace AdvanceBookShop.Models;

public class Book
{
    public Book()
    {
        Id = Guid.NewGuid();
        PublishedOn = DateTime.UtcNow;
    }
    public Guid Id { get; set; }
    public string Title { get; set; } 
    public string Author { get; set; } 
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? Isbn { get; set; }
    public DateTime PublishedOn { get; set; }
    public string Publisher { get; set; }
    public string? SeoText { get; set; } 
}