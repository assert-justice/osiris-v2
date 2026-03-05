using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;
using Osiris.Src.Denature.Theme;
using Osiris.Src.Roja;

namespace Osiris.Src.Denature;

public abstract class DeNode
{
    public readonly string Id;
    public readonly string Tag;
    public string Path{get; private set;}
    public readonly int Depth = 1;
    private DeDom? Dom;
    private DeNode? Parent;
    private bool ThemeRecalculationQueued = true;
    private HashSet<string> Clades = [];
    private DeThemeSheet? ThemeSheet;
    private DeThemeDirtyFlag ParentTheme = new(new());
    protected DeThemeDirtyFlag ThemeOverrides = new();
    private DeTheme Theme = new();
    private JsonObject State = [];
    private readonly List<DeNode> Children = [];
    private readonly Dictionary<string, DeNode> FreedChildren = [];
    private JsonObject Props = [];
    public DeNode(string id, JsonObject props, string tag = "div")
    {
        Id = id;
        Props = props;
        Tag = tag;
        Path = Id;;
    }
    private void SetDom(DeDom dom){Dom = dom;}
    private void SetParent(DeNode parent)
    {
        Parent = parent;
        Path = parent.Path + Path;
        ParentTheme = new(parent.Theme);
        if(parent.Dom is not null) SetDom(parent.Dom);
    }
    private void SetProps(JsonObject props){Props = props;}
    protected void SetThemeSheet(DeThemeSheet themeSheet)
    {
        ThemeSheet = themeSheet;
        ThemeRecalculationQueued = true;
    }
    protected void AddClade(string cladeName)
    {
        if(Clades.Contains(cladeName)) return;
        Clades.Add(cladeName);
        ThemeRecalculationQueued = true;
    }
    protected bool RemoveClade(string cladeName)
    {
        if(!Clades.Remove(cladeName)) return false;
        ThemeRecalculationQueued = true;
        return true;
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
    public void RenderInternal()
    {
        if(Dom is null) throw new Exception("No dom set");
        foreach (var child in Children)
        {
            FreedChildren.Add(child.Id, child);
        }
        Children.Clear();
        foreach (var child in Render(Props))
        {
            if(FreedChildren.TryGetValue(child.Id, out var oldChild) && child.GetType() == oldChild.GetType())
            {
                FreedChildren.Remove(child.Id);
                oldChild.SetProps(child.Props);
                Children.Add(oldChild);
                oldChild.Render(oldChild.Props);
            }
            else
            {
                child.OnMountInternal();
                child.SetParent(this);
                Children.Add(child);
                child.Render(child.Props);
            }
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
    public DeNode? GetParent(){return Parent;}
    public JsonObject GetState(){return State;}
    public void SetState(JsonObject state)
    {
        State = state;
        Dom?.MarkDirty(this);
    }
    // get theme
    public DeTheme GetTheme()
    {
        bool themeIsDirty = ParentTheme.IsDirty() || ThemeOverrides.IsDirty();
        if(ParentTheme.IsDirty()){Theme = RojaDict.DeepCopy(ParentTheme.GetTheme());}
        if (ThemeRecalculationQueued)
        {
            ThemeRecalculationQueued = false;
            Stack<DeThemeSheet> themeSheets = new();
            DeNode? node = this;
            while(node is not null)
            {
                if(node.ThemeSheet is not null) themeSheets.Push(node.ThemeSheet);
                node = node.Parent;
            }
            if(Dom is null) throw new Exception("No dom set");
            while(themeSheets.TryPop(out var sheet))
            {
                if(!sheet.TryMergeThemes(ref Theme, this, Dom.GetEnv())) throw new Exception("Failed to merge theme sheets");
            }
        }
        if (themeIsDirty)
        {
            // recalculate just our theme
            if(!RojaDict.TryMerge(ref Theme, ThemeOverrides.GetTheme())) throw new Exception("Failed to merge theme overrides");
        }
        return Theme;
    }
    public bool HasClade(string cladeName)
    {
        return Clades.Contains(cladeName);
    }
    public IEnumerable<string> GetClades()
    {
        foreach (var clade in Clades)
        {
            yield return clade;
        }
    }
}
