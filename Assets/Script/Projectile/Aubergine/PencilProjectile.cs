using Game;
using Script.Character;
using UnityEngine;

namespace Script.Projectile.Aubergine
{
    public class PencilProjectile: Projectile
    {
        public LinearMotionSetting motionSetting;
        private void Awake()
        {
            motion = gameObject.AddComponent<LinearMotion>();
        }

        public override void Init(GlortonFighter fighter, Transform muzzle1)
        {
            base.Init(fighter, muzzle); 
            (motion).Init(this,motionSetting); 
        }
        
        protected override void OnHitPlayer(GlortonFighter glortonFighter)
        {
            EventManager.Instance.Combat.Aubergine.OnAubergineRangedAttackSomeone?.Invoke(this, glortonFighter);
            Destroy(gameObject);
        }
    }
}