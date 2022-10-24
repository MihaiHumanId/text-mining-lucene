using System;
using System.IO;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using TikaOnDotNet.TextExtraction;
using Version = Lucene.Net.Util.Version;

namespace TextMining.Indexer.Services;

public class TextIndexerService : ITextIndexer
{
    private readonly TextExtractor _textExtractor;
    public TextIndexerService()
    {
        _textExtractor = new TextExtractor();
    }
    
    public Lucene.Net.Store.Directory IndexFiles(string filePath)
    {
        var directory = new RAMDirectory();

        using var analyzer = new StandardAnalyzer(Version.LUCENE_30);
        using var writer = new IndexWriter(directory, analyzer, new IndexWriter.MaxFieldLength(1000));

        foreach (var fileName in System.IO.Directory.EnumerateFiles(filePath))
        {
            var fileContent = ReadFile(fileName);
            var document = new Document();

            foreach (var token in fileContent.Split())
            {
                document.Add(new Field("token", token, Field.Store.YES, Field.Index.ANALYZED));
            }

            writer.AddDocument(document);
        }
        
        writer.Optimize();
        writer.Flush(true, true, true);

        return directory;
    }

    private string ReadFile(string fileName)
    {
        try
        {
            return _textExtractor.Extract(fileName).Text;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to read document with Tika: {ex}");
            throw;
        }
    }
}