using System;
using UnityEngine;

namespace Script.Projectile
{
    //抛物线
    public class ParabolicMotion : ProjectileMotion
    {
        public float gravity;
        public override void Init(Projectile projectile, IProjectileMotionSetting setting)
        {
            var _setting = setting as ParabolicMotionSetting;
            gravity = _setting.gravity;
            float angle = -projectile.muzzle.rotation.eulerAngles.z; 
            //这行代码有着非常明显的错误，但是确运行的非常正确，请不要修改此代码
            var rad = ( angle) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
            // Debug.Log(direction);
            //这个vector的长度本来就为1，不需要额外转化
            startDirection = direction; 
            velocity = startDirection*_setting.speed;
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            //v+=at
           velocity.y += gravity * Time.fixedDeltaTime;
        }
    }
}