using System.Collections.Generic;
using System.Collections.Immutable;

namespace Osiris.Src.Denature;

public class DeEnv
{
    public readonly int WidthPx;
    public readonly int HeightPx;
    public readonly ImmutableDictionary<string, string> Settings;
    public DeEnv(int widthPx, int heightPx)
    {
        WidthPx = widthPx;
        HeightPx = heightPx;
        Settings = ImmutableDictionary.Create<string,string>();
    }
    public DeEnv(int widthPx, int heightPx, KeyValuePair<string,string>[] settings)
    {
        WidthPx = widthPx;
        HeightPx = heightPx;
        Settings = ImmutableDictionary.CreateRange(settings);
    }
    public DeEnv(int widthPx, int heightPx, (string,string)[] settings)
    {
        WidthPx = widthPx;
        HeightPx = heightPx;
        var settingsBuilder = ImmutableDictionary.CreateBuilder<string,string>();
        foreach (var (key, val) in settings)
        {
            settingsBuilder.Add(key, val);
        }
        Settings = settingsBuilder.ToImmutable();
    }
}
