using Godot;
using System;

public class TechAmount : Label
{

    public override void _Ready()
    {
        
        SceneNode.PlayerProgressManager.Connect("UpdateTechAmountGUI", this, "UpdateAmount");
    }

    public void UpdateAmount(int techpoints)
    {
        Text = $"Tech Remaining: {techpoints}";
    }
}
