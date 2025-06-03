using System.Text.RegularExpressions;
using System.Collections;

namespace HtmlUtils;

public class HtmlTag
{
    public string   Name       => _inner["tagname"];
    public string   Id         => _inner.GetValueOrDefault("id", "");
    public string[] Classes    => _inner.GetValueOrDefault("class", "").Split(' ');
    public bool     IsVoidElem { get; }
    public string   ClosingTag => ( this.IsVoidElem ? "" : $"</{this.Name}>" );
    public int      Length     => _fullTag.Length;

    private readonly string _fullTag;

    public override string ToString() => this.Name + (string.IsNullOrEmpty(this.Id) ? "" : $"#{this.Id}");

    public HtmlTag(Match fullTag)
    {
        _inner          = new Dictionary<string, string>();
        _fullTag        = fullTag.Value;
        this["tagname"] = fullTag.Groups["tagname"].Value;

        this.IsVoidElem = ( _voidElemNames.Contains(this.Name) );
        
        foreach ( Match match in Regex.Matches(fullTag.Value, RX_ATTRMATCH) )
        {
            string value = Regex.Replace(match.Groups["val"].Value, "[\"']", string.Empty);
            this[match.Groups["key"].Value.ToLower()] = value.ToLower();
        }
    }

    public bool CheckAttr(string attrName, string attrValue)
    {
        // Check if Key Exists
        if ( !this.TryGetValue(attrName.ToLower(), out string? value) || value == null ) return false;

        return attrValue.Split(' ').All(val => value.Split(' ').Contains(val.ToLower()));
    }
    
    // See: https://developer.mozilla.org/en-US/docs/Glossary/Void_element
    private readonly string[] _voidElemNames =
    [
        "area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "param", "source", "track",
        "wbr"
    ];
    public const string RX_ATTRMATCH = @"(?<=\s)(?<key>[^>=]+)(=(?<val>""[^""]+""|'[^']+'))?";
    
    // Dict Methods
    public string this[string key]
    {
        get => _inner[key];
        set => _inner[key] = value;
    }
    
    private void Add(string key, string value) => _inner.Add(key, value);
    
    private void Remove(string key) => _inner.Remove(key);

    public bool ContainsKey(string key) => _inner.ContainsKey(key);

    public bool TryGetValue(string key, out string? value) => _inner.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _inner.GetEnumerator();

    // Inner Collection
    private Dictionary<string, string>                 _inner;
}