using System;
using System.Collections.Generic;
using Osiris.Src.Denature.Theme;
using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Node;

public abstract partial class DeNode
{
    private bool ThemeRecalculationQueued = true;
    private readonly HashSet<string> Clades = [];
    private DeThemeSheet? ThemeSheet;
    private DeThemeDirtyFlag ParentTheme = new(new());
    protected DeThemeDirtyFlag ThemeOverrides = new();
    private DeTheme Theme = new();
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
