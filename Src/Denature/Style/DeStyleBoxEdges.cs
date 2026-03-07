using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Style;

public class DeStyleBoxEdges : DeStyleNode
{
    public DeStyleLength Top{get; init;} = new();
    public DeStyleLength Bottom{get; init;} = new();
    public DeStyleLength Left{get; init;} = new();
    public DeStyleLength Right{get; init;} = new();
    public DeStyleBoxEdges(): base(null){}
    public DeStyleBoxEdges(DeStyleLength all): base(null)
    {
        Top = all;
        Bottom = all;
        Left = all;
        Right = all;
        Data.SetData(ToRoja());
    }
    public DeStyleBoxEdges(DeStyleLength topBottom, DeStyleLength leftRight): base(null)
    {
        Top = topBottom;
        Bottom = topBottom;
        Left = leftRight;
        Right = leftRight;
        Data.SetData(ToRoja());
    }
    public DeStyleBoxEdges(DeStyleLength top, DeStyleLength bottom, DeStyleLength left, DeStyleLength right): base(null)
    {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
        Data.SetData(ToRoja());
    }
    public DeStyleBoxEdges(RojaNode rojaNode) : base(rojaNode)
    {
        if(Data.TryGetField("top", out var top)) Top = new(top);
        if(Data.TryGetField("bottom", out var bottom)) Bottom = new(bottom);
        if(Data.TryGetField("left", out var left)) Left = new(left);
        if(Data.TryGetField("right", out var right)) Right = new(right);
    }
    public override RojaNode ToRoja()
    {
        var res = RojaNode.NewDict();
        res.SetField("top", Top.ToRoja());
        res.SetField("bottom", Bottom.ToRoja());
        res.SetField("left", Left.ToRoja());
        res.SetField("right", Right.ToRoja());
        return res;
    }
}
