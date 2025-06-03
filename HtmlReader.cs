using System.Data.SqlTypes;
using System.Text.RegularExpressions;

namespace HtmlUtils;

public class HtmlReader
{
    public string Document;
    public Match  Match;

    public HtmlReader(string html)
    {
        Document = html;
        Match    = Regex.Match(html, RX_TAGMATCH);
    }
    
    public void NextMatch() => Match = Match.NextMatch();
    
    // Static
    public const string RX_TAGMATCH  = @"</?(?<tagname>[^!>\s]+)(?<attr>\s(?:[^>]+)(?:=(""[^>]+""|'[^>]+'))?)*/?>";

    /// <summary>
    /// Removes newlines and tab characters
    /// </summary>
    /// <param name="path">Path to HTML File</param>
    /// <returns></returns>
    public static string ReadFile(string path) => File.ReadAllText(path);
}