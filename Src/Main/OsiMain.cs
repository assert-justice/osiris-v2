using System.Text.Json;
using Godot;
using Osiris.Src.Denature;
using Osiris.Src.Denature.Theme;

namespace Osiris.Src.Main;

public partial class OsiMain : Control
{
    public override void _Ready()
    {
        GetTree().Quit();
    }
}
