using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Style;

public class RojaNodeDirtyFlag
{
    private ulong LastCount = ulong.MaxValue;
    private readonly RojaNode Data = RojaNode.NewDict();
    public RojaNodeDirtyFlag(){}
    public RojaNodeDirtyFlag(RojaNode data){Data = data;}
    public bool IsDirty(){return LastCount != Data.WriteCount;}
    public RojaNode GetData(bool clearFlag = true)
    {
        if(clearFlag) LastCount = Data.WriteCount;
        return Data;
    }
    public void SetData(RojaNode data)
    {
        Data.SetData(data);
    }
    public void SetDirty(){LastCount = ulong.MaxValue;}
}
