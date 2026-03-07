using System;
using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Style;

public class DeStyleProperty<T> where T : IRojaSerializable<T>
{
    public readonly string Name;
    private T Value_;
    public T Value
    {
        get => GetValue();
        set
        {
            Value_ = value;
            Data.SetData(value.ToRoja());
        }
    }
    private T GetValue()
    {
        if(!Data.IsDirty()) return Value_;
        if(T.TryToValue(Data.GetData(), out var val))Value_ = val;
        else Value_ = GetDefault();
        return Value_;
    }
    private readonly RojaNodeDirtyFlag Data;
    Func<T> GetDefault;
    public DeStyleProperty(string name, 
        RojaNode? parent,
        Func<T> getDefault)
    {
        Name = name;
        GetDefault = getDefault;
        if(parent is null)
        {
            Data = new();
            Value_ = GetDefault();
        }
        else if(parent.GetField(name) is RojaNode rojaNode)
        {
            Data = new(rojaNode);
            T.TryToValue(rojaNode, out Value_);
        }
        else
        {
            Data = new();
            parent.SetField(name, Data.GetData());
            Value_ = GetDefault();
        }
    }
}

// public class DeStyleProperty(string name, Action<DeStyleNode, RojaNode> setter, Func<DeStyleNode, RojaNode> getter, bool isOptional = true)
// {
//     public readonly string Name = name;
//     public readonly Action<DeStyleNode, RojaNode> Setter = setter;
//     public readonly Func<DeStyleNode, RojaNode> Getter = getter;
//     public readonly bool IsOptional = isOptional;
// }
