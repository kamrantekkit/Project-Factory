using Godot;

public class MinerTile : BaseMachine
{
    private readonly int MaxTimeForOperation = 60;
    private readonly int MaxItemStorage = 3;
    private int CurrentItemStack = 0;
    private int CurrentTimeForOperation = 0;
    private EnvironmentalGridManager EnvLayerManager;


    public MinerTile()
    {
        switch (Tier)
        {
            case ItemType.TierType.Basic:
                PowerConsumption = 1;
                break;
        }
    }
    public override void _Ready()
    {
        base._Ready();
        EnvLayerManager = GetNode("/root/SceneNode/Navigation2D/EnvLayer") as EnvironmentalGridManager;
        CheckOre();
    }

    //Check Ground Tile for Ore
    private void CheckOre()
    {
        string groundTile = EnvLayerManager.GetTileName(TileLocation);
        if (!SceneNode.ResourceItems.ContainsKey(groundTile))
        {
            OutputItem = null;
        }
        OutputItem = SceneNode.ResourceItems[groundTile];
    }

    public override void On_Timer_Timeout()
    {
        if (OutputItem == null) return;
        CurrentTimeForOperation++;

        if (CurrentTimeForOperation >= MaxTimeForOperation && !PowerGridNetwork.IsOverloaded)
        {
            MachineOperate();
            OutputResource();
        }
    }

    private void OutputResource()
    {
        CheckForLogistics();
        if (CurrentItemStack != 0 && OutputTile != null)
        {
            if (OutputTile.RecieveItemOutput(this))
            {
                CurrentItemStack--;
                return;
            }
        }
    }

    private void CheckForLogistics()
    {
        Vector2 LocationCheckUp = new Vector2(TileLocation.x, TileLocation.y - 1);
        Vector2 LocationCheckRight = new Vector2(TileLocation.x + 1, TileLocation.y);
        Vector2 LocationCheckDown = new Vector2(TileLocation.x, TileLocation.y + 1);
        Vector2 LocationCheckLeft = new Vector2(TileLocation.x - 1, TileLocation.y);

        if (CheckIfConveryorBelt(LocationCheckUp))
        {
            UpdateConveryorOutput(LocationCheckUp);
            return;
        }
        else if (CheckIfConveryorBelt(LocationCheckRight))
        {
            UpdateConveryorOutput(LocationCheckRight);
            return;
        }
        else if (CheckIfConveryorBelt(LocationCheckDown))
        {
            UpdateConveryorOutput(LocationCheckDown);
            return;
        }
        else if (CheckIfConveryorBelt(LocationCheckLeft))
        {
            UpdateConveryorOutput(LocationCheckLeft);
            return;
        }
        else
        {
            OutputTile = null;
        }
    }

    private bool CheckIfConveryorBelt(Vector2 LocationToCheck)
    {
        if (buildLayer.BuildLayerManager.PlacedTiles.ContainsKey(LocationToCheck))
        {
            BaseBuildTile Tile = buildLayer.BuildLayerManager.PlacedTiles[LocationToCheck];
            if (Tile.TileName == TileTypes.Converyor)
            {
                return true;
            }
        }
        return false;
    }

    private void MachineOperate()
    {
        if (CurrentItemStack < MaxItemStorage)
        {
            CurrentItemStack++;
        }
        CurrentTimeForOperation = 0;
    }
}
