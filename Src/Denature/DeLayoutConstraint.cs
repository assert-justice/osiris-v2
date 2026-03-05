namespace Osiris.Src.Denature;

public struct DeLayoutConstraint
{
    public float MinWidth = float.NegativeInfinity;
    public float MinHeight = float.NegativeInfinity;
    public float MaxWidth = float.PositiveInfinity;
    public float MaxHeight = float.PositiveInfinity;
    public DeLayoutConstraint(){}
}
