using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace AdvanceBookShop.Features.Search;

public class SearchManager : ISearchManager
{
    private const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
    private static FSDirectory? directory;

    private readonly string luceneDir;

    public SearchManager(IHostEnvironment environment)
    {
        luceneDir = Path.Combine(environment.ContentRootPath, "Lucene_Index");
    }

    private FSDirectory Directory
    {
        get
        {
            if (directory is not null)
            {
                return directory;
            }

            DirectoryInfo info = System.IO.Directory.CreateDirectory(luceneDir);
            return directory = FSDirectory.Open(info);
        }
    }

    public void AddToIndex(List<SearchableBook> searchables)
    {
        DeleteFromIndex(searchables);
        UseWriter(x =>
        {
            foreach (SearchableBook searchable in searchables)
            {
                var doc = new Document();
                foreach (IIndexableField field in searchable.GetFields())
                {
                    doc.Add(field);
                }
                x.AddDocument(doc);
            }
        });
    }

    public void AddToIndex(SearchableBook searchable)
    {
        DeleteFromIndex(searchable);
        UseWriter(x =>
        {
            var doc = new Document();
            foreach (IIndexableField field in searchable.GetFields())
            {
                doc.Add(field);
            }
            x.AddDocument(doc);
        });
    }

    public void DeleteFromIndex(SearchableBook searchable)
    {
        UseWriter(x =>
        {
            x.DeleteDocuments(new Term(nameof(SearchableBook.Id), searchable.Id.ToString()));
        });
    }

    public void Clear()
    {
        UseWriter(x => x.DeleteAll());
    }

    public IEnumerable<SearchResult> Search(string searchQuery, int max)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
            return new List<SearchResult>();
        using var analyzer = new StandardAnalyzer(AppLuceneVersion);
        using DirectoryReader? reader = DirectoryReader.Open(Directory);
        var searcher = new IndexSearcher(reader);
        var parser = new MultiFieldQueryParser(AppLuceneVersion, new[]
        {
            nameof(SearchableBook.Title),
            nameof(SearchableBook.Author),
            nameof(SearchableBook.Description),
            nameof(SearchableBook.Publisher),
            nameof(SearchableBook.SeoText),
            nameof(SearchableBook.Isbn)
        }, analyzer);
        Query? query = parser.Parse(QueryParserBase.Escape(searchQuery.Trim()));
        ScoreDoc[]? hits = searcher.Search(query, null, max, Sort.RELEVANCE).ScoreDocs;

        return hits.Where((x, i) => i <= max)
            .Select(x => new SearchResult(searcher.Doc(x.Doc)))
            .ToList();
    }


    public void DeleteFromIndex(List<SearchableBook> searchables)
    {
        UseWriter(x =>
        {
            foreach (SearchableBook searchable in searchables)
            {
                x.DeleteDocuments(new Term(nameof(SearchableBook.Id), searchable.Id.ToString()));
            }
        });
    }

    private void UseWriter(Action<IndexWriter> action)
    {
        using var analyzer = new StandardAnalyzer(AppLuceneVersion);
        using var writer = new IndexWriter(Directory, new IndexWriterConfig(AppLuceneVersion, analyzer));
        action(writer);
        writer.Commit();
    }
}