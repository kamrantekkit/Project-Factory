using Godot;

public class BaseEntity : KinematicBody2D
{
    [Export] public int BaseMaxHealth { get; set; }
    [Export] public int BaseSpeed { get; set; }
    public int MaxHealth { get; set; }
    public int Speed { get; set; }
    public int Health { get; set; }


    public virtual void TakeDamage(int HitPoints)
    {
        Health -= HitPoints;
    }

    protected virtual void Destroy()
    {
        QueueFree();
    }
}