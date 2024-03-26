using System;
using Game;
using Script.Character;
using Script.Character.FB;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Projectile
{
    public class BallProjectile: Projectile
    {
        public GameObject spritePrefab;
        public BallProjectileMotionSetting motionSetting;
        private void Awake()
        {
            motion = gameObject.AddComponent<BallProjectileMotion>();
        }

        public override void Init(GlortonFighter fighter, Transform muzzle1)
        {
            base.Init(fighter, muzzle);
            ((BallProjectileMotion)motion).Init(this,motionSetting);
            sprite = Instantiate(spritePrefab, transform);
        }
        
        protected override void OnHitWall()
        {
            motion.velocity.x = -motion.velocity.x;
        }

        protected override void OnHitMechanism(IDamagable component)
        {
        }

        protected override void OnHitPlayer(GlortonFighter glortonFighter)
        {
            EventManager.Instance.Combat.Ball.OnBallRangedAttackSomeone?.Invoke(this, glortonFighter);
            Destroy(gameObject);
        }
    }
}