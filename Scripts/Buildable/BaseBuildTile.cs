using Godot;
using ItemType;
using System.Collections.Generic;
public class BaseBuildTile : Node2D
{
    public int health;
    public TileTypes TileName { get; set; }
    public Vector2 TileLocation;
    public List<Vector2> SubTileLocations = new List<Vector2>();
    public TierType Tier;
    public buildLayer.Rotations TileRotation { get; set; }
    public buildLayer.BuildLayerManager BuildManager;
    public BaseBuildable AssociatedItem;
    public void Destroy()
    {
        QueueFree();
    }
}
