global using ThemeMatcher = System.Func<Osiris.Src.Denature.Node.DeNode,Osiris.Src.Denature.DeEnv,bool>;
using System.Text.Json.Nodes;
using Osiris.Src.Denature.Node;
using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Theme;

public class DeThemeSheet
{
    // Todo: add variables to theme / theme sheet
    private readonly (ThemeMatcher[],JsonObject)[] Styles = [];
    public bool TryMergeThemes(ref DeTheme theme, DeNode node, DeEnv env)
    {
        foreach (var (matchers, style) in Styles)
        {
            foreach (var matcher in matchers)
            {
                if(!matcher(node, env)) continue;
                if(!RojaDict.TryMerge(ref theme, style)) return false;
                break;
            }
        }
        return true;
    }
}
