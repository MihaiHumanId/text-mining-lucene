using System;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;

namespace TextMining.Searcher.Services;

public class DocumentSearcherService : IDocumentSearcher
{
    public void SearchDocuments(Lucene.Net.Store.Directory index, string userQuery)
    {
        using var reader = IndexReader.Open(index, true);
        using var searcher = new IndexSearcher(reader);
        using var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

        var queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "token", analyzer);
        try
        {
            var query = queryParser.Parse(userQuery);
            var collector = TopScoreDocCollector.Create(1000, true);
            searcher.Search(query, collector);

            var matches = collector.TopDocs().ScoreDocs;

            foreach (var item in matches)
            {
                var documentId = item.Doc;
                var document = searcher.Doc(documentId);

                // var matchesInDocument = document.GetFields("token");
                // foreach (var match in matchesInDocument)
                // {
                //     Console.WriteLine($"Match: {match.StringValue}");
                // }
                Console.WriteLine($"Match found in doc {documentId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
        
    }
}