namespace LuceneBookShop.Features.Search;

public interface ISearchManager
{
    void AddToIndex(List<SearchableBook> searchables);
    void AddToIndex(SearchableBook searchable);
    void DeleteFromIndex(List<SearchableBook> searchables);
    void DeleteFromIndex(SearchableBook searchable);
    void Clear();
    IEnumerable<SearchResult> Search(string searchQuery, int max = 10);
}