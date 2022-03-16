using Godot;

public class HUDManager : Control
{
    public static SelectedItemHud SelectedItemHUD;
    public static PowerOverview PowerOverviewHUD;
    public static HealthOverview HealthOverviewHUD;
    public static LevelOverview LevelOverviewHUD;
    public static CostHUDManager CostManagerHUD;
    public static ScoreOverview ScoreOverviewHUD;
    public static WaveTimer WaveTimerHUD;
    public static PlayerActions Player;
    public static SceneNode SceneNode;
    public override void _Ready()
    {
        SelectedItemHUD = GetNode("./VBoxContainer/SelectedItemHud") as SelectedItemHud;
        PowerOverviewHUD = GetNode("./EnergyContainer/PowerOverview") as PowerOverview;
        HealthOverviewHUD = GetNode("./HealthContainer/Health") as HealthOverview;
        CostManagerHUD = GetNode("./SelectedItemCost") as CostHUDManager;
        LevelOverviewHUD = GetNode("./CurrentLevelContainer/VBoxContainer2/LevelOverview") as LevelOverview;
        ScoreOverviewHUD = GetNode("./ScoreOverview") as ScoreOverview;
        WaveTimerHUD = GetNode("./WaveTimer") as WaveTimer;



        Player = GetParent().GetParent() as PlayerActions;
    }
}
