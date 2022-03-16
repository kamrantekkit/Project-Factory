using Godot;
using ItemType;
public class MainCore : BaseBuildable
{
    public MainCore(TileTypes tileName, bool isBuildable, int itemID, TierType tier, Texture ItemTexture) : base(tileName, isBuildable, itemID, tier, ItemTexture)
    {
        base.TileNode = (PackedScene)ResourceLoader.Load("res://Scenes/BuildableTilesPackedScenes/Machines/MainBaseCore/MainBase.tscn");
        base.Width = 3;
        base.Height = 3;
        base.IsDestroyable = false;
    }
}
