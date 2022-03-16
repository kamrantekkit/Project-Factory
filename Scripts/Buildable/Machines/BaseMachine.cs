using Godot;

public class BaseMachine : BaseBuildTile
{
    public BaseMachine OutputTile;
    public Vector2 OutputTileLocation;
    public BaseItemResource OutputItem;
    public PowerGridNetwork PowerGridNetwork;
    public float PowerConsumption;
    protected bool IsConsumer = true;
    public override void _Ready()
    {
        Timer WorldTimer = (Timer)GetNode("/root/SceneNode/WorldTimer");
        WorldTimer.Connect("timeout", this, "On_Timer_Timeout");

        BuildManager = (buildLayer.BuildLayerManager)GetParent();
        PowerGridNetwork = (PowerGridNetwork)GetNode("/root/SceneNode/PowerGridNetwork");
        if (IsConsumer)
        {
            PowerGridNetwork.ConnectedConsumers.Add(this);
            UpdatePowerStatus();
        }
    }

    public override void _ExitTree()
    {
        PowerGridNetwork.ConnectedConsumers.Remove(this);
        UpdatePowerStatus();
    }
    protected virtual void UpdatePowerStatus()
    {
        PowerGridNetwork.UpdatePowerConsumption();
    }
    public virtual void On_Timer_Timeout()
    {
        return;
    }

    public virtual bool RecieveItemOutput(BaseMachine tile)
    {
        return false;
    }

    protected virtual void UpdateConveryorOutput(Vector2 tileLocation)
    {
        BaseBuildTile tile = buildLayer.BuildLayerManager.PlacedTiles[tileLocation];
        if (tile.TileName != TileTypes.Converyor) return;
        OutputTile = (ConveryorTile)tile;
    }

}
