using System;
using Osiris.Src.Denature.Node;
using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Style;

public class DeStyleSheet
{
    private readonly (Func<DeNode, DeEnv, bool>[], RojaNode)[] Styles = [];
    public RojaNode Merge(ref RojaNode rojaNode, DeNode node, DeEnv env)
    {
        foreach (var (matchers, style) in Styles)
        {
            foreach (var matcher in matchers)
            {
                if(!matcher(node, env)) continue;
                foreach (var (key, value) in style.GetEntries())
                {
                    rojaNode.SetField(key, value);
                }
                break;
            }
        }
        return rojaNode;
    }
}
