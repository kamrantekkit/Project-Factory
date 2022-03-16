using Godot;

namespace ItemType
{
    public class BaseItem : Object
    {

        public TierType Tier { get; protected set; }
        public Texture ItemSpriteTexture { get; protected set; }

        public BaseItem(TierType tier)
        {
            Tier = tier;
        }
    }
    public enum TierType
    {
        Basic,
        Advanced
    }
}


