using System;
using System.Collections.Generic;
using System.Linq;
using Osiris.Src.Denature.Theme;

namespace Osiris.Src.Denature.Node;

public abstract partial class DeNode
{
    public DeTransform Transform{get; private set;} = new();
    private DeLayoutConstraint LayoutConstraint = new();
    private void UpdateTransform()
    {
        /* Layout
        Each node knows what its parent's max size is
        In fact the parent probably tells the child how big its allowed to be
        Earlier nodes in the hierarchy get priority
        If there simply isn't enough room it may be possible to scroll down
        Avoid scrolling horizontally at all costs
        */
        if(Dom is null) return;
        var env = Dom.GetEnv();
    }
    // Old Garbage
    // private void UpdateTransform()
    // {
    //     // When a node is rendered its transform needs to be updated.
    //     // This proceeds from the top of the tree to the bottom
    //     // Typically the parent should know what its maximum size is
    //     // The children should know what their minimum size is
    //     // The default behavior should be to minimize the size of nodes
    //     // First let's calculate our minimum size. That just comes from our style
    //     // Unless its a percent or a flex or something. Then it defaults to zero

    //     var minSize = GetMinSize();
        
    //     Transform = new()
    //     {
    //         MinWidth = minWidth,
    //         MinHeight = minHeight,
    //     };
    // }
    // private DeMath.Vec2[] GetMinSizes()
    // {
    //     var minThemeSize = GetThemeMinSize();
    //     var minContentsSizes = GetContentsMinSizes();
    //     return DeMath.Vec2.Max(minThemeSize, minContentsSize);
    // }
    // private DeMath.Vec2 GetThemeMinSize()
    // {
    //     var pad = DeArea.GetAreaPx(Theme.Padding);
    //     var size = DeSize.GetSizePx(Theme.MinSize);
    //     return DeMath.Vec2.Grow(size, pad);
    // }
    // private DeMath.Vec2[] GetContentsMinSizes()
    // {
    //     int wrap = Theme.Wrap ?? 0;
    //     var flow = Theme.Flow ?? DeTheme.DeFlow.None;
    //     var justify = Theme.Justify ?? DeTheme.DeJustify.Start;
    //     DeMath.Vec2[] minChildSizes = [..Children.Select(c => c.GetMinSize())];
    //     var maxMinSize = DeMath.Vec2.Max(minChildSizes);
    //     if(flow == DeTheme.DeFlow.None) return maxMinSize;
    //     return flow switch
    //     {
    //         DeTheme.DeFlow.None => maxMinSize,
    //         DeTheme.DeFlow.Row => new(Enumerable.Sum(minChildSizes, size=>size.X), maxMinSize.Y),
    //         DeTheme.DeFlow.Column => new(maxMinSize.X, Enumerable.Sum(minChildSizes, size=>size.Y)),
    //     };
    // }
    // private static GetMinSizeRow()
}
