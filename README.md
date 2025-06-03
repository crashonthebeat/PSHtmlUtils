# PSHtmlUtils

This is a working repository for an HTML parser for PowerShell, which can probably also be imported as a class library 
in .NET. I'm intending for this to work a little like the built-in Json reader but also a little like Javascript so that 
I can possibly mess around with this in blazor.

This is the first PowerShell module I've written that isn't just three-to-seven powershell cmdlets in trench coats. It's
all single threaded because I believe in patience (read: async still confuses me).

## CmdletReference
### Read-Html
`Read-Html [-Path <Path to File>] [-Html <html string>]`

`Get-Content 'Path\to\html\file.html | Read-Html`

**Inputs**: string Path or string Html

**Outputs**: HtmlDocument

### Find-Node
`Find-Node -Document <html string> -Tag <name of tag>`

`Find-Node -Document <html string> -Id <id>`

`Find-Node -Document <html string> -Class <class names as string of space separated classes>`

`Find-Node -Document <html string> -AttributeName <other attribute to search> -AttributeValue <value>`

Document also accepts pipeline input

**Outputs**: NodeList

## Type Reference
### HtmlDocument
The main Html Document. Tbh I might just make this inherit HtmlNode and make Read-Html return the Html root.

### HtmlNode
Like JsonNode, this represents a tag enclosing a some HtmlNodes

### HtmlTag
Contains all the tag attributes for a specific HtmlNode

### NodeList
Really just a List<HtmlNode> given a nice fancy name