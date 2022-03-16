using Godot;
using System.Collections.Generic;

public class BaseMachineMultiTile : BaseMachine
{
    public List<Vector2> InputSubTiles = new List<Vector2>();
    public Vector2 OutputSubTile;
    public override void _Ready()
    {
        base._Ready();

    }

    public override bool RecieveItemOutput(BaseMachine tile)
    {
        return false;
    }



}
