using System;
using System.Text.Json.Nodes;
using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Theme;

public class DeTheme : RojaDict, IRojaSerializerJson<DeTheme>
{
    public enum DeFlow
    {
        None,
        Column,
        Row,
        Grid,
    }
    public enum DeJustify
    {
        Start,
        Center,
        End,
        Stretch,
    }
    public DeArea? Margins{get; private set;}
    public DeArea? Padding{get; private set;}
    public DeSize? Size{get; private set;}
    public DeSize? MinSize{get; private set;}
    public DeSize? MaxSize{get; private set;}
    public DeFlow? Flow{get; private set;}
    public int? Wrap{get; private set;}
    public DeJustify? Justify{get; private set;}
    public DeTheme(){}
    public DeTheme(
        DeArea? margins = null, 
        DeArea? padding = null, 
        DeSize? size = null, 
        DeSize? minSize = null,
        DeSize? maxSize = null,
        DeFlow? flowDirection = null,
        int? wrap = null,
        DeJustify? justify = null)
    {
        Margins = margins;
        Padding = padding;
        Size = size;
        MinSize = minSize;
        MaxSize = maxSize;
        Flow = flowDirection;
        Wrap = wrap;
        Justify = justify;
    }
    public override JsonNode? GetField(string fieldName)
    {
        return fieldName switch
        {
            "margins" => Margins?.ToJson(),
            "padding" => Padding?.ToJson(),
            "size" => Size?.ToJson(),
            "min_size" => MinSize?.ToJson(),
            "max_size" => MaxSize?.ToJson(),
            "flow" => Flow?.ToString(),
            "wrap" => Wrap,
            "justify" => Justify?.ToString(),
            _ => null,
        };
    }
    protected override bool TrySetFieldInternal(string fieldName, JsonNode? jsonNode)
    {
        switch (fieldName)
        {
            case "margins":
                Margins = DeArea.FromJson(jsonNode);
                return true;
            case "padding":
                Padding = DeArea.FromJson(jsonNode);
                return true;
            case "size":
                Size = DeSize.FromJson(jsonNode);
                return true;
            case "min_size":
                MinSize = DeSize.FromJson(jsonNode);
                return true;
            case "max_size":
                MaxSize = DeSize.FromJson(jsonNode);
                return true;
            case "flow":
                if(!RojaUtils.TryAsEnum<DeFlow>(jsonNode, out var flow)) return false;
                Flow = flow;
                return true;
            case "wrap":
                if(!RojaUtils.TryAsNumber(jsonNode, out int wrap)) return false;
                Wrap = wrap;
                return true;
            case "justify":
                if(!RojaUtils.TryAsEnum<DeJustify>(jsonNode, out var justify)) return false;
                Justify = justify;
                return true;
            default:
                return false;
        }
    }
    public static DeTheme? FromJson(JsonNode? jsonNode)
    {
        if(!RojaUtils.TryAsObject(jsonNode, out var jsonObject)) return null;
        var margins = DeArea.FromJson(jsonObject["margins"]);
        var padding = DeArea.FromJson(jsonObject["padding"]);
        var size = DeSize.FromJson(jsonObject["size"]);
        var minSize = DeSize.FromJson(jsonObject["min_size"]);
        var maxSize = DeSize.FromJson(jsonObject["max_size"]);
        DeFlow? flowDirection = RojaUtils.TryAsEnum<DeFlow>(jsonObject["flow"], out var flowDir) ? flowDir : null;
        int? wrap = RojaUtils.TryAsNumber(jsonObject["wrap"], out int w) ? w : null;
        DeJustify? justify = RojaUtils.TryAsEnum<DeJustify>(jsonObject["justify"], out var j) ? j : null;
        return new(margins, padding, size, minSize, maxSize, flowDirection, wrap, justify);
    }
    public JsonNode ToJson()
    {
        JsonObject obj = [];
        obj["margins"] = Margins?.ToJson();
        obj["padding"] = Padding?.ToJson();
        obj["size"] = Size?.ToJson();
        obj["min_size"] = MinSize?.ToJson();
        obj["max_size"] = MaxSize?.ToJson();
        obj["flow"] = Flow?.ToString();
        obj["wrap"] = Wrap?.ToString();
        obj["justify"] = Justify?.ToString();
        return obj;
    }
}
