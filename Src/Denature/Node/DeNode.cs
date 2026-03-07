using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace Osiris.Src.Denature.Node;

public abstract partial class DeNode
{
    public readonly string Id;
    public readonly Guid Uuid;
    public readonly string Tag;
    public string Path{get; private set;}
    public readonly int Depth = 1;
    protected DeDom? Dom{get; private set;}
    protected DeNode? Parent{get; private set;}
    private JsonObject State = [];
    private readonly List<DeNode> Children = [];
    private JsonObject Props = [];
    public DeNode(string id, JsonObject props, string tag = "div")
    {
        Id = id;
        Props = props;
        Tag = tag;
        Path = Id;
        Uuid = Guid.NewGuid();
    }
    public void InitRoot(DeDom dom){Dom = dom;}
    private void SetParent(DeNode parent)
    {
        Parent = parent;
        Path = parent.Path + Path;
        ParentStyle.SetData(parent.BaseStyle.GetData());
        if(parent.Dom is not null) Dom =parent.Dom;
    }
    private void SetProps(JsonObject props){Props = props;}
}
