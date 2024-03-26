using UnityEngine;

namespace Script.Projectile
{
    public class LinearMotion : ProjectileMotion
    {
        public override void Init(Projectile projectile,IProjectileMotionSetting setting)
        {
            var _setting = setting as LinearMotionSetting; 
            startDirection = new Vector2(projectile.launcher.transform.lossyScale.x,0);
            velocity = startDirection.normalized*_setting.speed;
        }
    }
}