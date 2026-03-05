using System;
using System.Text.Json.Nodes;
using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Theme;

public class DeSize : RojaDict, IRojaSerializerJson<DeSize>
{
    public DeLength? Width{get; private set;}
    public DeLength? Height{get; private set;}
    public DeSize(DeLength? width = null, DeLength? height = null){Width = width; Height = height;}

    public override JsonNode? GetField(string fieldName)
    {
        return fieldName switch
        {
            "width" => Width?.ToJson(),
            "height" => Height?.ToJson(),
            _ => null,
        };
    }
    protected override bool TrySetFieldInternal(string fieldName, JsonNode? jsonNode)
    {
        switch (fieldName)
        {
            case "width":
                Width = DeLength.FromJson(jsonNode);
                return true;
            case "height":
                Height = DeLength.FromJson(jsonNode);
                return true;
            default:
                return false;
        }
    }

    public JsonNode ToJson()
    {
        JsonObject obj = [];
        obj["width"] = Width?.ToJson();
        obj["height"] = Height?.ToJson();
        return obj;
    }

    public static DeSize? FromJson(JsonNode? jsonNode)
    {
        if(!RojaUtils.TryAsObject(jsonNode, out var jsonObject)) return null;
        var width = DeLength.FromJson(jsonObject["width"]);
        var height = DeLength.FromJson(jsonObject["height"]);
        return new(width, height);
    }
    public DeMath.Vec2 GetSizePx()
    {
        float w = DeLength.GetLengthPx(Width);
        float h = DeLength.GetLengthPx(Height);
        return new(w,h);
    }
    public static DeMath.Vec2 GetSizePx(DeSize? size)
    {
        return size?.GetSizePx() ?? new();
    }
}
