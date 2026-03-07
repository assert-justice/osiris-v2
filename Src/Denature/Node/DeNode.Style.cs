using System;
using System.Collections.Generic;
using Osiris.Src.Denature.Style;
using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Node;

public abstract partial class DeNode
{
    private bool StyleRecalculationQueued = true;
    private readonly HashSet<string> Clades = [];
    private DeStyleSheet? StyleSheet;
    private readonly RojaNodeDirtyFlag ParentStyle = new(new());
    private readonly RojaNodeDirtyFlag BaseStyle = new();
    protected readonly RojaNodeDirtyFlag StyleOverrides = new();
    protected void SetStyleSheet(DeStyleSheet styleSheet)
    {
        StyleSheet = styleSheet;
        StyleRecalculationQueued = true;
    }
    protected void AddClade(string cladeName)
    {
        if(Clades.Contains(cladeName)) return;
        Clades.Add(cladeName);
        StyleRecalculationQueued = true;
    }
    protected bool RemoveClade(string cladeName)
    {
        if(!Clades.Remove(cladeName)) return false;
        StyleRecalculationQueued = true;
        return true;
    }
    public DeStyle GetStyle()
    {
        return new();
        // if (StyleRecalculationQueued)
        // {
        //     StyleRecalculationQueued = false;
        //     Stack<DeStyleSheet> StyleSheets = new();
        //     DeNode? node = this;
        //     while(node is not null)
        //     {
        //         if(node.StyleSheet is not null) StyleSheets.Push(node.StyleSheet);
        //         node = node.Parent;
        //     }
        //     if(Dom is null) throw new Exception("No dom set");
        //     var styleData = BaseStyle.GetData();
        //     while(StyleSheets.TryPop(out var sheet))
        //     {
        //         sheet.Merge(ref styleData, this, Dom.GetEnv());
        //     }
        // }
        // bool StyleIsDirty = ParentStyle.IsDirty() || StyleOverrides.IsDirty();
        // if(ParentStyle.IsDirty())
        // {
        //     // var copy = ParentStyle.GetData().DeepCopy();
        //     // ParentStyle.GetData().SetData(copy);
        //     // Style = ParentStyle.GetData().DeepCopy(ParentStyle.GetData());
        // }
        // if (StyleIsDirty)
        // {
        //     // recalculate just our Style
        //     // if(!RojaDict.TryMerge(ref Style, StyleOverrides.GetStyle())) throw new Exception("Failed to merge Style overrides");
        // }
        // return BaseStyle;
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
