namespace Osiris.Src.Denature.Theme;

public readonly record struct DeArea
{
    public DeLength Top{get; init;} = new();
    public DeLength Bottom{get; init;} = new();
    public DeLength Left{get; init;} = new();
    public DeLength Right{get; init;} = new();
    public DeArea(DeLength all)
    {
        Top = all;
        Bottom = all;
        Left = all;
        Right = all;
    }
    public DeArea(DeLength topAndBottom, DeLength leftAndRight)
    {
        Top = topAndBottom;
        Bottom = topAndBottom;
        Left = leftAndRight;
        Right = leftAndRight;
    }
}
