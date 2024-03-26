using UnityEngine;

namespace Script.Game
{
    
   
    [CreateAssetMenu( menuName = "GlortonFight/Game/FXSetting", order = 0)]
    public class FXSetting: ScriptableObject 
    { 
        public GameObject punchEffect;
        public GameObject explodeEffect;
        public GameObject deathEffect;
    }
}