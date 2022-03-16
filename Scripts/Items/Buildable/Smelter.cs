using Godot;
using ItemType;

public class Smelter : BaseBuildable
{

    public Smelter(TileTypes tileName, bool isBuildable, int itemID, TierType tier, Texture ItemTexture) : base(tileName, isBuildable, itemID, tier, ItemTexture)
    {
        base.TileNode = (PackedScene)ResourceLoader.Load("res://Scenes/BuildableTilesPackedScenes/Machines/Smelter/Smelter.tscn");
        base.IsRotatable = true;
        base.Width = 1;
        base.Height = 2;
    }
}