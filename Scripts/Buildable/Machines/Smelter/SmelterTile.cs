using Godot;
using Godot.Collections;
using System.Collections.Generic;

public class SmelterTile : BaseMachineMultiTile
{
    private readonly int MaxItemStorageInput = 4;
    private int CurrentItemStackInput = 0;
    private int CurrentTimeForOperation = 0;
    private int MaxTimeForOutput = 5;
    private int CurrentTimeForOutput = 0;
    private List<BaseItemResource> QueuedItemsInput = new List<BaseItemResource>();
    private List<BaseItemResource> QueuedItemsOutput = new List<BaseItemResource>();
    private Dictionary SmelterRecipes;
    private Dictionary CurrentRecipe;


    public override void _Ready()
    {
        InputSubTiles.Add(SubTileLocations[0]);
        OutputSubTile = SubTileLocations[1];
        SmelterRecipes = SceneNode.ProcessingRecipes[TileName.ToString()] as Dictionary;

        switch (Tier)
        {
            case ItemType.TierType.Basic:
                base.PowerConsumption = 2;
                break;
        }
        base._Ready();
    }

    public override bool RecieveItemOutput(BaseMachine tile)
    {
        GD.Print(tile.OutputItem.Name);
        if (tile.TileRotation == TileRotation && !InputSubTiles.Contains(tile.OutputTileLocation)) return false;
        if (CurrentItemStackInput > MaxItemStorageInput || !SmelterRecipes.Contains(tile.OutputItem.Name.ToString())) return false;
        QueuedItemsInput.Add(tile.OutputItem);
        CurrentItemStackInput++;
        return true;
    }


    public override void On_Timer_Timeout()
    {
        if (GetRecipe())
        {
            CurrentTimeForOperation++;
            float reciepeTime = (float)CurrentRecipe["Time"];
            if (CurrentTimeForOperation >= reciepeTime && !PowerGridNetwork.IsOverloaded)
            {
                MachineOperate();
                CurrentTimeForOperation = 0;
            }
        }
        if (CurrentTimeForOutput >= MaxTimeForOutput && QueuedItemsOutput.Count > 0)
        {
            OutputResource();
        }

        CurrentTimeForOutput++;
    }

    private void MachineOperate()
    {
        Array ProducedItems = CurrentRecipe["Output"] as Array;
        int RequiredAmount = (int)(float)CurrentRecipe["Amount"];
        BaseItemResource ItemInput = QueuedItemsInput[0];
        List<BaseItemResource> avaliableItemsInStack = QueuedItemsInput.FindAll(item => item == QueuedItemsInput[0]);
        GD.Print(QueuedItemsOutput.Count);
        if (avaliableItemsInStack.Count < RequiredAmount)
        {
            return;
        }

        foreach (string Item in ProducedItems)
        {
            QueuedItemsOutput.Add(SceneNode.ResourceItems[Item]);
        }

        for (int i = 0; i < RequiredAmount; i++)
        {
            QueuedItemsInput.Remove(ItemInput);
        }

        CurrentItemStackInput -= RequiredAmount;
    }

    private bool GetRecipe()
    {
        if (QueuedItemsInput.Count <= 0) return false;
        BaseItemResource Item = QueuedItemsInput[0];
        CurrentRecipe = SmelterRecipes[Item.Name.ToString()] as Dictionary;
        return true;
    }


    private void OutputResource()
    {
        if (CheckForLogistics())
        {
            OutputItem = QueuedItemsOutput[0];
            if (OutputTile.RecieveItemOutput(this))
            {
                QueuedItemsOutput.RemoveAt(0);
                OutputItem = null;
                return;
            }
        }
        return;
    }

    //Check if there is converyor belt at output
    private bool CheckForLogistics()
    {
        Vector2 LocationCheck;
        switch (TileRotation)
        {
            case buildLayer.Rotations.Up:
                LocationCheck = new Vector2(OutputSubTile.x, OutputSubTile.y - 1);
                break;
            case buildLayer.Rotations.Right:
                LocationCheck = new Vector2(OutputSubTile.x + 1, OutputSubTile.y);
                break;
            case buildLayer.Rotations.Down:
                LocationCheck = new Vector2(OutputSubTile.x, OutputSubTile.y + 1);
                break;
            case buildLayer.Rotations.Left:
                LocationCheck = new Vector2(OutputSubTile.x - 1, OutputSubTile.y);
                break;
            default:
                return false;
        }

        if (buildLayer.BuildLayerManager.PlacedTiles.ContainsKey(LocationCheck))
        {
            UpdateConveryorOutput(LocationCheck);
            if (OutputTile == null) return false;
            return true;
        }
        else
        {
            OutputTile = null;
            return false;
        }

    }
}
