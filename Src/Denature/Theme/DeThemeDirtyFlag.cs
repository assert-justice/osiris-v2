namespace Osiris.Src.Denature.Theme;

public class DeThemeDirtyFlag
{
    private int LastCount = int.MinValue;
    private readonly DeTheme Theme = new();
    public DeThemeDirtyFlag(){}
    public DeThemeDirtyFlag(DeTheme theme){Theme = theme;}
    public bool IsDirty(){return LastCount != Theme.WriteCount;}
    public DeTheme GetTheme(bool clearFlag = true)
    {
        if(clearFlag) LastCount = Theme.WriteCount;
        return Theme;
    }
}
