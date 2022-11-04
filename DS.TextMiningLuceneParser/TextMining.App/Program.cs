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
        
         var option = ConsoleOption.None;
         while (option != ConsoleOption.Exit)
         {
             Console.WriteLine("------------------------------");
             Console.WriteLine("1. Index files");
             Console.WriteLine("2. Search files");
             Console.WriteLine("5. Exit");
             Console.WriteLine("------------------------------");
             Console.Write("Choose an action index: ");
             var userOpt = int.Parse(Console.ReadLine()!);
             option = (ConsoleOption) userOpt;
             switch (option)
             {
                 case ConsoleOption.IndexFiles:
                     textIndexerService.IndexFiles("../../../resources");
                     break;
                 case ConsoleOption.SearchFiles:
                     Console.Write("Provide a query: ");
                     var query = Console.ReadLine();
                     // File.WriteAllText("../../../TextMining.Indexer/Helpers/query.txt", Console.Read().ToString());
                     //var query = File.ReadAllText("../../../TextMining.Indexer/Helpers/query.txt");
                     documentSearcherService.SearchDocuments(query);
                     break;
                 case ConsoleOption.Exit:
                     Environment.Exit(0);
                     break;
                 default:
                     Console.WriteLine("This value is unknown");
                     break;
             }
         }
    }
}
