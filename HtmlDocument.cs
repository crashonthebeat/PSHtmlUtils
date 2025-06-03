namespace HtmlUtils;

public class HtmlDocument
{
    public HtmlNode Root        { get; }
    public string   RawDocument { get; }

    public HtmlDocument(string raw)
    {
        this.RawDocument = raw;
        HtmlReader reader = new(this.RawDocument);

        this.Root = new HtmlNode(reader);
    }
    
    public static HtmlDocument ReadFile(string path) => new(File.ReadAllText(path));
}