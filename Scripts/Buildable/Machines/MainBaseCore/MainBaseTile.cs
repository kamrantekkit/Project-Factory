using Godot;
using Godot.Collections;

public class MainBaseTile : BaseMachineMultiTile
{
    public static Dictionary<BaseItemResource, int> StoredResources = new Dictionary<BaseItemResource, int>();
    private BaseMultiGenerator InternalGenerator = new BaseMultiGenerator();

    public override void _Ready()
    {
        base._Ready();
        SceneNode.MainBase = this;

        InternalGenerator.CurrentPowerOutput = 5;
        PowerGridNetwork.ConnectedGenerators.Add(InternalGenerator);
        PowerGridNetwork.UpdatePowerOutput();

        int[] InputSubTile = new int[] { 1, 3, 5, 7 };
        foreach (int i in InputSubTile)
        {
            base.InputSubTiles.Add(SubTileLocations[i]);
        }
    }

    public void UpdatePlayerResourceInvs()
    {
        GetTree().CallGroup("HUDStoredItems", "UpdateAmount");
    }

    public override bool RecieveItemOutput(BaseMachine tile)
    {
        if (!SubTileLocations.Contains(tile.OutputTileLocation)) return false;
        BaseItemResource item = tile.OutputItem;
        if (!StoredResources.ContainsKey(item))
        {
            StoredResources.Add(item, 1);
            UpdatePlayerResourceInvs();
            return true;
        }
        StoredResources[item]++;
        SceneNode.PlayerProgressManager.AddExprience(item.Exprience);
        UpdatePlayerResourceInvs();
        return true;
    }

    public bool CheckForEnoughResources(BaseItemResource resource, int Amount)
    {
        if (!StoredResources.ContainsKey(resource)) return false;
        if (StoredResources[resource] < Amount) return false;
        return true;
    }

    public void UseResourceForRecipe(Dictionary Recipe)
    {
        foreach (string item in Recipe.Keys)
        {
            BaseItemResource ItemResource = SceneNode.ResourceItems[item];
            int Amount = (int)(float)Recipe[item];
            StoredResources[ItemResource] -= Amount;
        }
        UpdatePlayerResourceInvs();
    }

    public void ReturnResourceFromPlayerDestroy(Dictionary Recipe)
    {
        foreach (string item in Recipe.Keys)
        {
            BaseItemResource ItemResource = SceneNode.ResourceItems[item];
            int Amount = (int)(float)Recipe[item];
            StoredResources[ItemResource] += Amount;
        }
        UpdatePlayerResourceInvs();
    }
}
