using Godot;

public class LaserEmitter : RayCast2D
{
    public bool IsCasting { get => _IsCasting; set => UpdateCasting(value); }
    public bool _IsCasting = false;
    public BasicEnemy HitEntity = null;
    private Line2D LineFill;
    private Tween LineTween;
    private Particles2D CollidingParticlesEmitter;


    public override void _Ready()
    {
        SetPhysicsProcess(false);

        LineFill = GetNode("./Line2D") as Line2D;
        LineTween = GetNode("./Tween") as Tween;
        CollidingParticlesEmitter = GetNode("./CollidingParticlesEmitter") as Particles2D;
        LineFill.SetPointPosition(1, Vector2.Zero);
    }
    public override void _PhysicsProcess(float delta)
    {
        Vector2 CastPoint = CastTo;
        ForceRaycastUpdate();

        CollidingParticlesEmitter.Emitting = IsColliding();

        if (IsColliding())
        {
            CastPoint = ToLocal(GetCollisionPoint());
            CollidingParticlesEmitter.GlobalRotation = GetCollisionNormal().Angle();
            CollidingParticlesEmitter.Position = CastPoint;
            Node CollidedEntity = (Node)GetCollider();
            if (CollidedEntity != null)
            {
                HitEntity = (BasicEnemy)CollidedEntity;
            }
        }

        LineFill.SetPointPosition(1, CastPoint);
    }

    public void UpdateCasting(bool Casting)
    {
        _IsCasting = Casting;
        if (Casting)
        {
            Appear();
        }
        else
        {
            CollidingParticlesEmitter.Emitting = Casting;
            Dissapear();
            LineFill.SetPointPosition(1, Vector2.Zero);

        }
        SetPhysicsProcess(Casting);
    }

    private void Appear()
    {
        LineTween.StopAll();
        LineTween.InterpolateProperty(LineFill, "width", 0, 4, 0.2F);
        LineTween.Start();
    }

    private void Dissapear()
    {
        LineTween.StopAll();
        LineTween.InterpolateProperty(LineFill, "width", 4, 0, 0.1F);
        LineTween.Start();
    }
}
