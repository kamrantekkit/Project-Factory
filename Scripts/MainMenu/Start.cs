using Godot;
using System;

public class Start : BaseButton
{
    public void OnPressed()
    {
        audioStreamPlayer.Play();
        GetTree().ChangeScene("res://Scenes/MainMenu/DifficultySelection.tscn");
    }

}
