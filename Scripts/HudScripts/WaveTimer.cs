using Godot;
using System;

public class WaveTimer : Label
{
    public void UpdateTime(int TimeRemaining)
    {
        Text = $" Next Wave in \n" +
            $"{TimeRemaining}";
    }
}
