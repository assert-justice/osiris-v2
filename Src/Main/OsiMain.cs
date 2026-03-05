using System;
using Godot;
using Osiris.Src.Denature;
using Osiris.Src.Ui;

namespace Osiris.Src.Main;

public partial class OsiMain : Control
{
    private static OsiMain? RootControl;
    private DeDom? Dom;
    public static OsiMain GetRootControl()
    {
        if(RootControl is not null) return RootControl;
        throw new Exception("Tried to read main instance before initialization");
    }
    public override void _Ready()
    {
        RootControl = this;
        Dom = new(new OsiUiApp());

        // GetTree().Quit();
    }
    public override void _PhysicsProcess(double delta)
    {
        Dom?.Update();
    }
}
