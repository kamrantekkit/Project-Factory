using Godot;
using Godot.Collections;

public class BasicEnemy : BaseEntity
{
    private Navigation2D NavigationNode;
    private SceneNode SceneNode;
    private PlayerActions Player;
    private Array<Vector2> CurrentPath = new Array<Vector2>();
    private Vector2 Velocity = Vector2.Zero;
    private bool IsPlayerNear = false;

    public int BaseDamage = 5;
    public int Damage;
    private readonly int DamageTick = 1;
    private float DamageTillTick = 0;
    public override void _Ready()
    {
        SceneNode = (SceneNode)GetParent();
        NavigationNode = (Navigation2D)GetNode("../Navigation2D");
        Health = BaseMaxHealth;
        Speed = BaseSpeed;
        Damage = BaseDamage;
    }

    public override void _PhysicsProcess(float delta)
    {           
        if (SceneNode.Player == null) return;
        float DistanceSpeed = Speed * delta;
        Velocity = Vector2.Zero;

        if (IsPlayerNear)
        {

            if (Player == null)
            {
                Velocity = Position.DirectionTo(SceneNode.Player.Position) * Speed;
                MoveAndSlide(Velocity);
            }
            if (Player != null)
            {
                DamageTillTick += delta;
                if (DamageTillTick >= DamageTick)
                {
                    DamageTillTick = 0;
                    Player.TakeDamage(Damage);
                }
            }
            CurrentPath = new Array<Vector2>();
            return;
        }

        if (CurrentPath.Count == 0)
        {
            GenerateNewPath();
        }

        MoveAlongPath(DistanceSpeed);
    }

    public void WasHit(int Damage)
    {
        Health -= Damage;
        if (Health <= 0) Destroy();
    }

    protected override void Destroy()
    {
        SceneNode.WaveManager.OnEnemyDestroy(this);
        base.Destroy();
    }

    private void MoveAlongPath(float DistanceSpeed)
    {
        Vector2 StartPoint = Position;
        while (CurrentPath.Count > 0)
        {
           float DistanceToNext = StartPoint.DistanceTo(CurrentPath[0]);
            if (DistanceSpeed <= DistanceToNext && DistanceToNext >= 0)
            {
                Position = StartPoint.LinearInterpolate(CurrentPath[0], DistanceSpeed / DistanceToNext);
                return;
            } else if (DistanceSpeed <= 0) {
                Position = CurrentPath[0];
                return;
            }
            DistanceSpeed -= DistanceToNext;
            StartPoint = CurrentPath[0];
            CurrentPath.RemoveAt(0);
        }
    }
    private void GenerateNewPath()
    {
        Vector2[] GeneratedPath = NavigationNode.GetSimplePath(GlobalPosition, SceneNode.Player.GlobalPosition) ;
        if (GeneratedPath.Length == 0) return;
        CurrentPath = new Array<Vector2>(GeneratedPath);
    }

    public void PlayerEnteredFollowRange(Node Body)
    {
        if (Body is PlayerActions)
        {
            IsPlayerNear = true;
        }

    }

    public void PlayerExitedFollowRange(Node Body)
    {
        if (Body is PlayerActions)
        {
            IsPlayerNear = false;
        }
    }

    public void EntityEnteredAttackRange(Node Body)
    {
        if (Body is PlayerActions)
        {
            Player = (PlayerActions)Body;
        }
    }

    public void EntityExitedAttackRange(Node Body)
    {
        if (Body is PlayerActions)
        {
            Player = null;
            DamageTillTick = 0;
        }
    }
}
