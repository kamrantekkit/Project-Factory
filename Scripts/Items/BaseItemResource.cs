using Godot;
using ItemType;

public class BaseItemResource : BaseItem
{

    public ResourceNames Name { get; protected set; }
    public int Exprience { get; protected set; }
    public bool IsFuel;
    public bool IsMaterial;

    public BaseItemResource(ResourceNames name, TierType tier, string spriteResourcePath, int exprience = 0, bool isMaterial = false, bool isfuel = false) : base(tier)
    {
        Name = name;
        IsFuel = isfuel;
        IsMaterial = isMaterial;
        Exprience = exprience;
        ItemSpriteTexture = (Texture)ResourceLoader.Load(spriteResourcePath);
    }
}

public enum ResourceNames
{
    IronOre,
    CopperOre,
    CoalOre,
    TinOre,
    SilverOre,
    IronPlate,
    CopperPlate,
    TinPlate,
    SilverPlate,

}