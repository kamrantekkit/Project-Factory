using Godot;
using System.Collections.Generic;

public class WeaponNode : Node2D
{
    private LaserEmitter Weapon;
    private List<BasicEnemy> NearbyEnemies = new List<BasicEnemy>();
    private BasicEnemy CurrentTarget = null;
    private int BaseDamage = 5;
    private int Damage = 0;
    private float TimeDelta = 0;
    private bool IsAttacking = false;
    public override void _Ready()
    {
        Weapon = GetNode("./Emitter") as LaserEmitter;
        SceneNode.PlayerProgressManager.Connect("UpdateModifiorsForNodes", this, "UpdateTechNodeModifiors");

        Damage = BaseDamage;
    }

    public void UpdateTechNodeModifiors()
    {
        Damage = BaseDamage + (int)PlayerProgressManager.TechTreeNodes["Damage"].Modifier;
        GD.Print(Damage);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (IsAttacking)
        {
            if (!NearbyEnemies.Contains(CurrentTarget))
            {
                if(!GetNewTarget()) return;
            }

            RotateToPoint(CurrentTarget.GlobalPosition);
            if (!Weapon.IsCasting) AttackEnemy(); 

            if (TimeDelta >= 1 && NearbyEnemies.Contains(Weapon.HitEntity))
            {
                Weapon.HitEntity.WasHit(Damage);
                TimeDelta = 0;
            }
        }
        TimeDelta += delta;
    }

    private bool GetNewTarget()
    {
        if (CurrentTarget != null || NearbyEnemies.Count == 0)
        {
            ResetWeapon();
            return false;
        }
        CurrentTarget = NearbyEnemies[0];
        return true;
    }
    private void AttackEnemy()
    {  
        Weapon.UpdateCasting(true);
    }

    private void ResetWeapon()
    {
        IsAttacking = false;
        Weapon.UpdateCasting(false);
        Rotation = 0;
    }
    public void OnBodyEntered(Node Body)
    {
        if (Body is BasicEnemy)
        {
            NearbyEnemies.Add((BasicEnemy)Body);
        }

    }
    public void OnBodyExited(Node Body)
    {
        if (Body is BasicEnemy)
        {
            NearbyEnemies.Remove((BasicEnemy)Body);
            if (Body == CurrentTarget) CurrentTarget = null;
            if (NearbyEnemies.Count == 0) ResetWeapon();
        }
    }


    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Attack"))
        {
            IsAttacking = true;
        }
        if (@event.IsActionReleased("Attack"))
        {
            ResetWeapon();
        }
    }

    private void RotateToPoint(Vector2 point)
    {
        LookAt(point);
    }
}
