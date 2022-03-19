using Godot;
using System;

public class BaseButton : Button
{
    protected AudioStreamPlayer audioStreamPlayer;
    public override void _Ready()
    {
        audioStreamPlayer = GetNode("/root/AudioPlayerMenu") as AudioStreamPlayer;
    }
}