using Game;
using Script.Character;
using UnityEngine;

namespace Script.Projectile.Trash
{
    public class TrashProjectile: Projectile
    {
        public ParabolicMotionSetting motionSetting;
        private void Awake()
        {
            motion = gameObject.AddComponent<ParabolicMotion>();
        }

        public override void Init(GlortonFighter fighter, Transform muzzle1)
        {
            base.Init(fighter, muzzle1);
            ((ParabolicMotion)motion).Init(this,motionSetting); 
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void OnHitPlayer(GlortonFighter glortonFighter)
        {
            //碰到玩家产生爆炸
         EventManager.Instance.Combat.Trash.OnTrashRangedAttackSomeone?.Invoke(this,glortonFighter); 
            Destroy(gameObject);
        }
    }
}