using System;
using System.Collections.Generic;
using Osiris.Src.Denature.Node;

namespace Osiris.Src.Denature;

public class DeDom
{
    private readonly DeNode Root;
    private readonly Dictionary<string, (Func<string>,HashSet<DeNode>)> Callbacks = [];
    private readonly HashSet<Guid> DirtyNodeIds = [];
    private readonly HashSet<DeNode> FreedNodes = [];
    private readonly List<DeNode> DirtyNodes = [];
    private bool IsRendering_ = false;
    public DeDom(DeNode root)
    {
        Root = root;
        Root.InitRoot(this);
        MarkDirty(Root);
        Update();
    }
    public bool IsRendering(){return IsRendering_;}
    public void AddCallback(string callbackName, Func<string> callback)
    {
        Callbacks.Add(callbackName, (callback, []));
    }
    public void RemoveCallback(string callbackName)
    {
        Callbacks.Remove(callbackName);
    }
    public bool TryAddCallbackListener(string callbackName, DeNode node)
    {
        if(!Callbacks.TryGetValue(callbackName, out var value)) return false;
        value.Item2.Add(node);
        return true;
    }
    public bool TryRemoveCallbackListener(string callbackName, DeNode node)
    {
        if(!Callbacks.TryGetValue(callbackName, out var value)) return false;
        value.Item2.Remove(node);
        return true;
    }
    public void MarkDirty(DeNode node)
    {
        if(DirtyNodeIds.Contains(node.Uuid)) return;
        DirtyNodes.Add(node);
        DirtyNodeIds.Add(node.Uuid);
    }
    public void MarkClean(DeNode node)
    {
        DirtyNodeIds.Remove(node.Uuid);
    }
    public void MarkFreed(DeNode node)
    {
        FreedNodes.Add(node);
    }
    public void Update()
    {
        if(DirtyNodes.Count == 0) return;
        IsRendering_ = true;
        // Todo: check whether this sorts the right way
        DirtyNodes.Sort((a,b)=>a.Depth-b.Depth);
        // For each dirty node, check if it is still in the dirty ids set
        foreach (var node in DirtyNodes)
        {
            if(!DirtyNodeIds.Contains(node.Uuid)) continue;
            if(FreedNodes.Contains(node)) continue;
            // Call render on the node. This will recursively call render on its children and clean itself.
            // Todo: nodes cannot set state while rendering. Callbacks are deferred. Not 100% foolproof
            node.RenderInternal();
        }
        DirtyNodes.Clear();
        DirtyNodeIds.Clear();
        // Should let them be reclaimed by the gc. Unless I have a memory leak...
        FreedNodes.Clear();
        IsRendering_ = false;
    }
    public DeEnv GetEnv(){return new(640, 480);}
}
