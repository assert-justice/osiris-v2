namespace Osiris.Src.Denature.Theme;

public readonly record struct DeSize
{
    public DeLength Width{get; init;} = new();
    public DeLength Height{get; init;} = new();
    public DeSize(){}
}
