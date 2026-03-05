using System;
using System.Text.Json.Nodes;
using Godot;
using Osiris.Src.Denature.Node;
using Osiris.Src.Main;

namespace Osiris.Src.Ui;

public abstract class OsiUiNode : DeNode
{
    protected OsiUiNode(string id, JsonObject props, string tag = "div") : base(id, props, tag){}
    protected abstract Control GetControl();
    protected override void OnMount()
    {
        var control = GetControl();
        if(Parent is null)
        {
            // we must be the root
            OsiMain.GetRootControl().AddChild(control);
        }
        else if(Parent is OsiUiNode uiNode)
        {
            uiNode.GetControl().AddChild(control);
        } 
        else throw new Exception("Invalid parent type");
    }
}
