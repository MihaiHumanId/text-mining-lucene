using System;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using TextMining.Indexer.Helpers;

namespace TextMining.Searcher.Services;

public class DocumentSearcherService : IDocumentSearcher
{
    public void SearchDocuments(string userQuery)
    {
        var fsDirectory = FSDirectory.Open($"{Helper.BasePath}/index");
        var directoryReader = DirectoryReader.Open(fsDirectory);

        userQuery = Helper.CleanText(userQuery);
        if (Helper.StopWords.Contains(userQuery))
        {
            Console.WriteLine($"Your query <{userQuery}> is a stop word. This kind of search yields no results.");
            return;
        }
        
        userQuery = Helper.RemoveStopWords(userQuery);
        Console.WriteLine($"Your query after cleanup: {userQuery}");

        var query = new MultiPhraseQuery
        {
            new Term("token", userQuery)
        };
        
        var searcher = new IndexSearcher(directoryReader);
        var hits = searcher.Search(query, 20).ScoreDocs;

        foreach (var hit in hits)
        {
            var documentId = hit.Doc;
            var document = searcher.Doc(documentId);
            Console.WriteLine($"Match found in doc: {documentId}");
        }
    }
}