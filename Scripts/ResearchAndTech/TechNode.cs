using System.Collections.Generic;

public class TechNode
{
    public string Name { get; protected set; }
    public int CurrentLevel = 1;
    public float Modifier = 0;
    public float ModifiorIncrement { get; protected set; }
    public int NeededTechForUpgrade { get; protected set; }

    public TechNode(string name, int neededTechForUpgrade, float modifierIncrement)
    {
        Name = name;
        NeededTechForUpgrade = neededTechForUpgrade;
        ModifiorIncrement = modifierIncrement;
    }
}