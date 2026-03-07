using System;
using System.Collections.Generic;
using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Style;

public abstract class DeStyleNode
{
    protected readonly RojaNode Data;
    public DeStyleNode(RojaNode? rojaNode)
    {
        Data = rojaNode ?? new();
    }
    public bool HasProperty(string propertyName)
    {
        return Data.GetField(propertyName) is not null;
    }
    public abstract RojaNode ToRoja();
    // private static readonly Dictionary<Type, HashSet<DeStyleProperty>> PropertyGroupLookup = [];
    // private readonly HashSet<DeStyleProperty> PropertyGroup;
    // public DeStyleNode(RojaNode rojaNode)
    // {
    //     var type = GetType();
    //     if(!PropertyGroupLookup.TryGetValue(type, out var group))
    //     {
    //         group = BindPropertyGroup();
    //         PropertyGroupLookup.Add(type, group);
    //     }
    //     PropertyGroup = group;
    // }
    // protected abstract HashSet<DeStyleProperty> BindPropertyGroup();
    // private static T FromRoja<T>(T styleNode, RojaNode rojaNode) where T : DeStyleNode
    // {
    //     // Make GetEntries return key value pairs
    //     Dictionary<string, RojaNode> excessProperties = [..rojaNode.GetEntries()];
    //     // For each property in property group
    //     // If its required and not in excess properties throw exception
    //     // Otherwise if its present call the 

    //     // You know what? Maybe just use reflection. Fuck it.
    // }
    // public virtual RojaNode ToRoja(){}
}
