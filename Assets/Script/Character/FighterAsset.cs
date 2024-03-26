using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.Character
{
    
    [CreateAssetMenu(fileName = "Character", menuName = "GlortonFight/Character/New Fighter Asset", order = 0)]
    public class FighterAsset: ScriptableObject
    {
        [FormerlySerializedAs("fighter")] public GlortonFighter Fighter;
        [FormerlySerializedAs("skin")] public FighterSkinSheet Skin;
        [FormerlySerializedAs("preview")] public Sprite Preview;
        [NonSerialized]
        public bool Selected; 
        [NonSerialized] 
        public FighterType Type;
        [NonSerialized]
        public byte Index;

        public Sprite HealthSlot;
        [FormerlySerializedAs("Thumbnail")] public Sprite HealthBarBG;
    }
}