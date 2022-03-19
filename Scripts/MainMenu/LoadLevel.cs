using Godot;
using System;

public class LoadLevel : BaseButton
{
    public void OnPressed()
    {
        audioStreamPlayer.Play();
        GetTree().ChangeScene($"res://Scenes/Difficulties/{Name}.tscn");
    }
}
