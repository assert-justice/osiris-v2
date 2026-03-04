using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Osiris.Src.Denature.Theme;

public class DeTheme
{
    [JsonConverter(typeof(JsonStringEnumConverter<DeFlowDirection>))]
    public enum DeFlowDirection
    {
        None,
        Column,
        Row,
    }
    [JsonConverter(typeof(JsonStringEnumConverter<DeJustify>))]
    public enum DeJustify
    {
        Stretch,
        Start,
        Center,
        End,
    }
    public DeArea Margins = new();
    public DeArea Padding = new();
    public DeSize Size = new();
    public DeSize MinSize = new();
    public DeSize MaxSize = new()
    {
        Width=new(float.PositiveInfinity, DeLength.DeKind.Px),
        Height=new(float.PositiveInfinity, DeLength.DeKind.Px),
    };
    public DeFlowDirection FlowDirection = DeFlowDirection.None;
    public int Wrap = 0;
    public DeJustify Justify = DeJustify.Stretch;
    public DeTheme(){}
    public DeTheme(DeNode node, DeEnv env, DeThemeSheet themeSheet)
    {
        // Todo: finish implementing this
        foreach (var (name, value) in themeSheet.GetThemeFields(node, env))
        {
            switch (name)
            {
                case "Margins":
                break;
                case "Padding":
                break;
                case "Size":
                break;
                case "MinSize":
                break;
                case "MaxSize":
                break;
                case "FlowDirection":
                break;
                case "Wrap":
                break;
                case "Justify":
                break;
                default:
                throw new Exception($"Unexpected style field name '{name}'");
            }
        }
    }
}
