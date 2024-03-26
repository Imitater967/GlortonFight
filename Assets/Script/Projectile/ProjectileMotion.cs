using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Projectile
{
    public abstract class ProjectileMotion  : MonoBehaviour
    { 
        public Vector2 startDirection;
        public Vector2 velocity; 

        public abstract void Init(Projectile projectile, IProjectileMotionSetting setting);
        protected virtual void FixedUpdate()
        {
            Vector3 originPos = transform.position;
            Vector3 movement = velocity * Time.fixedDeltaTime;
            transform.position = originPos + movement; 
        }
    }
}