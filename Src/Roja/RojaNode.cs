using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Osiris.Src.Roja;

public class RojaNode : IRojaSerializerJson<RojaNode>
{
    public enum ValueKind
    {
        Null,
        Array,
        Bool,
        Dict,
        Number,
        String,
    }
    public ValueKind Kind{get; private set;} = ValueKind.Null;
    public JsonNode? Value{get; private set;}
    public RojaNode(){}
    public RojaNode(JsonNode? jsonNode)
    {
        (Value, Kind) = FromJsonInternal(jsonNode);
    }
    private RojaNode(JsonNode? value, ValueKind kind)
    {
        Value = value;
        Kind = kind;
    }
    private static (JsonNode?, ValueKind) FromJsonInternal(JsonNode? jsonNode)
    {
        if(RojaUtils.IsNullOrUndefined(jsonNode)) return (null, ValueKind.Null);
        JsonNode? value = jsonNode;
        ValueKind kind = jsonNode?.GetValueKind() switch
        {
            JsonValueKind.Object => ValueKind.Dict,
            JsonValueKind.Array => ValueKind.Array,
            JsonValueKind.String => ValueKind.String,
            JsonValueKind.Number => ValueKind.Number,
            JsonValueKind.True or JsonValueKind.False => ValueKind.Bool,
            _ => ValueKind.Null
        };
        return (value, kind);
    }
    public void SetData(JsonNode? jsonNode)
    {
        (Value, Kind) = FromJsonInternal(jsonNode);
    }
    public bool SetField(string? key, RojaNode? value)
    {
        if(Value is null || key is null) return false;
        if(Kind == ValueKind.Array)
        {
            Value.AsArray()[key] = value?.ToJson();
            return true;
        }
        else if(Kind == ValueKind.Dict)
        {
            Value.AsObject()[key] = value?.ToJson();
            return true;
        }
        else return false;
    }
    public bool SetField(int key, RojaNode? value)
    {
        return SetField(key.ToString(), value);
    }
    public RojaNode? GetField(string key)
    {
        if(Value is null) return null;
        if(Kind == ValueKind.Array)
        {
            return new RojaNode(Value.AsArray()[key]);
        }
        else if(Kind == ValueKind.Dict)
        {
            return new RojaNode(Value.AsObject()[key]);
        }
        else return null;
    }
    public RojaNode? GetField(int key)
    {
        return GetField(key.ToString());
    }
    public bool SetPath(string path, RojaNode? rojaNode)
    {
        // Todo: make more efficient and robust
        var p = path.Split('/');
        Stack<string> stackPath = new();
        foreach (var seg in p)
        {
            stackPath.Push(seg);
        }
        return SetPath(stackPath, rojaNode);
    }
    public bool SetPath(Stack<string> path, RojaNode? rojaNode)
    {
        if(!path.TryPop(out string? seg))
        {
            SetData(rojaNode?.ToJson());
            return true;
        }
        var field = GetField(seg);
        if(field is null || field.Kind == ValueKind.Null)
        {
            // Todo: be smarter about whether we should create a dict or array
            field = new(new JsonObject(), ValueKind.Dict);
            SetField(seg, field);
        }
        return field.SetPath(path, rojaNode);
    }
    public RojaNode? GetPath(string path)
    {
        // Todo: make more efficient and robust
        var p = path.Split('/');
        Stack<string> stackPath = new();
        foreach (var seg in p)
        {
            stackPath.Push(seg);
        }
        return GetPath(stackPath);
    }
    public RojaNode? GetPath(Stack<string> path)
    {
        if(!path.TryPop(out string? seg))
        {
            return this;
        }
        var field = GetField(seg);
        if(field is null || field.Kind == ValueKind.Null) return null;
        return field.GetPath(path);
    }
    public T? As<T>() where T : IRojaSerializerJson<T>
    {
        return T.FromJson(Value);
    }
    public bool TryGetString(out string? value)
    {
        if(Value is not null && Value.AsValue().TryGetValue(out value)) return true;
        value = default;
        return false;
    }
    public static RojaNode? FromString(string value)
    {
        return new(JsonValue.Create(value), ValueKind.String);
    }
    public bool TryGetBool(out bool value)
    {
        if(Value is not null && Value.AsValue().TryGetValue(out value)) return true;
        value = default;
        return false;
    }
    public static RojaNode? FromBool(bool value)
    {
        return new(JsonValue.Create(value), ValueKind.Bool);
    }
    public bool TryGeDouble(out double value)
    {
        if(Value is not null && Value.AsValue().TryGetValue(out value)) return true;
        value = default;
        return false;
    }
    public static RojaNode? FromDouble(double value)
    {
        return new(JsonValue.Create(value), ValueKind.Number);
    }
    public bool TryGeFloat(out float value)
    {
        if(Value is not null && Value.AsValue().TryGetValue(out value)) return true;
        value = default;
        return false;
    }
    public static RojaNode? FromFloat(float value)
    {
        return new(JsonValue.Create(value), ValueKind.Number);
    }
    public bool TryGeInt(out int value)
    {
        if(TryGeFloat(out float val))
        {
            value = (int)val;
            return true;
        }
        value = default;
        return false;
    }
    public static RojaNode? FromInt(int value)
    {
        // Todo: make sure this actually works
        return new(JsonValue.Create(value), ValueKind.Number);
    }
    public bool TryGetEnum<T>(out T value) where T : struct
    {
        value = default;
        if(!TryGetString(out string? str)) return false;
        return Enum.TryParse(str, true, out value);
    }
    public static RojaNode? FromEnum<T>(T value) where T : struct
    {
        // Todo: handle name formatting better
        return new(JsonValue.Create(value.ToString()), ValueKind.Number);
    }
    public bool TryGetJsonObject(out JsonObject? value)
    {
        if(Kind == ValueKind.Dict)
        {
            value = Value?.AsObject();
            return true;
        }
        value = default;
        return false;
    }
    public static RojaNode? FromJsonObject(JsonObject? value)
    {
        if(value is null) return null;
        return new(value, ValueKind.Dict);
    }
    public bool TryGetJsonArray(out JsonArray? value)
    {
        if(Kind == ValueKind.Array)
        {
            value = Value?.AsArray();
            return true;
        }
        value = default;
        return false;
    }
    public static RojaNode? FromJsonArray(JsonArray? value)
    {
        if(value is null) return null;
        return new(value, ValueKind.Array);
    }
    public JsonNode? ToJson()
    {
        return Value;
    }
    public static RojaNode? FromJson(JsonNode? jsonNode)
    {
        var (value, kind) = FromJsonInternal(jsonNode);
        return new(value, kind);
    }
}
