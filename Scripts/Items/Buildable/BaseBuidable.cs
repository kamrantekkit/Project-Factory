using Godot;
using ItemType;
public class BaseBuildable : BaseItem
{
    public TileTypes TileName { get; protected set; }

    public int Width { get; protected set; }
    public int Height { get; protected set; }
    public bool IsBuildable { get; protected set; }
    public bool IsRotatable { get; protected set; }
    public int ItemID { get; protected set; }
    public bool IsDestroyable { get; protected set; } = true;
    public PackedScene TileNode; //Reference to the prepacked Node loaded with corresponding tile script
    public BaseBuildable(TileTypes tileName, bool isBuildable, int itemID, TierType tier, Texture ItemTexture) : base(tier)
    {
        TileName = tileName;
        IsBuildable = isBuildable;
        IsRotatable = false;
        ItemID = itemID;
        Width = 1;
        Height = 1;
        ItemSpriteTexture = ItemTexture;
    }
}

public enum TileTypes
{
    MainBase,
    Drill,
    Converyor,
    Smelter,
    FuelBurningGenerator,
}