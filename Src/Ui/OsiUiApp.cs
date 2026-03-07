using Godot;

namespace Osiris.Src.Ui;

public class OsiUiApp : OsiUiNode
{
    private readonly Panel AppControl;
    public OsiUiApp() : base("app", [])
    {
        AppControl = new();
        // if(!DeLength.TryFromString("100 %", out var width)) throw new System.Exception("oops");
        // if(!DeLength.TryFromString("100 %", out var height)) throw new System.Exception("oops");
        // ThemeOverrides.GetTheme().TrySetField<DeSize>("width", new(width, height));
    }

    protected override Control GetControl()
    {
        return AppControl;
    }
    protected override void OnMount()
    {
        base.OnMount();
        GD.Print(AppControl.GetParent());
        AppControl.Size = new(1000, 1000);
    }
}
