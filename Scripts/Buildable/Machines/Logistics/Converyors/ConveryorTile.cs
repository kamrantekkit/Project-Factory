using Godot;

public class ConveryorTile : BaseMachine
{
    protected ConveryorTile LeftConveryor;
    protected ConveryorTile RightConveryor;
    protected buildLayer.Rotations ConveryorLeftSide;
    protected buildLayer.Rotations ConveryorRightSide;
    private Sprite ItemSprite;

    private readonly int MaxTimeForOperation = 3;
    private int CurrentTimeForOperation = 0;
    private readonly int MaxProgressToTransport = 10;
    private int CurrentProgressToTransport = 0;
    private int Speed = 2;
    private readonly int MaxTransportAmount = 1;
    private int CurrentTransportAmount = 0;
    public override void _Ready()
    {
        base._Ready();
        base.TileName = TileTypes.Converyor;
        UpdateConverorSides();

        if (CheckForMachine())
        {
            UpdateConveryorSide(this);
        }
    }
    public override void _ExitTree()
    {
        if (CheckForMachine())
        {
            UpdateConveryorSide(null);
        }
    }

    //Checks if converyour belt is outputing to the side of converyor belt
    public void UpdateConveryorSide(ConveryorTile SideConveryor)
    {
        if (this.OutputTile.TileName != TileTypes.Converyor) return;
        ConveryorTile OutputTile = (ConveryorTile)this.OutputTile;
        if (OutputTile.ConveryorLeftSide == TileRotation)
        {
            OutputTile.LeftConveryor = SideConveryor;
        }
        else if (OutputTile.ConveryorRightSide == TileRotation)
        {
            OutputTile.RightConveryor = SideConveryor;
        }
        OutputTile.UpdateTexture();
    }

    //Update the texture according to belts connected to it
    public void UpdateTexture()
    {
        Converyor itemConveryorBuildable = (Converyor)AssociatedItem;
        if (LeftConveryor != null && RightConveryor != null)
        {
            BuildManager.UpdateTile(TileLocation, itemConveryorBuildable.InterSection.ItemID, TileRotation);
        }
        else if (RightConveryor != null)
        {
            BuildManager.UpdateTile(TileLocation, itemConveryorBuildable.RightTurn.ItemID, TileRotation);
        }
        else if (LeftConveryor != null)
        {
            BuildManager.UpdateTile(TileLocation, itemConveryorBuildable.LeftTurn.ItemID, TileRotation);
        }
        else
        {
            BuildManager.UpdateTile(TileLocation, itemConveryorBuildable.ItemID, TileRotation);
        }
    }

    //Sets the converyor sides relative to its rotation
    private void UpdateConverorSides()
    {
        switch (TileRotation)
        {
            case buildLayer.Rotations.Up:
                ConveryorLeftSide = buildLayer.Rotations.Right;
                ConveryorRightSide = buildLayer.Rotations.Left;
                break;
            case buildLayer.Rotations.Right:
                ConveryorLeftSide = buildLayer.Rotations.Down;
                ConveryorRightSide = buildLayer.Rotations.Up;
                break;
            case buildLayer.Rotations.Down:
                ConveryorLeftSide = buildLayer.Rotations.Left;
                ConveryorRightSide = buildLayer.Rotations.Right;
                break;
            case buildLayer.Rotations.Left:
                ConveryorLeftSide = buildLayer.Rotations.Up;
                ConveryorRightSide = buildLayer.Rotations.Down;
                break;
        }

        CheckExistingSideConveryors(ConveryorLeftSide);
        CheckExistingSideConveryors(ConveryorRightSide);
    }

    //Checks if the converyour already has converyors on its sides outputing to it
    private void CheckExistingSideConveryors(buildLayer.Rotations ConveryorSide)
    {
        Vector2 LocationCheck;
        switch (ConveryorSide)
        {
            case buildLayer.Rotations.Up:
                LocationCheck = new Vector2(TileLocation.x, TileLocation.y - 1);
                break;
            case buildLayer.Rotations.Right:
                LocationCheck = new Vector2(TileLocation.x + 1, TileLocation.y);
                break;
            case buildLayer.Rotations.Down:
                LocationCheck = new Vector2(TileLocation.x, TileLocation.y + 1);
                break;
            case buildLayer.Rotations.Left:
                LocationCheck = new Vector2(TileLocation.x - 1, TileLocation.y);
                break;
            default:
                return;
        }
        if (buildLayer.BuildLayerManager.PlacedTiles.ContainsKey(LocationCheck))
        {
            BaseBuildTile tile = buildLayer.BuildLayerManager.PlacedTiles[LocationCheck];
            if (tile.TileName != TileTypes.Converyor) return;
            ConveryorTile converyortile = (ConveryorTile)tile;
            converyortile.CheckForMachine();
            if (converyortile.OutputTile != this) return;
            converyortile.UpdateConveryorSide(converyortile);
        }
    }

    public override void On_Timer_Timeout()
    {
        CurrentTimeForOperation++;
        if (CurrentTimeForOperation == MaxTimeForOperation)
        {
            MachineOperate();
        }
    }

    public override bool RecieveItemOutput(BaseMachine tile)
    {
        if (CurrentTransportAmount < MaxTransportAmount)
        {
            OutputItem = tile.OutputItem;
            CurrentTransportAmount++;
            ItemSprite = new Sprite
            {
                Texture = OutputItem.ItemSpriteTexture,
                Position = new Vector2(8, 8)
            };
            AddChild(ItemSprite);
            return true;
        }
        return false;
    }

    private void OutputResource()
    {
        if (CurrentTransportAmount != 0 && CheckForMachine())
        {
            if (OutputTile.RecieveItemOutput(this))
            {
                RemoveChild(ItemSprite);
                OutputItem = null;
                ItemSprite = null;
                CurrentTransportAmount--;
                return;
            }
        }
        return;
    }
    public bool CheckForMachine()
    {

        Vector2 LocationCheck;
        switch (TileRotation)
        {
            case buildLayer.Rotations.Up:
                LocationCheck = new Vector2(TileLocation.x, TileLocation.y - 1);
                break;
            case buildLayer.Rotations.Right:
                LocationCheck = new Vector2(TileLocation.x + 1, TileLocation.y);
                break;
            case buildLayer.Rotations.Down:
                LocationCheck = new Vector2(TileLocation.x, TileLocation.y + 1);
                break;
            case buildLayer.Rotations.Left:
                LocationCheck = new Vector2(TileLocation.x - 1, TileLocation.y);
                break;
            default:
                return false;
        }

        if (buildLayer.BuildLayerManager.PlacedTiles.ContainsKey(LocationCheck))
        {
            BaseBuildTile tile = buildLayer.BuildLayerManager.PlacedTiles[LocationCheck];
            if (!tile.GetType().IsSubclassOf(typeof(BaseMachine))) return false;
            BaseMachine machineTile = (BaseMachine)tile;
            OutputTileLocation = LocationCheck;
            OutputTile = machineTile;
            return true;
        }
        else
        {
            OutputTile = null;
            return false;
        }
    }
    private void MachineOperate()
    {
        if (OutputItem == null)
        {
            CurrentTimeForOperation = 0;
            CurrentProgressToTransport = 0;
            return;
        }

        if (CurrentProgressToTransport >= MaxProgressToTransport)
        {
            OutputResource();
            CurrentProgressToTransport = 0;
        }

        CurrentProgressToTransport += Speed;
        CurrentTimeForOperation = 0;
    }
}