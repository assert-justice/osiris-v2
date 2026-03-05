namespace Osiris.Src.Roja;

public interface IRojaSerializerString<T>
{
    public static abstract bool TryFromString(string serialized, out T res);
    public abstract string ToString();
}
