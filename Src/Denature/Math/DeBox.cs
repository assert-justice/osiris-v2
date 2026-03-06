namespace Osiris.Src.Denature.Math;

public readonly struct DeBox
{
    public float Width{get; init;}
    public float Height{get; init;}
    public DeBox(){}
    public DeBox(float width, float height)
    {
        Width = width;
        Height = height;
    }
    public DeBox Grow(DeBoxEdges box)
    {
        return new(Width + box.Left + box.Right, Height + box.Top + box.Bottom);
    }
    public DeBox Shrink(DeBoxEdges box)
    {
        return new(Width - box.Left - box.Right, Height - box.Top - box.Bottom);
    }
    public static DeBox Max(params DeBox[] sizes)
    {
        float mw = float.NegativeInfinity;
        float mh = float.NegativeInfinity;
        foreach (var size in sizes)
        {
            if(size.Width > mw) mw = size.Width;
            if(size.Height > mh) mh = size.Height;
        }
        return new(mw, mh);
    }
    public static DeBox Min(params DeBox[] sizes)
    {
        float mw = float.PositiveInfinity;
        float mh = float.PositiveInfinity;
        foreach (var size in sizes)
        {
            if(size.Width < mw) mw = size.Width;
            if(size.Height < mh) mh = size.Height;
        }
        return new(mw, mh);
    }
}
