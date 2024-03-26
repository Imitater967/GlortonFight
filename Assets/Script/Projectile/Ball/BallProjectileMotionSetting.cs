using UnityEngine;

namespace Script.Projectile
{
    
    [CreateAssetMenu( menuName = "GlortonFight/Projectile/BallMotionSetting", order = 0)]
    public class BallProjectileMotionSetting : LinearMotionSetting
    {
        public float A;

        public float T;
    }
}