using System.Text.Json.Nodes;

namespace Osiris.Src.Roja;

public interface IRojaSerializerJson<T>
{
    public static abstract T? FromJson(JsonNode? jsonNode);
    public abstract JsonNode? ToJson();
}
