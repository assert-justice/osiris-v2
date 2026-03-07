using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Style;

/// <summary>
/// Struct <c>DeStyleBox</c> represents an element's size.
/// Generally the extents of a box cannot be negative and are clamped to zero by the layout engine.
/// </summary>
public class DeStyleBox : DeStyleNode
{
    public DeStyleLength Width{get; init;} = new();
    public DeStyleLength Height{get; init;} = new();
    public DeStyleBox(): base(RojaNode.NewDict()){}
    public DeStyleBox(DeStyleLength width, DeStyleLength height): this()
    {
        Width = width; Height = height;
        Data.SetData(ToRoja());
    }
    public DeStyleBox(RojaNode rojaNode): base(rojaNode)
    {
        if(Data.TryGetField("width", out var width)) Width = new(width);
        if(Data.TryGetField("height", out var height)) Height = new(height);
    }
    public override RojaNode ToRoja()
    {
        var res = RojaNode.NewDict();
        res.SetField("width", Width.ToRoja());
        res.SetField("height", Height.ToRoja());
        return res;
    }
}
