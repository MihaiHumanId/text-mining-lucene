using System;
using TextMining.Indexer.Services;
using TextMining.Searcher.Services;

namespace TextMining.App;

internal class Program
{
    public static void Main(string[] args)
    {
        var textIndexerService = new TextIndexerService();
        var documentSearcherService = new DocumentSearcherService();
        
        var index = textIndexerService.IndexFiles("../../../resources");
        
        Console.WriteLine("Files indexed...");
        
        Console.WriteLine("Write your query/question");
        var userQuery = Console.ReadLine();
        
        documentSearcherService.SearchDocuments(index, userQuery);
    }
}
