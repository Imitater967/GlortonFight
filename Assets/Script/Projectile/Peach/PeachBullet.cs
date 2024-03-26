using Game;
using Script.Character;
using UnityEngine;

namespace Script.Projectile.Peach
{
    public class PeachBullet:Projectile
    {
        public LinearMotionSetting motionSetting;
        private void Awake()
        {
            motion = gameObject.AddComponent<LinearMotion>();
        }
        public override void Init(GlortonFighter fighter, Transform muzzle1)
        {
            base.Init(fighter, muzzle);
            ((LinearMotion)motion).Init(this,motionSetting); 
        }
        protected override void OnHitWall()
        {
            Destroy(gameObject); 
        } 
        protected override void OnHitPlayer(GlortonFighter glortonFighter)
        {            
            EventManager.Instance.Combat.Peach.OnPeachBulletShootSomeone?.Invoke(this, glortonFighter);
            Destroy(gameObject);
        }
    }
}