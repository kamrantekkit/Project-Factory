using Godot;
using ItemType;
public class Converyor : BaseBuildable
{
    public ConveryorDirectionType ConveryorDirection;

    public Converyor LeftTurn;
    public Converyor RightTurn;
    public Converyor InterSection;
    public Converyor(TileTypes tileName, bool isBuildable, int itemID, TierType tier, ConveryorDirectionType converyorDirection, Texture ItemTexture) : base(tileName, isBuildable, itemID, tier, ItemTexture)
    {
        base.IsRotatable = true;
        base.TileNode = (PackedScene)ResourceLoader.Load("res://Scenes/BuildableTilesPackedScenes/Machines/Logistics/Converyors/Converyor.tscn");
        ConveryorDirection = converyorDirection;
        base.Width = 1;
        base.Height = 1;
    }
}

public enum ConveryorDirectionType
{
    Straight,
    LeftTurn,
    RightTurn,
    Intersection
}