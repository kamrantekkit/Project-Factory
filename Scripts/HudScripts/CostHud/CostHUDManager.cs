using Godot;
using Godot.Collections;

public class CostHUDManager : VBoxContainer
{
    public void UpdateTileCost(Dictionary ItemRecipe)
    {
        GetTree().CallGroup("CostTileHUD","UpdateAmount",ItemRecipe);
    }

}
