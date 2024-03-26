using UnityEngine;

namespace Script.Projectile
{
    
    [CreateAssetMenu( menuName = "GlortonFight/Projectile/LinearMotionSetting", order = 0)]
    public class LinearMotionSetting: ScriptableObject,IProjectileMotionSetting
    { 
        public float speed=1.5f;
    }
}