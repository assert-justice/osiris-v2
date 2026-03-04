using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;
using Osiris.Src.Denature.Theme;

namespace Osiris.Src.Denature;

public abstract class DeNode
{
    public readonly string Id;
    // public readonly bool Persistent;
    private readonly DeDom? Dom;
    private readonly string Tag;
    public string Path{get; private set;}
    private DeNode? Parent;
    private JsonObject State = [];
    private HashSet<string> Clades = [];
    private readonly List<DeNode> Children = [];
    private readonly Dictionary<string, DeNode> FreedChildren = [];
    private JsonObject Props = [];
    // Todo: make setting the theme dirty the node
    protected DeTheme Theme = new();
    public readonly int Depth = 1;
    // Todo: figure out how to make node initialization less annoying. Set parent/dom via
    public DeNode(string id, JsonObject props, DeTheme? theme = null, string tag = "div", HashSet<string>? clades = null/*, bool persistent = false*/)
    {
        Id = id;
        Props = props;
        Theme = theme ?? new();
        Tag = tag;
        Clades = clades ?? [];
        // Todo: reconsider the whole persistent thing. Is it premature optimization?
        // Persistent = persistent;
        // if(persistent) Dom.MarkPersistent(this);
        Path = Id;;
    }
    // public DeNode(string id, JsonObject props, DeTheme? theme = null, bool persistent = false) 
    //     : this(id, props, theme, persistent)
    // {
    //     // Note: adding the child to the parent's list of children is the parent's responsibility
    //     Parent = parent;
    //     Path = parent.Path + Path;
    //     Depth = parent.Depth + 1;
    // }
    private void SetDom(){}
    private void SetParent(DeNode parent)
    {
        Parent = parent;
        Path = parent.Path + Path;
    }
    private void OnMountInternal()
    {
        foreach (var child in Children)
        {
            child.OnMountInternal();
        }
        OnMount();
    }
    private void OnUnmountInternal()
    {
        foreach (var child in Children)
        {
            child.OnUnmountInternal();
        }
        OnUnmount();
    }
    protected virtual void OnMount(){}
    protected virtual void OnUnmount(){}
    public void RenderInternal(JsonObject props)
    {
        if(Dom is null) throw new Exception("No dom set");
        foreach (var child in Children)
        {
            FreedChildren.Add(child.Id, child);
        }
        Children.Clear();
        foreach (var child in Render(props))
        {
            if(FreedChildren.TryGetValue(child.Id, out var oldChild) && child.GetType() == oldChild.GetType())
            {
                FreedChildren.Remove(child.Id);
                oldChild.SetProps(child.Props);
                Children.Add(oldChild);
            }
            else
            {
                child.OnMountInternal();
                Children.Add(child);
            }
            // Todo: handle props!
            child.Render([]);
        }
        // Properly free the excess children
        foreach (var freedChild in FreedChildren.Values)
        {
            freedChild.OnUnmountInternal();
            Dom.MarkFreed(freedChild);
        }
        Dom.MarkClean(this);
    }
    public virtual IEnumerable<DeNode> Render(JsonObject props){yield break;}
    public void SetProps(JsonObject props){Props = props;}
}
