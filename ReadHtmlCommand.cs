using System.Management.Automation;

namespace HtmlUtils;

[Cmdlet(VerbsCommunications.Read, "Html"), OutputType(typeof(HtmlDocument))]
public class ReadHtmlCommand : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true, ParameterSetName = "ByPath")]
    [ValidateNotNullOrEmpty]
    public string Path { get; set; }

    [Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "ByContent")]
    [ValidateNotNullOrEmpty]
    public string Html { get; set; }

    protected override void ProcessRecord()
    {
        string html;
        if ( this.ParameterSetName == "ByPath" ) html = File.ReadAllText(this.Path);
        else html = this.Html;
        HtmlDocument doc = new(html);

        this.WriteObject(doc);
    }
}

[Cmdlet(VerbsCommon.Find, "Nodes"), OutputType(typeof(NodeList))]
public class FindNodesByAttr : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "tagname")]
    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "id")]
    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "class")]
    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByAttr")]
    public HtmlDocument Document { get; set; }
    
    [Parameter(Mandatory = true, ParameterSetName = "tagname")]
    public string Tag { get; set; }
    
    [Parameter(Mandatory = true, ParameterSetName = "id")]
    public string Id { get; set; }
    
    [Parameter(Mandatory = true, ParameterSetName = "class")]
    public string Class { get; set; }
    
    [Parameter(Mandatory = true, ParameterSetName = "ByAttr")]
    [Alias("AttrName, Name")]
    public string AttributeName { get; set; }
    
    [Parameter(Mandatory = true, ParameterSetName = "ByAttr")]
    [Alias("AttrValue, Value")]
    public string AttributeValue { get; set; }

    protected override void ProcessRecord()
    {
        NodeList nodes = this.ParameterSetName switch
        {
            "tagname" => this.Document.Root.GetNodesByTag(this.Tag),
            "id"      => this.Document.Root.GetNodesById(this.Id),
            "class"   => this.Document.Root.GetNodesByClass(this.Class),
            "byattr"  => this.Document.Root.GetNodesByAttr(this.AttributeName, this.AttributeValue),
            _   => throw new ParameterBindingException("Invalid Parameter Combination") // how would this even happen
        };

        this.WriteObject(nodes);
    }
}