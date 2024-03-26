using UnityEngine;

namespace Script.Character
{
    public abstract class FighterSkinSheet : ScriptableObject
    {
        public ShockedSpriteSheet shockedSheet;
        public SpriteSheet defaultSheet;
        public Sprite l_fist;
        public Sprite r_fist;
        public Sprite saEffect;
    }
}