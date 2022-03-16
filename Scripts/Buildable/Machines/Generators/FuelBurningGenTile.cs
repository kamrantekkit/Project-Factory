using Godot.Collections;

public class FuelBurningGenTile : BaseMultiGenerator
{
    public int MaxFuelItemStorage;
    public float RemainingFuelBurnTime = 0;
    public System.Collections.Generic.List<BaseItemResource> FuelItemStorage = new System.Collections.Generic.List<BaseItemResource>();
    private Dictionary FuelRecipes;
    private Dictionary CurrentRecipe;

    public override void _Ready()
    {
        base._Ready();
        TileName = TileTypes.FuelBurningGenerator;
        FuelRecipes = SceneNode.ProcessingRecipes[TileName.ToString()];

        switch (Tier)
        {
            case ItemType.TierType.Basic:
                base.MaxPowerOutput = 5;
                MaxFuelItemStorage = 5;
                break;
        }

    }
    public override bool RecieveItemOutput(BaseMachine tile)
    {
        if (FuelItemStorage.Count >= MaxFuelItemStorage) return false;
        if (!tile.OutputItem.IsFuel) return false;
        FuelItemStorage.Add(tile.OutputItem);
        return true;
    }

    public override void On_Timer_Timeout()
    {
        if (RemainingFuelBurnTime < 0)
        {
            ConsumeFuel();
        }
        else
        {
            RemainingFuelBurnTime--;
        }
    }

    private void ConsumeFuel()
    {
        if (FuelItemStorage.Count <= 0)
        {
            CurrentPowerOutput = 0;
            UpdatePowerStatus();
            return;
        };
        BaseItemResource Item = FuelItemStorage[0];
        FuelItemStorage.RemoveAt(0);
        CurrentRecipe = FuelRecipes[Item.Name.ToString()] as Dictionary;
        RemainingFuelBurnTime = (float)CurrentRecipe["Time"] * 20;
        CurrentPowerOutput = MaxPowerOutput;
        UpdatePowerStatus();
    }

}
