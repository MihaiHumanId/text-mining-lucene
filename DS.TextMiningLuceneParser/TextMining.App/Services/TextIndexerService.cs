using System;
using System.Linq;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;
using TextMining.App.Helpers;
using TikaOnDotNet.TextExtraction;

namespace TextMining.App.Services;

public class TextIndexerService
{
    private readonly TextExtractor _textExtractor;

    public TextIndexerService()
    {
        _textExtractor = new TextExtractor();
    }

    public void IndexFiles(string filePath)
    {
        var directory = new SimpleFSDirectory($"{Helper.BasePath}/index");

        var analyzer = new SimpleAnalyzer(LuceneVersion.LUCENE_48);
        var indexConfig = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
        using var writer = new IndexWriter(directory, indexConfig);
        writer.DeleteAll();

        var files = System.IO.Directory.EnumerateFiles(filePath);
        foreach (var fileName in files)
        {
            var fileContent = ReadFile(fileName);
            fileContent = Helper.CleanText(fileContent);
            fileContent = Helper.RemoveStopWords(fileContent);
            Console.WriteLine($"Document {fileName.Split('/').Last()} with content: {fileContent}");
            var document = new Document();

            foreach (var token in fileContent.Split()) document.Add(new TextField("token", token, Field.Store.YES));

            writer.AddDocument(document);
        }

        writer.Flush(false, false);
        Console.WriteLine($"{files.Count()} files successfully indexed.");
    }

    private string ReadFile(string fileName)
    {
        try
        {
            return _textExtractor.Extract(fileName).Text;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to read document {fileName} with Tika: {ex}");
            throw;
        }
    }
}