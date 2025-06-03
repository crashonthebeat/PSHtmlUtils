namespace HtmlUtils;

public class HtmlNode
{
    public HtmlTag   HtmlTag    { get; }
    public int       NodeLength { get; private set; }
    public string    InnerHtml  { get; }
    public int       DocIndex   { get; } // What index the tag starts at
    public NodeList? ChildNodes { get; }
    public HtmlNode? ParentNode { get; init; }
    //public HtmlNode?  ParentNode { get; set; }

    public HtmlNode(HtmlReader reader)
    {
        this.HtmlTag    = new HtmlTag(reader.Match);
        this.NodeLength = this.HtmlTag.Length;
        this.DocIndex   = reader.Match.Index;

        // Void Elements do not have children and do not increase indent level
        if ( !this.HtmlTag.IsVoidElem ) this.ChildNodes = this.InitChildNodes(reader);

        int i = this.DocIndex      + this.NodeLength;
        int l = reader.Match.Index - i;

        this.InnerHtml = ( l > 0 && reader.Match.Index < reader.Document.Length )
                             ? reader.Document.Substring(i, l)
                             : "";
    }

    public NodeList GetNodesById(string id) => this.GetNodesByAttr("id", id);

    public NodeList GetNodesByTag(string tag) => this.GetNodesByAttr("tagname", tag);

    public NodeList GetNodesByClass(string className) => this.GetNodesByAttr("class", className);

    public NodeList GetNodesByAttr(string attrName, string attrValue)
    {
        NodeList nodes = [ ];
        if ( this.HtmlTag.CheckAttr(attrName, attrValue) ) nodes.Add(this);
        if ( this.ChildNodes is not { Count: > 0 } ) return nodes;

        foreach ( HtmlNode node in this.ChildNodes ) { nodes.AddRange(node.GetNodesByAttr(attrName, attrValue)); }

        return nodes;
    }

    public HtmlNode? GetNextNode()
    {
        int i = this.ParentNode!.ChildNodes!.IndexOf(this) + 1;
        return i == this.ParentNode.ChildNodes.Count ? null : this.ParentNode!.ChildNodes[i];
    }

    private NodeList InitChildNodes(HtmlReader reader)
    {
        NodeList children = [ ];
        reader.NextMatch();

        while ( reader.Match.Success && reader.Match.Value != this.HtmlTag.ClosingTag )
        {
            children.Add(new HtmlNode(reader) { ParentNode = this });
            reader.NextMatch();
        }

        return children;
    }

    public override string ToString() => this.HtmlTag.ToString();
}

public class NodeList : List<HtmlNode> { }