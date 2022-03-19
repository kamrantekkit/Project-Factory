using Godot;
using System;

public class GameOver : Panel
{
    private Label WavesSurvived;
    private Label Score;

    public override void _Ready()
    {
        WavesSurvived = GetNode("./MarginContainer/WavesSurvived") as Label;
        Score = GetNode("./VBoxContainer/Score") as Label;
    }

    public void GameSummary(int PlayerScore, int Waves)
    {
        Score.Text = $"Score: {PlayerScore}";
        WavesSurvived.Text = $"You have Survived {Waves} Waves";
    } 

}
