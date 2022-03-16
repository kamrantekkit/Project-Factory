using Godot;
using System;

public class TechGUI : TabContainer
{

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("TechUpgradeToggle"))
        {
			if (HUDManager.Player.IsGUIOpen)
            {
                HUDManager.Player.IsGUIOpen = false;
                Visible = false;
            }
            else 
            {
                HUDManager.Player.IsGUIOpen = true;
                Visible = true;
            }
        }
    }
}
