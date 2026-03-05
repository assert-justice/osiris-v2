using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace Osiris.Src.Denature.Node;

public abstract partial class DeNode
{
    private readonly Dictionary<string, DeNode> FreedChildren = [];
    private bool IsFirstRender = true;
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
        if(IsFirstRender)
        {
            OnMountInternal();
            IsFirstRender = false;
        }
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
            }
            else
            {
                child.SetParent(this);
                Children.Add(child);
            }
        }
        // Free the excess children
        foreach (var freedChild in FreedChildren.Values)
        {
            freedChild.OnUnmountInternal();
            Dom.MarkFreed(freedChild);
        }
        foreach (var child in Children)
        {
            child.RenderInternal();
        }
        UpdateTransform();
        Dom.MarkClean(this);
    }
    protected virtual IEnumerable<DeNode> Render(JsonObject props){yield break;}
    protected JsonObject GetState(){return State;}
    protected void SetState(JsonObject state)
    {
        State = state;
        Dom?.MarkDirty(this);
    }
}
