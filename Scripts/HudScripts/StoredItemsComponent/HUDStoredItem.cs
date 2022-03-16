using Godot;

public class HUDStoredItem : HBoxContainer
{
    private BaseItemResource AssociatedItem;

    private Label AmountLabel;
    public override void _Ready()
    {
        AmountLabel = GetNode("./Amount") as Label;
        
        if (SceneNode.ResourceItems.ContainsKey(Name))
        {
            AssociatedItem = SceneNode.ResourceItems[Name];
        }
        else
        {
            GD.Print("Could not find", Name, "Item");
        }

        UpdateAmount();
    }

    public void UpdateAmount()
    {
        if (!MainBaseTile.StoredResources.ContainsKey(AssociatedItem)) return;
        int ItemAmount = (int)MainBaseTile.StoredResources[AssociatedItem];
        AmountLabel.Text = $"{AssociatedItem.Name}: {ItemAmount}";
    }
}
