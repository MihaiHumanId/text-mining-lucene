using System;
using System.Linq;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using TextMining.App.Helpers;

namespace TextMining.App.Services;

public class DocumentSearcherService
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
        
        var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
        var parser = new QueryParser(LuceneVersion.LUCENE_48, "token", analyzer);
        var query = parser.Parse(userQuery);

        var searcher = new IndexSearcher(directoryReader);
        var hits = searcher.Search(query, 20).ScoreDocs;

        var documentNames = System.IO.Directory.EnumerateFiles($"{Helper.BasePath}/resources").ToList();
        foreach (var hit in hits)
        {
            var documentId = hit.Doc;
            Console.WriteLine($"Match found in doc: {documentNames[documentId]} with score: {hit.Score}");
        }
    }
}