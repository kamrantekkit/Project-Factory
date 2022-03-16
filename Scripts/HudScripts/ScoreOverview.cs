using Godot;
using System;

public class ScoreOverview : Label
{
    private int score;

    public void AddPoints(int points)
    {
        score += points;
        UpdateText();
    }

    private void UpdateText()
    {
        Text = $"Score: {score}";
    }
}
