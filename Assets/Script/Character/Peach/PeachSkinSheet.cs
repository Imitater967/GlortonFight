using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Character.Peach
{
    [CreateAssetMenu(fileName = "Character", menuName = "GlortonFight/Character/New Peach Skin", order = 0)]
    public class PeachSkinSheet: FighterSkinSheet
    { 
        [FormerlySerializedAs("l_shotHand")] public Sprite r_shotHand; 
    }
}