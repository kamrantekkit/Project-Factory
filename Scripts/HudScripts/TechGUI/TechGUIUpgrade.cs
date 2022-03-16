using Godot;
using System;

public class TechGUIUpgrade : Button
{
    public override void _Pressed()
    {
        //Try To Upgrade
        SceneNode.PlayerProgressManager.UpgradeTech(PlayerProgressManager.TechTreeNodes[GetParent().Name]);
    }
}
