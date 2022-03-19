using Godot;
using System;

public class ReturnToMenu : BaseButton
{ 
    public void OnPressed()
    {
        audioStreamPlayer.Play();
        GetTree().ChangeScene("res://Scenes/MainMenu/MainMenuSelection.tscn");
    }
}       
