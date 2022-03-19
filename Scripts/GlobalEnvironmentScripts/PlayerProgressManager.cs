using Godot;
using System.Collections.Generic;

public class PlayerProgressManager : Node2D
{
    public static Dictionary<string, TechNode> TechTreeNodes = new Dictionary<string, TechNode>(); 
    private int CurrentLevel = 1;
    private int ExpToNextLevel = 100;
    private int CurrentEXPAmount = 0;

    private float EXPMultiplier;
    private int LevelCapStep = 15;
    private int TechPoints = 0;

    public override void _Ready()
    {
        SceneNode sceneNode = GetParent() as SceneNode;
        sceneNode.Connect("LoadIsComplete", this, nameof(OnLoadComplete));

        switch (sceneNode.WorldDifficulty)
        {
            case Difficulty.Easy:
                EXPMultiplier = 2;
                break;
            case Difficulty.Normal:
                EXPMultiplier = 1;
                break;
            case Difficulty.Hard:
                EXPMultiplier = 0.5F;
                break;
        }
        SetupTechNodeTree();
    }

    public void OnLoadComplete()
    {
        UpdateExprienceHUD();
    }

    public void AddExprience(int exprience)
    {
        HUDManager.ScoreOverviewHUD.AddPoints(exprience);
        CurrentEXPAmount += (int)(exprience * EXPMultiplier);
        if (CurrentEXPAmount >= ExpToNextLevel)
        {
            CurrentLevel++;
            TechPoints++;
            int ExcessExp = CurrentEXPAmount - ExpToNextLevel;
            CurrentEXPAmount = ExcessExp;
            if (CurrentLevel % 5 == 0)
            {
               LevelCapStep = (int)(LevelCapStep * 1.2F);
                GD.Print($"Milestone reached, Increasing by {LevelCapStep}");
            }
            GetTree().CallGroup("TechUpgradesHUD", "UpdateGUI");
            EmitSignal(nameof(UpdateTechAmountGUI), TechPoints);
            ExpToNextLevel += LevelCapStep;
        }

        UpdateExprienceHUD();
    }

    private void UpdateExprienceHUD()
    {
        HUDManager.LevelOverviewHUD.UpdateText(CurrentLevel, CurrentEXPAmount, ExpToNextLevel);
    }

    public void UpgradeTech(TechNode techNode)
    {
        GD.Print(techNode.Name);
        if (techNode.NeededTechForUpgrade > TechPoints) return;
        TechPoints -= techNode.NeededTechForUpgrade;
        techNode.Modifier += techNode.ModifiorIncrement;
        techNode.CurrentLevel++;
        GetTree().CallGroup("TechUpgradesHUD", "UpdateGUI");
        EmitSignal(nameof(UpdateTechAmountGUI), TechPoints);
        EmitSignal(nameof(UpdateModifiorsForNodes));
    }

    private void SetupTechNodeTree()
    {
        CreateTechNode("Damage", 2, 4);
        CreateTechNode("Health", 1, 8);
        CreateTechNode("Speed", 1, 2);

    }
    private void CreateTechNode(string name, int TechForUpgrade, float modifierIncrement)
    {
        TechNode techNode = new TechNode(name, TechForUpgrade, modifierIncrement);
        TechTreeNodes.Add(name, techNode);
    }

    [Signal]
    delegate void UpdateTechAmountGUI(int techpoints);

    [Signal]
    delegate void UpdateModifiorsForNodes();
}

