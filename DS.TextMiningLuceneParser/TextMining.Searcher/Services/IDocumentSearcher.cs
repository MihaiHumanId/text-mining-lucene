namespace TextMining.Searcher.Services;

public interface IDocumentSearcher
{
    void SearchDocuments(Lucene.Net.Store.Directory index, string query);
}