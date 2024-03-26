using System;
using UnityEngine;

namespace Script.Character
{


    [CreateAssetMenu( menuName = "GlortonFight/Character/Shocked", order = 0)]
    public class ShockedSpriteSheet : ScriptableObject
    {
        public SpriteSheet tick0;
        public SpriteSheet tick1;
    }
}