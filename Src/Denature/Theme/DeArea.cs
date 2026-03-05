using System.Text.Json.Nodes;
using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Theme;

public class DeArea : RojaDict, IRojaSerializerJson<DeArea>
{
    public DeLength? Top{get; private set;}
    public DeLength? Bottom{get; private set;}
    public DeLength? Left{get; private set;}
    public DeLength? Right{get; private set;}
    public DeArea(){}
    public DeArea(DeLength? all)
    {
        Top = all;
        Bottom = all;
        Left = all;
        Right = all;
    }
    public DeArea(DeLength? topAndBottom, DeLength? leftAndRight)
    {
        Top = topAndBottom;
        Bottom = topAndBottom;
        Left = leftAndRight;
        Right = leftAndRight;
    }
    public DeArea(DeLength? top, DeLength? bottom, DeLength? left, DeLength? right)
    {
        Top = top; Bottom = bottom; Left = left; Right = right;
    }

    public override JsonNode? GetField(string fieldName)
    {
        return fieldName switch
        {
            "top" => Top?.ToJson(),
            "bottom" => Bottom?.ToJson(),
            "left" => Left?.ToJson(),
            "right" => Right?.ToJson(),
            _ => null,
        };
    }
    public override bool TrySetFieldInternal(string fieldName, JsonNode? jsonNode)
    {
        switch (fieldName)
        {
            case "top":
                Top = DeLength.FromJson(jsonNode);
                return true;
            case "bottom":
                Bottom = DeLength.FromJson(jsonNode);
                return true;
            case "left":
                Left = DeLength.FromJson(jsonNode);
                return true;
            case "right":
                Right = DeLength.FromJson(jsonNode);
                return true;
            default:
                return false;
        }
    }
    public static DeArea? FromJson(JsonNode? jsonNode)
    {
        if(!RojaUtils.TryAsObject(jsonNode, out var jsonObject)) return null;
        var top = DeLength.FromJson(jsonObject["top"]);
        var bottom = DeLength.FromJson(jsonObject["bottom"]);
        var left = DeLength.FromJson(jsonObject["left"]);
        var right = DeLength.FromJson(jsonObject["right"]);
        return new(top, bottom, left, right);
    }

    public JsonNode ToJson()
    {
        JsonObject obj = [];
        obj["top"] = Top?.ToJson();
        obj["bottom"] = Bottom?.ToJson();
        obj["bight"] = Right?.ToJson();
        obj["left"] = Left?.ToJson();
        return obj;
    }
}
