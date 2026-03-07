using Osiris.Src.Roja;

namespace Osiris.Src.Denature.Style;

public class DeStyle : DeStyleNode
{
    /// <summary>
    /// Field <c>Size</c> represents the ideal size of the element. 
    /// If it is smaller than MinSize or larger than MaxSize it is ignored.
    /// </summary>
    public DeStyleBox Size{get; init;} = new();
    public DeStyleBox MinSize{get; init;} = new();
    public DeStyleBox MaxSize{get; init;} = new();
    // public DeStyleBoxEdges Padding{get; init;} = new();
    // public DeStyleBoxEdges Border{get; init;} = new();
    // public DeStyleBoxEdges Margins{get; init;} = new();
    // public DeFlowMode FlowMode{get; init;} = DeFlowMode.Column;
    // public DeAlignMode AlignModeH{get; init;} = DeAlignMode.Stretch;
    // public DeAlignMode AlignModeV{get; init;} = DeAlignMode.Stretch;
    // public DeStyleLength AutoColumnLength{get; init;} = new();
    // public DeStyleLength AutoRowLength{get; init;} = new();
    // public DeStyleLength[] ColumnLengths{get; init;} = [];
    // public DeStyleLength[] RowLengths{get; init;} = [];
    // public int GridColumnStart{get; init;} = new();
    // public int GridColumnEnd{get; init;} = new();
    // public int GridRowStart{get; init;} = new();
    // public int GridRowEnd{get; init;} = new();
    // Allow stretch controls whether the element is allowed to grow beyond its max size if its contents don't fit
    // Otherwise the contents are clipped
    // If allowed, the layout engine should put the element in a scroll box
    // public DeDirection AllowStretch{get; init;} = DeDirection.None;
    // public DeDirection AllowWrap{get; init;} = DeDirection.None;
    public DeStyle(): this(RojaNode.NewDict()){}
    public DeStyle(RojaNode rojaNode): base(rojaNode)
    {
        if(Data.TryGetField("size", out var size)) Size = new(size);
        if(Data.TryGetField("min_size", out var minSize)) MinSize = new(minSize);
        if(Data.TryGetField("max_size", out var maxSize)) MaxSize = new(maxSize);
    }
    public override RojaNode ToRoja()
    {
        throw new System.NotImplementedException();
    }
}
