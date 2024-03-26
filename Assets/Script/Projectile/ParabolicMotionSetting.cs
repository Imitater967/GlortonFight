using UnityEngine;

namespace Script.Projectile
{
    
    [CreateAssetMenu( menuName = "GlortonFight/Projectile/ParabolicMotionSetting", order = 0)]
    public class ParabolicMotionSetting: ScriptableObject,IProjectileMotionSetting
    {
        public float speed=1.5f; 
        public float gravity=-2f;
    }
}