using Lucene.Net.Documents;
using Lucene.Net.Index;
using System.Globalization;
using LuceneBookShop.Models;

namespace LuceneBookShop.Features.Search;

public class SearchableBook
{
    public Guid Id { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public string Title { get; set; }
    public string Isbn { get; set; }
    public string SeoText { get; set; }
    public decimal Price { get; set; }
    public string Publisher { get; set; }

    public SearchableBook(Book book)
    {
        Id = book.Id;
        Author = book.Author;
        Description = book.Description ?? string.Empty;
        ImageUrl = book.ImageUrl;
        Title = book.Title;
        Isbn = book.Isbn ?? string.Empty;
        SeoText = book.SeoText ?? string.Empty;
        Price = book.Price;
        Publisher = book.Publisher;
    }

    public IEnumerable<IIndexableField> GetFields()
    {
        return new Field[]
        {
            new TextField(nameof(Title), Title, Field.Store.YES),
            new TextField(nameof(SeoText), SeoText, Field.Store.NO),
            new TextField(nameof(Author), Author, Field.Store.YES),
            new TextField(nameof(Publisher), Publisher, Field.Store.NO),
            new TextField(nameof(Description), Description, Field.Store.NO),
            new TextField(nameof(Isbn), Isbn, Field.Store.NO),
            new StringField(nameof(Id), Id.ToString(), Field.Store.YES),
            new StringField(nameof(ImageUrl), ImageUrl ?? string.Empty, Field.Store.YES),
            new StringField(nameof(Price), Price.ToString(CultureInfo.InvariantCulture), Field.Store.YES),
        };
    }
}