namespace TextMining.Indexer.Services;

public interface ITextIndexer
{
    Lucene.Net.Store.Directory IndexFiles(string filePath);
}