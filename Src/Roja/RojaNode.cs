using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Osiris.Src.Roja;

public class RojaNode
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
    public RojaNode? Parent;
    public int Count{get {
            if(TryAsJsonArray(out var array)) return array?.Count ?? 0;
            else if(TryAsJsonObject(out var obj)) return obj?.Count ?? 0;
            return 0;
        }
    }
    public bool LockWriteCount = false;
    private ulong WriteCount_ = 0;
    public ulong WriteCount
    {
        get => WriteCount_; 
        // private set
        // {
        //     if (!LockWriteCount)
        //     {
        //         WriteCount_ = value;
        //     }
        // }
    }
    public void IncWriteCount()
    {
        if(LockWriteCount) return;
        WriteCount_++;
        Parent?.IncWriteCount();
    }
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
        if(jsonNode is null 
            || jsonNode.GetValueKind() == JsonValueKind.Null 
            || jsonNode.GetValueKind() == JsonValueKind.Undefined) return (null, ValueKind.Null);
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
       IncWriteCount();
    }
    public void SetData(RojaNode? rojaNode)
    {
       IncWriteCount();
        if(rojaNode is null){Value = null; Kind = ValueKind.Null;}
        else{Value = rojaNode.Value; Kind = rojaNode.Kind;}
    }
    public bool SetField(string? key, RojaNode? value)
    {
       IncWriteCount();
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
    public bool TryGetField(string key, out RojaNode rojaNode)
    {
        rojaNode = GetField(key)!;
        return rojaNode is not null;
    }
    public bool TryGetField(int key, out RojaNode rojaNode)
    {
        rojaNode = GetField(key)!;
        return rojaNode is not null;
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
    public IEnumerable<KeyValuePair<string,RojaNode>> GetEntries()
    {
        if(TryAsJsonObject(out var obj))
        {
            foreach (var (key,value) in obj)
            {
                yield return new(key, new(value));
            }
        }
        if(TryAsJsonArray(out var arr))
        {
            int key = 0;
            foreach (var value in arr)
            {
                yield return new(key.ToString(), new(value));
                key++;
            }
        }
    }
    public IEnumerable<RojaNode> GetValues()
    {
        if(TryAsJsonObject(out var obj))
        {
            foreach (var (_,value) in obj)
            {
                yield return new(value);
            }
        }
        if(TryAsJsonArray(out var arr))
        {
            foreach (var value in arr)
            {
                yield return new(value);
            }
        }
    }
    public bool SetPath(string path, RojaNode? rojaNode)
    {
        // Todo: make more efficient and robust
        IncWriteCount();
        var p = path.Split('/');
        Stack<string> stackPath = new();
        foreach (var seg in p)
        {
            stackPath.Push(seg);
        }
        return SetPath(stackPath, rojaNode);
    }
    private bool SetPath(Stack<string> path, RojaNode? rojaNode)
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
    public bool TryAsString(out string value)
    {
        return TryAsString(this, out value);
    }
    public static bool TryAsString(RojaNode? rojaNode, out string value)
    {
        if(rojaNode?.Value is JsonNode jsonNode && jsonNode.AsValue().TryGetValue(out string? str))
        {
            value = str ?? string.Empty;
            return true;
        }
        value = string.Empty;
        return false;
    }
    public static RojaNode FromString(string value)
    {
        return new(JsonValue.Create(value), ValueKind.String);
    }
    public static implicit operator RojaNode(string value) => FromString(value);
    public static bool TryAsBool(RojaNode? rojaNode, out bool value)
    {
        if(rojaNode?.Value is JsonNode jsonNode && jsonNode.AsValue().TryGetValue(out value)) return true;
        value = default;
        return false;
    }
    public bool TryAsBool(out bool value)
    {
        return TryAsBool(this, out value);
    }
    public static RojaNode FromBool(bool value)
    {
        return new(JsonValue.Create(value), ValueKind.Bool);
    }
    public static implicit operator RojaNode(bool value) => FromBool(value);
    public static bool TryAsDouble(RojaNode? rojaNode, out double value)
    {
        if(rojaNode?.Value is JsonNode jsonNode && jsonNode.AsValue().TryGetValue(out value)) return true;
        value = default;
        return false;
    }
    public bool TryAsDouble(out double value)
    {
        return TryAsDouble(this, out value);
    }
    public static RojaNode FromDouble(double value)
    {
        return new(JsonValue.Create(value), ValueKind.Number);
    }
    public static implicit operator RojaNode(double value) => FromDouble(value);
    public static bool TryAsFloat(RojaNode? rojaNode, out float value)
    {
        if(rojaNode?.Value is JsonNode jsonNode && jsonNode.AsValue().TryGetValue(out value)) return true;
        value = default;
        return false;
    }
    public bool TryAsFloat(out float value)
    {
        return TryAsFloat(this, out value);
    }
    public static RojaNode FromFloat(float value)
    {
        return new(JsonValue.Create(value), ValueKind.Number);
    }
    public static implicit operator RojaNode(float value) => FromFloat(value);
    public static bool TryAsInt(RojaNode? rojaNode, out int value)
    {
        if(rojaNode?.Value is JsonNode jsonNode && jsonNode.AsValue().TryGetValue(out float f))
        {
            value = (int)f;
            return true;
        }
        value = default;
        return false;
    }
    public bool TryAsInt(out int value)
    {
        return TryAsInt(this, out value);
    }
    public static RojaNode FromInt(int value)
    {
        // Todo: make sure this actually works
        return new(JsonValue.Create(value), ValueKind.Number);
    }
    public static implicit operator RojaNode(int value) => FromInt(value);
    public static bool TryAsEnum<T>(RojaNode? rojaNode, out T value) where T : struct
    {
        value = default;
        if(!TryAsString(rojaNode, out string str)) return false;
        return Enum.TryParse(str, true, out value);
    }
    public bool TryAsEnum<T>(out T value) where T : struct
    {
        return TryAsEnum(this, out value);
    }
    public static RojaNode? FromEnum<T>(T value) where T : struct
    {
        // Todo: handle name formatting better
        return new(JsonValue.Create(value.ToString()), ValueKind.Number);
    }
    public static bool TryAsJsonObject(RojaNode? rojaNode, out JsonObject value)
    {
        value = []; 
        if(rojaNode is null) return false;
        if(rojaNode.Kind != ValueKind.Dict) return false;
        var val = rojaNode?.Value?.AsObject();
        if(val is null) return false;
        value = val;
        return true;
    }
    public bool TryAsJsonObject(out JsonObject value)
    {
        return TryAsJsonObject(this, out value);
    }
    public static bool TryAsJsonArray(RojaNode? rojaNode, out JsonArray value)
    {
        value = []; 
        if(rojaNode is null) return false;
        if(rojaNode.Kind != ValueKind.Dict) return false;
        var val = rojaNode?.Value?.AsArray();
        if(val is null) return false;
        value = val;
        return true;
    }
    public bool TryAsJsonArray(out JsonArray value)
    {
        return TryAsJsonArray(this, out value);
    }
    public JsonNode? ToJson()
    {
        return Value;
    }

    public static RojaNode FromJson(JsonNode? jsonNode)
    {
        return new(jsonNode);
    }

    public static RojaNode NewDict()
    {
        return new(new JsonObject());
    }
    public static RojaNode NewDict(IEnumerable<KeyValuePair<string, RojaNode>> entries)
    {
        RojaNode res = new(new JsonObject())
        {
            LockWriteCount = true
        };
        foreach (var (key, value) in entries)
        {
            res.SetField(key, value);
        }
        res.LockWriteCount = false;
        // Todo: write count should be 0 at initialization right?
        res.WriteCount_ = 0;
        return res;
    }
    public static RojaNode NewArray()
    {
        return new(new JsonObject());
    }
    public static RojaNode NewArray(IEnumerable<RojaNode> values)
    {
        RojaNode res = new(new JsonObject())
        {
            LockWriteCount = true
        };
        int idx = 0;
        foreach (var value in values)
        {
            res.SetField(idx, value);
            idx++;
        }
        res.LockWriteCount = false;
        return res;
    }
    public RojaNode DeepCopy()
    {
        return new(Value?.DeepClone());
    }

    public static bool TryFromJson(JsonNode? jsonNode, out RojaNode rojaNode)
    {
        var (value, kind) = FromJsonInternal(jsonNode);
        rojaNode = new(value, kind);
        return true;
    }
}
