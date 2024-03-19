using Lucene.Net.Documents;

namespace LuceneBookShop.Features.Search;

public class SearchResult
{
    private readonly Document _doc;
    public SearchResult(Document doc)
    {
        _doc = doc;
    }
    public Guid Id { get; set; }
    public string ImageUrl { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }

    public void Parse(Action<Document> action)
    {
        action(_doc);
    }
}