using UnityEngine;

namespace Script.Game
{
    [CreateAssetMenu( menuName = "GlortonFight/Game/AudioSetting", order = 0)]
    public class SoundSetting : ScriptableObject 
    {
        public float Volume=1; 
        public AudioAsset[] attackAudio;
        public AudioAsset boom;
        [Header("Characters-StrawBerry")] 
        public AudioAsset strawBerrySpecialAttack; 
        public AudioAsset strawBerryRangedAttack;
        [Header("Characters-Ball")] 
        public AudioAsset ballSpecialAttack; 
        public AudioAsset ballRangedAttackSomeone;
        [Header("Characters-Peach")] 
        public AudioAsset peachRocketLaunch;
        public AudioAsset peachGunFire;
        [Header("Characters-Aubergine")] 
        public AudioAsset aubergineSpecialAttack;
        [Header("Characters-Aubergine")]
        public AudioAsset coffeeSpecialAttack;
        public AudioAsset[] coffeeFart;
    }
}