using buildLayer;
using Godot;
using Godot.Collections;

public class PlayerActions : BaseEntity
{
    private EnvironmentalGridManager EnvGridManager;
    private BuildLayerManager BuildManager;
    private AnimatedSprite AniSprite;
    private Rotations currentRotation = Rotations.Up;
    private BaseBuildable SelectedTile;
    private System.Collections.Generic.List<BaseBuildable> BuildablesTiles;
    private Dictionary TileRecipes;
    private Dictionary CurrentSelectedRecipe = null;
    private Vector2 velocity = new Vector2();
    
    public bool IsMouseOverHUD = false;
    public bool IsGUIOpen = false;
    public int RegenRate;

    int cycleItemPosition = 0;
    int cycleItemMax;

    readonly int TimeForStartRegen = 5;
    readonly int RegenTick = 2;
    readonly int RegenPerTick = 2;
    float TimeRemaingForStartRegen = 0;
    float CurrentTickForRegen = 0;
    bool StartRegen = false;

    public override void _Ready()
    {
        MaxHealth = BaseMaxHealth;
        Health = MaxHealth;
        Speed = BaseSpeed;
        RegenRate = 2;

        EnvGridManager = GetNode("../Navigation2D/EnvLayer") as EnvironmentalGridManager;
        BuildManager = GetNode("../Navigation2D/BuildLayer") as BuildLayerManager;
        AniSprite = GetNode("./AnimatedSprite") as AnimatedSprite;

        SceneNode.PlayerProgressManager.Connect("UpdateModifiorsForNodes", this, "UpdateTechNodeModifiors");

        TileRecipes = SceneNode.ProcessingRecipes["Player"];
        BuildablesTiles = SceneNode.BuildTiles.FindAll(tile => tile.IsBuildable == true);
        cycleItemMax = BuildablesTiles.Count - 1;


        UpdateHealthHUD();
        CycleItem();
    }

    public override void _Input(InputEvent @event)
    {
        if (IsGUIOpen) return;

        if (Input.IsActionJustPressed("rotate"))
        {
            bool isRotatable = BuildablesTiles[cycleItemPosition].IsRotatable;
            if (!isRotatable)
            {
                currentRotation = Rotations.Up;
            }
            else
            {
                switch (currentRotation)
                {
                    case Rotations.Up:
                        currentRotation = Rotations.Right;
                        break;
                    case Rotations.Right:
                        currentRotation = Rotations.Down;
                        break;
                    case Rotations.Down:
                        currentRotation = Rotations.Left;
                        break;
                    case Rotations.Left:
                        currentRotation = Rotations.Up;
                        break;
                }
            }
            UpdateRotation(currentRotation);
        }

        if (@event is InputEventMouseButton && !IsMouseOverHUD)
        {
            Vector2 mouseClickLocation = GetGlobalMousePosition();
            Vector2 MouserLocation = EnvGridManager.GetCellFromVector(mouseClickLocation);

            if (Input.IsActionPressed("build"))
            {
                TryToBuild(MouserLocation);
            }

            if (Input.IsActionPressed("Destroy"))
            {
                BuildManager.RemoveTile(MouserLocation);
            }

        }

        if (Input.IsActionJustPressed("cycleLeft"))
        {
            if (cycleItemPosition == 0)
            {
                cycleItemPosition = cycleItemMax;
            }
            else
            {
                cycleItemPosition -= 1;
            }

            CycleItem();
        }
        else if (Input.IsActionJustPressed("cycleRight"))
        {
            if (cycleItemPosition == cycleItemMax)
            {
                cycleItemPosition = 0;
            }
            else
            {
                cycleItemPosition += 1;
            }

            CycleItem();
        }

    }

    public override void TakeDamage(int HitPoints)
    {
        TimeRemaingForStartRegen = 0;
        CurrentTickForRegen = 0;
        StartRegen = false;
        UpdateHealthHUD();
        base.TakeDamage(HitPoints);
    }

    public void UpdateTechNodeModifiors()
    {
        MaxHealth = BaseMaxHealth + (int)PlayerProgressManager.TechTreeNodes["Health"].Modifier;
        Speed = BaseSpeed + (int)PlayerProgressManager.TechTreeNodes["Speed"].Modifier;
        UpdateHealthHUD();
    }

    private void UpdateHealthHUD()
    {
        HUDManager.HealthOverviewHUD.UpdateHealth(MaxHealth, Health);
    }

    //Attempt to build tile
    private void TryToBuild(Vector2 mouseLocation)
    {
        if (!TryToCraft()) return;
        
        if (SelectedTile.Width > 1 || SelectedTile.Height > 1)
        {
            BuildManager.SpawnTile(mouseLocation, currentRotation, SelectedTile.ItemID, SelectedTile, SelectedTile.Width, SelectedTile.Height);
        }
        else
        {
            BuildManager.SpawnTile(mouseLocation, currentRotation, SelectedTile.ItemID, SelectedTile);
        }
    }


    //Check if user has enough resources
    private bool TryToCraft()
    {
        foreach (string RequiredItem in CurrentSelectedRecipe.Keys)
        {
            BaseItemResource item = SceneNode.ResourceItems[RequiredItem];
            int Amount = (int)(float)CurrentSelectedRecipe[RequiredItem];
            if (!SceneNode.MainBase.CheckForEnoughResources(item, Amount)) return false;
        }
        SceneNode.MainBase.UseResourceForRecipe(CurrentSelectedRecipe);
        return true;
    }

    //Get Corresponding Recipe for selected Tile
    private void UpdateRecipeForTile()
    {
        Dictionary TileRecipe = TileRecipes[SelectedTile.TileName.ToString()] as Dictionary;
        CurrentSelectedRecipe = TileRecipe[SelectedTile.Tier.ToString()] as Dictionary;
        HUDManager.CostManagerHUD.UpdateTileCost(CurrentSelectedRecipe);
    }


    private void CycleItem()
    {
        BaseBuildable itemSelected = BuildablesTiles[cycleItemPosition];
        SelectedTile = itemSelected;
        UpdateItemSelected();
        UpdateRecipeForTile();
        currentRotation = Rotations.Up;
        UpdateRotation(currentRotation);
    }

    private void UpdateRotation(Rotations currentRotation)
    {
        HUDManager.SelectedItemHUD.UpdateRotation(currentRotation);
    }

    //Update HUD to show corresponding item
    private void UpdateItemSelected()
    {
        HUDManager.SelectedItemHUD.ChangeText(SelectedTile);
    }

    //When item is clicked on hotbar
    public void ChangeSelectedItem(BaseBuildable tileSelected)
    {
        cycleItemPosition = BuildablesTiles.FindIndex(tile => tile == tileSelected);
        CycleItem();
    }
    private void GetInputForMovement()
    {
        velocity = new Vector2();

        if (Input.IsActionPressed("right")) velocity.x += 1;

        if (Input.IsActionPressed("left")) velocity.x -= 1;

        if (Input.IsActionPressed("down")) velocity.y += 1;

        if (Input.IsActionPressed("up")) velocity.y -= 1;

        velocity = velocity.Normalized() * Speed;
    }

    //When PhysicsProcess Event is called
    public override void _PhysicsProcess(float delta)
    {
        GetInputForMovement();
        if (velocity.x != 0 || velocity.y != 0)
        {
            AniSprite.Play("Run");
        }
        else
        {
            AniSprite.Stop();
        }
        velocity = MoveAndSlide(velocity);

        if (Health < MaxHealth)
        {
            TimeRemaingForStartRegen += delta;
            if (TimeRemaingForStartRegen >= TimeForStartRegen)
            {
                StartRegen = true;
                TimeRemaingForStartRegen = 0;
            }
        }

        if (StartRegen)
        {
            CurrentTickForRegen += delta;
            if (CurrentTickForRegen >= RegenTick)
            {
                CurrentTickForRegen = 0;
                Health += RegenPerTick;
                if (Health >= MaxHealth)
                {
                    Health = MaxHealth;
                    StartRegen = false;
                }
             
                UpdateHealthHUD();
            }
        }
    }
}


