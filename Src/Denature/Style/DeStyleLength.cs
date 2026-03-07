using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Style;

/// <summary>
/// Struct <c>DeStyleLength</c> represents a length in a wide variety of units.
/// </summary>
public class DeStyleLength : DeStyleNode
{
    public DeLengthUnit Unit{get; init;} = DeLengthUnit.Px;
    public float Value{get; init;} = 0;
    public DeStyleLength(): this(RojaNode.NewDict()){}
    public DeStyleLength(DeLengthUnit unit, float value): this()
    {
        Unit = unit; Value = value;
        Data.SetData(ToRoja());
    }
    public DeStyleLength(RojaNode rojaNode): base(rojaNode)
    {
        if(RojaNode.TryAsEnum<DeLengthUnit>(rojaNode?.GetField("unit"), out var unit)) Unit = unit;
        if(RojaNode.TryAsFloat(rojaNode?.GetField("value"), out float value)) Value = value;
    }
    public override RojaNode ToRoja()
    {
        var res = RojaNode.NewDict();
        res.SetField("unit", Unit.ToString());
        res.SetField("value", Value);
        return res;
    }
}
