using Godot;
using buildLayer;
public class SelectedItemHud : Label
{
    private string CurrentText;
    private TextureRect SelectedItemTexture;
    public override void _Ready()
    {
        SelectedItemTexture = GetParent().GetNode("./CenterContainer/HBoxContainer/TextureRect") as TextureRect;
    }
    public void ChangeText(BaseBuildable SelectedTile)
    {
        CurrentText = $"{SelectedTile.TileName}";
        Text = CurrentText;
        SelectedItemTexture.Texture = SelectedTile.ItemSpriteTexture;
    }

    public void UpdateRotation(Rotations currentRotation)
    {
        Text = CurrentText + $", {currentRotation}";
    }
}
