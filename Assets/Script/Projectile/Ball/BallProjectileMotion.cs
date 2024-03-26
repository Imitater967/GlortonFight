using UnityEngine;

namespace Script.Projectile
{
    public class BallProjectileMotion : LinearMotion
    {
        [SerializeField]
        protected float A;
        [SerializeField]
        protected float T;

        [SerializeField] protected Vector2 startPos;
        public override void Init(Projectile projectile,IProjectileMotionSetting iset)
        {
            base.Init(projectile,iset);
            var setting = (BallProjectileMotionSetting)iset;
            A = setting.A;
            T = setting.T;
            startPos = projectile.transform.position;
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            float yOffset = Mathf.Sin((transform.position.x-startPos.x) * T) * A;
            Vector3 nextPos = transform.position;
            nextPos.y = startPos.y + yOffset;
            transform.position = nextPos;
        }
    }
}