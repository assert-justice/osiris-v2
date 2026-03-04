using System.Text.Json;
using Godot;
using Osiris.Src.Denature;
using Osiris.Src.Denature.Theme;

namespace Osiris.Src.Main;

public partial class OsiMain : Control
{
    public override void _Ready()
    {
        DeLength deLength = new(100, DeLength.DeKind.Em);
        string strLength = JsonSerializer.Serialize(deLength, DeStatic.SerializerOptions);
        GD.Print(strLength);
        DeSize deSize = new();
        string strSize = JsonSerializer.Serialize(deSize, DeStatic.SerializerOptions);
        GD.Print(strSize);
        DeTheme deTheme = new();
        string strTheme = JsonSerializer.Serialize(deTheme, DeStatic.SerializerOptions);
        GD.Print(strTheme);
        var deTheme2 = JsonSerializer.Deserialize<DeTheme>(strTheme, DeStatic.SerializerOptions);
        GD.Print(deTheme2);
        GetTree().Quit();
    }
}
