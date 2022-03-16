using Godot;
using Godot.Collections;
using System;

public class ItemCostHandler : HBoxContainer
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

    }

    public void UpdateAmount(Dictionary ItemRecipe)
    {
        if (!ItemRecipe.Contains(AssociatedItem.Name.ToString()))
        {
            Visible = false;
            return;
        }

        Visible = true;
        int ItemAmount = (int)(float)ItemRecipe[AssociatedItem.Name.ToString()];
        AmountLabel.Text = $": {ItemAmount}";
    }

}
