using Godot;
using ItemType;
public class FuelBurningGen : BaseBuildable
{
    public FuelBurningGen(TileTypes tileName, bool isBuildable, int itemID, TierType tier, Texture ItemTexture) : base(tileName, isBuildable, itemID, tier, ItemTexture)
    {
        base.TileNode = (PackedScene)ResourceLoader.Load("res://Scenes/BuildableTilesPackedScenes/Machines/Generators/FuelBurningGen.tscn");
        base.Width = 2;
        base.Height = 2;
    }

}