// Todo: remove this
using System;
using System.Text.Json.Nodes;

namespace Osiris.Src.Roja;

public abstract class RojaDict
{
    public int WriteCount{get; private set;} = 0;
    public abstract JsonNode? GetField(string fieldName);
    public virtual T? GetField<T>(string fieldName) where T : IRojaSerializerJson<T>
    {
        return T.FromJson(GetField(fieldName));
    }
    public bool TrySetField(string fieldName, JsonNode? jsonNode)
    {
        return TrySetFieldInternal(fieldName, jsonNode);
    }
    protected abstract bool TrySetFieldInternal(string fieldName, JsonNode? jsonNode);
    public bool TrySetField<T>(string fieldName, T value) where T : IRojaSerializerJson<T>
    {
        WriteCount++;
        return TrySetField(fieldName, value.ToJson());
    }
    private static (JsonObject, int) GetData<T>(T dict) where T : RojaDict, IRojaSerializerJson<T>, new()
    {
        if(!RojaUtils.TryAsObject(dict.ToJson(), out var backup)) throw new Exception("Should be unreachable");
        return (backup, dict.WriteCount);
    }
    public static bool TryFromJsonObject<T>(JsonObject jsonObject, out T res) where T : RojaDict, IRojaSerializerJson<T>, new()
    {
        res = new();
        return TryMerge(ref res, jsonObject);
    }
    public static T DeepCopy<T>(T dict) where T : RojaDict, IRojaSerializerJson<T>, new()
    {
        T res = new()
        {
            WriteCount = dict.WriteCount
        };
        var (data,_) = GetData(dict);
        if(!TryMerge(ref res, data)) throw new Exception("Should be unreachable");
        return res;
    }
    public static bool TryMerge<T>(ref T dict, T merged) where T : RojaDict, IRojaSerializerJson<T>, new()
    {
        var (data,_) = GetData(merged);
        return TryMerge(ref dict, data);
    }
    public static bool TryMerge<T>(ref T dict, JsonObject data) where T : RojaDict, IRojaSerializerJson<T>, new()
    {
        var (backup, writeCount) = GetData(dict);
        bool dictDirty = false;
        foreach (var (key, value) in data)
        {
            if(!dict.TrySetField(key, value))
            {
                if (dictDirty)
                {
                    if(!TryFromJsonObject(backup, out dict)) throw new Exception("Should be unreachable");
                    dict.WriteCount = writeCount;
                    return false;
                }
                return false;
            }
            dictDirty = true;
        }
        // A merge only counts as a single "write"
        dict.WriteCount = writeCount + 1;
        return true;
    }
}
