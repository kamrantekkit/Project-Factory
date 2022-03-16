using Godot;
using ItemType;
public class MachineDrill : BaseBuildable
{
    public MachineDrill(TileTypes tileName, bool isBuildable, int itemID, TierType tier, Texture ItemTexture) : base(tileName, isBuildable, itemID, tier, ItemTexture)
    {
        base.TileNode = (PackedScene)ResourceLoader.Load("res://Scenes/BuildableTilesPackedScenes/Machines/Miners/Miner.tscn");
        base.Width = 1;
        base.Height = 1;
    }

}