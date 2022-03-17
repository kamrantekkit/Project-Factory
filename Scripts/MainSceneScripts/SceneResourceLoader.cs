using Godot;
using Godot.Collections;
using ItemType;
public static class SceneResourceLoader
{
    public static SceneNode SceneNode { get; set; }
    private static int LoadedTilesAmount = 0;
    private static int CollisionExtentSize = 8;


    //Load tiles in to TileSet
    public static void LoadBuildableTiles()
    {
        //Create Main Base Structure
        CreateMainBase("res://Assets/Machine/MainBase/MainBase.tres");

        //Create Basic Converyor
        CreateConveryorTile(TileTypes.Converyor, TierType.Basic, true, "res://Assets/Machine/Logistics/ConveryorStraight/Basic/ConveryorStraightBasic.tres", ConveryorDirectionType.Straight);
        CreateConveryorTile(TileTypes.Converyor, TierType.Basic, false, "res://Assets/Machine/Logistics/ConveryorTurn/LeftConnector/Basic/ConveryorLeftBasic.tres", ConveryorDirectionType.LeftTurn);
        CreateConveryorTile(TileTypes.Converyor, TierType.Basic, false, "res://Assets/Machine/Logistics/ConveryorTurn/RightConnector/Basic/ConveryorRightBasic.tres", ConveryorDirectionType.RightTurn);
        CreateConveryorTile(TileTypes.Converyor, TierType.Basic, false, "res://Assets/Machine/Logistics/ConveryorIntersection/Basic/ConveryorIntersectionBasic.tres", ConveryorDirectionType.Intersection);

        //Create Buildable Drills
        CreateDrill(TileTypes.Drill, TierType.Basic, true, "res://Assets/Machine/Drills/Basic/BasicDrill.tres");

        //Create Smelter
        CreateSmelter(TileTypes.Smelter, TierType.Basic, true, "res://Assets/Machine/Smelter/Basic/BasicSmelter.tres");

        //Create Fuel Burning Generator
        CreateFuelBurningGen(TileTypes.FuelBurningGenerator, TierType.Basic, true, "res://Assets/Machine/Generator/FuelBurning/Basic/BasicGenerator.tres");
    }

    private static void CreateTileWithCollision(AnimatedTexture animatedTexture, BaseBuildable Tile)
    {
        SceneNode.BuildManager.TileSet.CreateTile(LoadedTilesAmount);
        SceneNode.BuildManager.TileSet.TileSetTexture(LoadedTilesAmount, animatedTexture);
        RectangleShape2D CollisionShape = new RectangleShape2D();
        CollisionShape.Extents = new Vector2(CollisionExtentSize * Tile.Width, CollisionExtentSize*Tile.Height);
        SceneNode.BuildManager.TileSet.TileAddShape(LoadedTilesAmount, CollisionShape, new Transform2D(0, new Vector2 (CollisionExtentSize * Tile.Width, CollisionExtentSize * Tile.Height)));
    }

    //Generate a Converyor Tile 
    private static void CreateConveryorTile(TileTypes tileName, TierType tier, bool isBuildable, string textureResourcePath, ConveryorDirectionType converyorDirection)
    {
        AnimatedTexture animatedTexture = (AnimatedTexture)ResourceLoader.Load(textureResourcePath);
        SceneNode.BuildManager.TileSet.CreateTile(LoadedTilesAmount);
        SceneNode.BuildManager.TileSet.TileSetTexture(LoadedTilesAmount, animatedTexture);
        Converyor generatedTile = new Converyor(tileName, isBuildable, LoadedTilesAmount, tier, converyorDirection, animatedTexture);
        if (converyorDirection != ConveryorDirectionType.Straight)
        {
            Converyor MainConveryor = (Converyor)SceneNode.BuildTiles.Find(tileItem => tileItem.TileName == tileName && tileItem.Tier == tier);
            switch (converyorDirection)
            {
                case ConveryorDirectionType.LeftTurn:
                    MainConveryor.LeftTurn = generatedTile;
                    break;
                case ConveryorDirectionType.RightTurn:
                    MainConveryor.RightTurn = generatedTile;
                    break;
                case ConveryorDirectionType.Intersection:
                    MainConveryor.InterSection = generatedTile;
                    break;
            }
        }
        SceneNode.BuildTiles.Add(generatedTile);
        LoadedTilesAmount++;
    }

    //Generate a Drill Tile
    private static void CreateDrill(TileTypes tileName, TierType tier, bool isBuildable, string textureResourcePath)
    {
        AnimatedTexture animatedTexture = (AnimatedTexture)ResourceLoader.Load(textureResourcePath);
        MachineDrill generatedTile = new MachineDrill(tileName, isBuildable, LoadedTilesAmount, tier, animatedTexture);
        SceneNode.BuildTiles.Add(generatedTile);
        CreateTileWithCollision(animatedTexture, generatedTile);
        LoadedTilesAmount++;
    }

    //Generate a Smelter Tile
    private static void CreateSmelter(TileTypes tileName, TierType tier, bool isBuildable, string textureResourcePath)
    {
        AnimatedTexture animatedTexture = (AnimatedTexture)ResourceLoader.Load(textureResourcePath);
        Smelter generatedTile = new Smelter(tileName, isBuildable, LoadedTilesAmount, tier, animatedTexture);
        SceneNode.BuildTiles.Add(generatedTile);
        CreateTileWithCollision(animatedTexture, generatedTile);
        LoadedTilesAmount++;
    }


    //Generate a MainBase Tile
    private static void CreateMainBase(string textureResourcePath)
    {
        AnimatedTexture animatedTexture = (AnimatedTexture)ResourceLoader.Load(textureResourcePath);
        MainCore generatedTile = new MainCore(TileTypes.MainBase, false, LoadedTilesAmount, TierType.Basic, animatedTexture);
        SceneNode.BuildTiles.Add(generatedTile);
        CreateTileWithCollision(animatedTexture, generatedTile);
        LoadedTilesAmount++;
    }

    //Generate a Fuel Burning Generator Tile
    private static void CreateFuelBurningGen(TileTypes tileName, TierType tier, bool isBuildable, string textureResourcePath)
    {
        AnimatedTexture animatedTexture = (AnimatedTexture)ResourceLoader.Load(textureResourcePath);
        FuelBurningGen generatedTile = new FuelBurningGen(tileName, isBuildable, LoadedTilesAmount, tier, animatedTexture);
        SceneNode.BuildTiles.Add(generatedTile);
        CreateTileWithCollision(animatedTexture, generatedTile);
        LoadedTilesAmount++;
    }
    public static void LoadItems()
    {
        //Ores
        AddItem(ResourceNames.IronOre, TierType.Basic, "res://Assets/ItemResources/RawOres/IronOre.png");
        AddItem(ResourceNames.CoalOre, TierType.Basic, "res://Assets/ItemResources/RawOres/CoalOre.png", 0, false, true);
        AddItem(ResourceNames.CopperOre, TierType.Basic, "res://Assets/ItemResources/RawOres/CopperOre.png");
        AddItem(ResourceNames.SilverOre, TierType.Basic, "res://Assets/ItemResources/RawOres/SilverOre.png");
        AddItem(ResourceNames.TinOre, TierType.Basic, "res://Assets/ItemResources/RawOres/TinOre.png");

        //Processed Metals
        AddItem(ResourceNames.IronPlate, TierType.Basic, "res://Assets/ItemResources/ProcessedItems/IronPlate.png", Exprience: 2, isMaterial: true);
        AddItem(ResourceNames.CopperPlate, TierType.Basic, "res://Assets/ItemResources/ProcessedItems/CopperPlate.png", Exprience: 2, isMaterial: true);
        AddItem(ResourceNames.SilverPlate, TierType.Basic, "res://Assets/ItemResources/ProcessedItems/SilverPlate.png", Exprience: 6, isMaterial: true);
        AddItem(ResourceNames.TinPlate, TierType.Basic, "res://Assets/ItemResources/ProcessedItems/TinPlate.png", Exprience: 4, isMaterial: true);
    }

    private static void AddItem(ResourceNames itemName, TierType tier, string textureResourcePath, int Exprience = 0, bool isMaterial = false, bool isFuel = false)
    {
        BaseItemResource generatedItem = new BaseItemResource(itemName, tier, textureResourcePath, Exprience,isMaterial, isFuel);
        SceneNode.ResourceItems.Add(itemName.ToString(), generatedItem);
    }

    public static void LoadRecipes()
    {
        RecipeLoader("res://GameProperties/Recipes/SmelterRecipes.json", TileTypes.Smelter.ToString());
        RecipeLoader("res://GameProperties/Recipes/BurnableFuels.json", TileTypes.FuelBurningGenerator.ToString());
        RecipeLoader("res://GameProperties/Recipes/TileRecipes.json", "Player");
    }

    private static void RecipeLoader(string directory, string recipeType)
    {
        File JSONRecipes = new File();
        JSONRecipes.Open(directory, File.ModeFlags.Read);
        Dictionary ParsedJSONRecipes = JSON.Parse(JSONRecipes.GetAsText()).Result as Dictionary;
        SceneNode.ProcessingRecipes.Add(recipeType, ParsedJSONRecipes);
        JSONRecipes.Close();
    }
}




