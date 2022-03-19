using Godot;
using System;

public class Quit : BaseButton
{
    public void OnPressed()
    {
        audioStreamPlayer.Play();
        GetTree().Quit();  
    }
}
