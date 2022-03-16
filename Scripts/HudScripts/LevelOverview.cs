using Godot;
using System;

public class LevelOverview : Label
{
    private ProgressBar ExprienceLevelBar;
    public override void _Ready()
    {
        ExprienceLevelBar = GetNode("../ExprienceLevelBar") as ProgressBar;
    }

    public void UpdateText(int CurrentLevel, int CurrentExprience, int MaxExprience)
    {
        Text = $"Level {CurrentLevel}: {CurrentExprience}/{MaxExprience}Exp";
        ExprienceLevelBar.MaxValue = MaxExprience;
        ExprienceLevelBar.Value = CurrentExprience;
    }
}
