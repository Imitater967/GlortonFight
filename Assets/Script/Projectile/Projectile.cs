using System;
using Game;
using Script.Character;
using UnityEngine;

namespace Script.Projectile
{
    public abstract class Projectile: MonoBehaviour
    { 
        public ProjectileSetting setting; 
        public GlortonFighter launcher;
        public float lifeTime;
        [NonSerialized]
        public Transform muzzle;
        [NonSerialized]
        public ProjectileMotion  motion;
        [NonSerialized]
        public GameObject sprite; 
        public LayerMask checkMask;
        public bool debugging;
        public virtual void Init(GlortonFighter fighter, Transform muzzle1)
        {
            this.muzzle = muzzle1; 
            gameObject.layer = LayerMask.NameToLayer("Projectile");
            launcher = fighter;
            lifeTime = setting.lifeTime; 
            if(setting.triggerMechanism)
                checkMask= setting.checkLayers & (~(1 << gameObject.layer|1<<launcher.gameObject.layer|1<<LayerMask.NameToLayer("Mechanism")));
            else
                checkMask= setting.checkLayers & (~(1 << gameObject.layer|1<<launcher.gameObject.layer));
            
        }
        protected virtual void FixedUpdate()
        {
            lifeTime -= Time.fixedDeltaTime;
            if(lifeTime<0)
                Destroy(gameObject);
            CheckForHit();
        }

        protected virtual Collider2D CheckTarget()
        {
            Vector2 startPos = transform.position; 
            Vector2 nextPos = startPos+ motion.velocity * Time.fixedDeltaTime;
            RaycastHit2D hit = Physics2D.Linecast(startPos, nextPos, checkMask);
            Debug.DrawLine(startPos,nextPos,Color.magenta);
            return   hit.collider ;
        }
        protected virtual void CheckForHit()
        {
            var checkTarget = CheckTarget();
                if (checkTarget != null)
                { 
                    if (checkTarget.TryGetComponent(out IDamagable damagable))
                    {
                        if (damagable.GetType()==Damagable.Fighter)
                        {
                            OnHitPlayer(damagable as GlortonFighter);
                        }
                        else
                        { 
                            OnHitMechanism(damagable);
                        }
                    }
                    else
                    {  
                        OnHitWall();
                    } 
            }
            // var checkBoundsTransform = checkBounds.transform;
            // Vector2 center=  checkBounds.offset+new Vector2(checkBoundsTransform.position.x,checkBoundsTransform.position.y);
            // var size = checkBounds.size;
            // var halfExtend = new Vector2(size.x / 2, 0)
            //                  * new Vector2(checkBoundsTransform.localScale.x, checkBoundsTransform.localScale.y);
            // var layer = setting.checkLayers & (~(1 << launcher.gameObject.layer));
            // Collider[] colliders = Physics.OverlapBox(center, halfExtend, checkBoundsTransform.rotation, layer);
            // foreach (var collider1 in colliders)
            // {
            //     if (collider1.TryGetComponent(out GlortonFighter fighter))
            //     {
            //         
            //     }
            // }
        }

        protected virtual void OnHitWall()
        {
            Destroy(gameObject);
        }

        protected virtual void OnHitMechanism(IDamagable component)
        {
            EventManager.Instance.Mechanism.OnPlayerTriggerMechanism?.Invoke(launcher,component);
        }
        protected abstract void OnHitPlayer(GlortonFighter glortonFighter);

        protected void AlignRotationByVelocity()
        {
            float angle = 0;
            if (motion.velocity.y != 0)
            {
                angle=Mathf.Atan(motion.velocity.x / motion.velocity.y) * Mathf.Rad2Deg;
            }
            if (motion.velocity.y > 0)
            {
                angle = angle - 180;
            }

            var rot = new Vector3(0, 0, 180 - angle); 
            transform.eulerAngles = rot; 
        }
    }
 
}