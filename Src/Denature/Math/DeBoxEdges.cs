namespace Osiris.Src.Denature.Math;

public readonly struct DeBoxEdges
{
    public float Top{get; init;}
    public float Bottom{get; init;}
    public float Left{get; init;}
    public float Right{get; init;}
    public DeBoxEdges(){}
    public DeBoxEdges(float all)
    {
        Top = all;
        Bottom = all;
        Left = all;
        Right = all;
    }
    public DeBoxEdges(float topBottom, float leftRight)
    {
        Top = topBottom;
        Bottom = topBottom;
        Left = leftRight;
        Right = leftRight;
    }
    public DeBoxEdges(float top, float bottom, float left, float right)
    {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }
}