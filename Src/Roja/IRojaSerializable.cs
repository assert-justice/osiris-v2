namespace Osiris.Src.Roja;

public interface IRojaSerializable<T>
{
    public static abstract bool TryToValue(RojaNode? rojaNode, out T value);
    public abstract RojaNode ToRoja();
}

public class RojaEnum<T> : IRojaSerializable<RojaEnum<T>> where T : struct
{
    public readonly T Value;
    public RojaEnum(T value){Value = value;}
    public static bool TryToValue(RojaNode? rojaNode, out RojaEnum<T> value)
    {
        throw new System.NotImplementedException();
    }

    public RojaNode ToRoja()
    {
        return Value.ToString() ?? string.Empty;
    }
}
public class RojaFloat : IRojaSerializable<RojaFloat>
{
    public readonly float Value;
    public RojaFloat(float value){Value = value;}
    public static bool TryToValue(RojaNode? rojaNode, out RojaFloat value)
    {
        throw new System.NotImplementedException();
    }

    public RojaNode ToRoja()
    {
        return Value;
    }
}
