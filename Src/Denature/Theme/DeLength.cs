using System;
using System.Text.Json.Serialization;
using Godot;

namespace Osiris.Src.Denature.Theme;

public readonly struct DeLength
{
    [JsonConverter(typeof(JsonStringEnumConverter<DeKind>))]
    public enum DeKind
    {
        Px,
        Percent,
        Em,
        Flex,
    }
    public readonly float Value = 0;
    public readonly DeKind Kind = DeKind.Px;
    public DeLength(){}
    public DeLength(float value, DeKind kind)
    {
        Value = value;
        Kind = kind;
    }
    public static bool TryParse(string lengthStr, out DeLength length)
    {
        length = default;
        if(lengthStr == "auto")
        {
            length = new();
            return true;
        }
        int sepIdx = lengthStr.Find(' ');
        if(sepIdx == -1) return false;
        char[] chars = lengthStr.ToCharArray();
        ReadOnlySpan<char> valueSpan = chars.AsSpan(0, sepIdx);
        if(!float.TryParse(valueSpan, out float value)) return false;
        for (int idx = sepIdx; idx < chars.Length; idx++)
        {
            char c = chars[idx];
            if(c == ' ') continue;
            if(char.IsAsciiLetterLower(c)) chars[idx] = char.ToUpper(c);
            if(!Enum.TryParse<DeKind>(chars.AsSpan(idx), out var kind)) return false;
            length = new(value, kind);
            return true;
        }
        return false;
    }
}
