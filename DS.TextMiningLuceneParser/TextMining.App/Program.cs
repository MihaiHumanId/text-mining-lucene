using System;
using System.IO;
using System.Text;
using TextMining.App.Services;

namespace TextMining.App;

internal static class Program
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
            option = (ConsoleOption)userOpt;
            switch (option)
            {
                case ConsoleOption.IndexFiles:
                    textIndexerService.IndexFiles($"{Helpers.Helper.BasePath}/resources");
                    break;
                case ConsoleOption.SearchFiles:
                    var query = File.ReadAllText($"{Helpers.Helper.BasePath}/query.txt", Encoding.UTF8);
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