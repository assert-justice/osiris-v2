using System;
using System.Text.Json.Nodes;

namespace Osiris.Src.Roja;

public static class RojaUtils
{
    public static bool TryAsObject(JsonNode? jsonNode, out JsonObject jsonObject)
    {
        jsonObject = default!;
        if(jsonNode is null || jsonNode.GetValueKind() != System.Text.Json.JsonValueKind.Object) return false;
        jsonObject = jsonNode.AsObject();
        return true;
    }
    public static bool TryAsNumber(JsonNode? jsonNode, out float value)
    {
        value = default;
        if(jsonNode is null || jsonNode.GetValueKind() != System.Text.Json.JsonValueKind.Number) return false;
        return jsonNode.AsValue().TryGetValue(out value);
    }
    public static bool TryAsNumber(JsonNode? jsonNode, out int value)
    {
        value = default;
        if(jsonNode is null || jsonNode.GetValueKind() != System.Text.Json.JsonValueKind.Number) return false;
        // Todo: investigate this
        return int.TryParse(jsonNode.ToString(), out value);
    }
    public static bool TryAsString(JsonNode? jsonNode, out string value)
    {
        value = string.Empty;
        if(jsonNode is null || jsonNode.GetValueKind() != System.Text.Json.JsonValueKind.String) return false;
        if(!jsonNode.AsValue().TryGetValue(out string? val)) return false;
        if(val is null) return false;
        value = val;
        return true;
    }
    public static bool TryAsEnum<T>(JsonNode? jsonNode, out T value, bool ignoreCase = true) where T : struct
    {
        value = default!;
        if(!TryAsString(jsonNode, out string str)) return false;
        return Enum.TryParse<T>(str, ignoreCase, out value);
    }
}
