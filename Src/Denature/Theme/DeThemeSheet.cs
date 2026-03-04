using System;
using System.Collections.Generic;

namespace Osiris.Src.Denature.Theme;

public class DeThemeSheet
{
    private readonly (Func<DeNode,DeEnv,bool>[],(string,string)[])[] Styles = [];
    public IEnumerable<(string,string)> GetThemeFields(DeNode node, DeEnv env)
    {
        foreach (var (matchers, style) in Styles)
        {
            foreach (var matcher in matchers)
            {
                if(!matcher(node, env)) continue;
                foreach (var field in style)
                {
                    yield return field;
                }
            }
        }
    }
}
