using Godot;
using System;

public class ScoreOverview : Label
{
    public int score;

    private float ScoreMultiplier;

    public override void _Ready()
    {
        SceneNode sceneNode = GetNode("/root/SceneNode") as SceneNode;
        switch (sceneNode.WorldDifficulty)
        {
            case Difficulty.Easy:
                ScoreMultiplier = 0.8F;
                break;
            case Difficulty.Normal:
                ScoreMultiplier = 1;
                break;
            case Difficulty.Hard:
                ScoreMultiplier = 1.5F;
                break;
        }
    }
 
    public void AddPoints(int points)
    {
        score += (int)(points * ScoreMultiplier);
        UpdateText();
    }

    private void UpdateText()
    {
        Text = $"Score: {score}";
    }
}