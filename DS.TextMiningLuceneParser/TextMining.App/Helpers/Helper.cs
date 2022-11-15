using System.Collections.Generic;
using System.IO;
using System.Text;
using opennlp.tools.stemmer.snowball;

namespace TextMining.App.Helpers;

public static class Helper
{
    private static readonly SnowballStemmer Stemmer = new(SnowballStemmer.ALGORITHM.ROMANIAN);

    private static readonly Dictionary<string, string> Diacritics = new()
    {
        { "Ă", "a" },
        { "ă", "a" },
        { "Â", "a" },
        { "â", "a" },
        { "Î", "i" },
        { "î", "i" },
        { "Ș", "s" },
        { "ș", "s" },
        { "Ş", "s" },
        { "ş", "s" },
        { "Ț", "t" },
        { "ț", "t" }
    };

    public static readonly string BasePath = "../../..";

    public static readonly List<string> StopWords = new(
        File.ReadAllLines($"{BasePath}/TextMining.App/Helpers/ro-stop-words.txt"));

    public static string CleanText(string content)
    {
        content = content.Trim();
        // lowercase
        content = content.ToLower();
        // remove accents
        content = RemoveAccent(content);

        return content;
    }

    public static string RemoveStopWords(string content)
    {
        foreach (var token in content.Split())
        {
            if (StopWords.Contains(token)) content = content.Replace(token, "");
            var stemmedToken = Stemmer.stem(token);
            content = content.Replace(token, stemmedToken.toString());
        }

        return content;
    }

    private static string RemoveAccent(string content)
    {
        var text = new StringBuilder();

        foreach (var c in content)
        {
            var isDiacritic = Diacritics.TryGetValue(c.ToString(), out var value);
            if (isDiacritic)
                text.Append(value);
            else
                text.Append(c);
        }

        return text.ToString();
    }
}