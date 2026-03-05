using System;
using System.Text.Json.Nodes;
using Godot;
using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Theme;

public class DeLength : IRojaSerializerJson<DeLength>, IRojaSerializerString<DeLength>
{
    public enum DeKind
    {
        // Absolute length in pixels
        Px,
        Percent,
        Cap,
        Ch,
        Em,
        Ex,
        Lh,
        Rcap,
        Rch,
        Rem,
        Rex,
        Rlh,
        Vh,
        Vw,
        Vmax,
        Vmin,
        Flex,
    }
    public readonly float Value = 0;
    public readonly DeKind Kind = DeKind.Px;
    public DeLength(float value, DeKind kind)
    {
        Value = value;
        Kind = kind;
    }
    public static DeLength? FromJson(JsonNode? jsonNode)
    {
        if(!RojaUtils.TryAsObject(jsonNode, out var jsonObject)) return null;
        if(!RojaUtils.TryAsNumber(jsonObject["width"], out float value)) return null;
        if(!RojaUtils.TryAsString(jsonObject["kind"], out string kindStr)) return null;
        if(kindStr.Trim() == "%") kindStr = "Percent";
        if(!Enum.TryParse<DeKind>(kindStr, true, out var kind)) return null;
        return new(value, kind);
    }
    public JsonNode ToJson()
    {
        JsonObject obj = [];
        obj["value"] = Value;
        var kind = Kind.ToString();
        if(kind == "Percent") kind = "%";
        obj["kind"] = kind;
        return obj;
    }
    public static bool TryFromString(string serialized, out DeLength res)
    {
        res = default!;
        // Todo: optimize this with spans
        var args = serialized.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if(args.Length != 2) return false;
        if(!float.TryParse(args[0], out float value)) return false;
        string kindStr = args[1];
        if(kindStr == "%") kindStr = "Percent";
        if(!Enum.TryParse<DeKind>(kindStr, true, out var kind)) return false;
        res = new(value, kind);
        return true;
    }
    public new string ToString()
    {
        return $"{Value} {Kind}";
    }
}
