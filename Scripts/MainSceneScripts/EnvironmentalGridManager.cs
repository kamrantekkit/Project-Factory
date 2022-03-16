using Godot;

public class EnvironmentalGridManager : TileMap
{

    public override void _Ready()
    {
    }
    public Vector2 GetCellFromVector(Vector2 vector)
    {
        return WorldToMap(vector);

    }

    public string GetTileName(Vector2 location)
    {
        var tileID = GetCellv(location);
        string tileName = TileSet.TileGetName(tileID);
        return tileName;
    }

}


