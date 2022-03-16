using buildLayer;
using Godot;
using Godot.Collections;
public class SceneNode : Node2D
{
    public static System.Collections.Generic.List<BaseBuildable> BuildTiles = new System.Collections.Generic.List<BaseBuildable>(); //For keeping track of all tiles in tilemap
    public static Dictionary<string, BaseItemResource> ResourceItems = new Dictionary<string, BaseItemResource>(); //For keeping track of all items
    public static Dictionary<string, Dictionary> ProcessingRecipes = new Dictionary<string, Dictionary>();
    public static MainBaseTile MainBase;
    public static PlayerProgressManager PlayerProgressManager;
    public static WaveManager WaveManager;
    public BuildLayerManager BuildManager;
    public Navigation2D NavigationNode;
    public PlayerActions Player;

    [Export]
    public Difficulty WorldDifficulty;
    

    private Transform2D PlayerSpawnLocation = new Transform2D(0, new Vector2(100, -100));

    public override void _Ready()
    {
        BuildManager = GetNode("./Navigation2D/BuildLayer") as BuildLayerManager;
        NavigationNode = GetNode("./Navigation2D") as Navigation2D;
        PlayerProgressManager = GetNode("./PlayerProgressManager") as PlayerProgressManager;
        WaveManager = GetNode("./WaveManager") as WaveManager;
        SceneResourceLoader.SceneNode = this;
        SceneResourceLoader.LoadBuildableTiles();
        SceneResourceLoader.LoadItems();
        SceneResourceLoader.LoadRecipes();
        AddStarterResources();
        SpawnPlayer();
        EmitSignal(nameof(LoadIsComplete));
    }

    //Create Player Instance
    private void SpawnPlayer()
    {
        PackedScene playerScene = (PackedScene)ResourceLoader.Load("res://Scenes/Player.tscn");
        Player = (PlayerActions)playerScene.Instance();
        Player.Transform = PlayerSpawnLocation;
        AddChild(Player);
    }

    //Add Starter Resources
    private void AddStarterResources()
    {
        MainBaseTile.StoredResources.Add(ResourceItems[ResourceNames.IronPlate.ToString()], 300);
        MainBaseTile.StoredResources.Add(ResourceItems[ResourceNames.CopperPlate.ToString()], 300);
        MainBaseTile.StoredResources.Add(ResourceItems[ResourceNames.SilverPlate.ToString()], 100);
        MainBaseTile.StoredResources.Add(ResourceItems[ResourceNames.TinPlate.ToString()], 150);
    }

    [Signal]
    delegate void LoadIsComplete();



}
public enum Difficulty
{
    Easy,
    Normal,
    Hard
}