using Godot;
using ItemType;

public class HotbarButtonHandler : TextureButton
{
    private BaseBuildable AssociatedTile;
    private TierType tier;
    public override void _Ready()
    {

        string categoryTier = GetParent().Name;
        if (TierType.Basic.ToString() == categoryTier)
        {
            tier = TierType.Basic;
        }
        else if (TierType.Advanced.ToString() == categoryTier)
        {
            tier = TierType.Advanced;
        }

        FindTile();
        base._Ready();
    }

    private void FindTile()
    {
        AssociatedTile = SceneNode.BuildTiles.Find(tile => tile.TileName.ToString() == Name && tile.Tier == tier);
    }
    public override void _Pressed()
    {
        HUDManager.Player.ChangeSelectedItem(AssociatedTile);
    }

}
