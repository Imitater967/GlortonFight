using Game;
using Script.Character;
using UnityEngine;

namespace Script.Projectile.Peach
{
    public class PeachRocket: Projectile
    {
        public BoxCollider2D range;
        public Transform head;
        public ParabolicMotionSetting motionSetting;
        private void Awake()
        {
            motion = gameObject.AddComponent<ParabolicMotion>();
            range = gameObject.GetComponent<BoxCollider2D>();
        }

        public override void Init(GlortonFighter fighter, Transform muzzle1)
        {
            base.Init(fighter, muzzle1);
            ((ParabolicMotion)motion).Init(this,motionSetting); 
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            base.AlignRotationByVelocity();
        }

        protected override Collider2D CheckTarget()
        {
            Vector2 startPos = transform.position; 
            Vector2 nextPos = head.position;
            RaycastHit2D hit = Physics2D.Linecast(startPos, nextPos, checkMask);
            Debug.DrawLine(startPos,nextPos,Color.red);
            return   hit.collider ;
        }

        protected override void OnHitWall()
        {
        }

        protected override void OnHitPlayer(GlortonFighter glortonFighter)
        {
            //碰到玩家产生爆炸
            EventManager.Instance.Combat.Peach.OnPeachRocketExplode?.Invoke(this); 
            Destroy(gameObject);
        }
    }
}