using Godot;
using System;

public class Help : BaseButton
{

    public void OnPressed()
    {
        audioStreamPlayer.Play();
        GetTree().ChangeScene("res://Scenes/MainMenu/HelpMenu.tscn");
    }
}
