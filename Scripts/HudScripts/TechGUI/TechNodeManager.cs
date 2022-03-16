using Godot;
using System;

public class TechNodeManager : HBoxContainer
{
    private Label TechNodeLevel;
    public override void _Ready()
    {
        TechNodeLevel = GetNode("./Level") as Label;
    }

    public void UpdateGUI()
    {
        TechNode AssociatedTechNode = PlayerProgressManager.TechTreeNodes[Name];
        TechNodeLevel.Text = $"{Name}: +{AssociatedTechNode.Modifier} {AssociatedTechNode.CurrentLevel}Lvl ";
    }

}
