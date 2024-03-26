using System;
using Game;
using Unity.Netcode;
using UnityEngine;

namespace Script.Character
{
    public class GlortonFighterCombat: NetworkGlortonFighterComponent
    {
        [NonSerialized]
        public BoxCollider2D punch; 
        [NonSerialized]
        public BoxCollider2D throwArea;
        [NonSerialized]
        public BoxCollider2D kick;
        [NonSerialized]
        public BoxCollider2D upPunch; 
        public LayerMask checkLayer;
        public LayerMask checkWithTirggerLayer;
        //伤害百分比, 使用1+damage对受到的力进行翻倍
        [SerializeField] protected NetworkVariable<float> _damage = new NetworkVariable<float>();
        [SerializeField] protected NetworkVariable<short> _health = new NetworkVariable<short>();

        public ChrCombatSetting setting;
        protected RaycastHit2D hitBack;
        public bool playerBehind => hitBack.collider == null;

        public short Health
        {
            get => _health.Value;
            set { _health.Value = value; }
        }
        public float Damage
        {
            get => _damage.Value;
            set
            {
                _damage.Value = value;
                if (IsServer)
                {
                    EventManager.Instance.Combat.OnPlayerReceiveDamageServer?.Invoke( fighter, value);
                    SendDamageEventClientRpc(fighter, value);
                }
            }
        }
        [ClientRpc]
        private void SendDamageEventClientRpc(NetworkBehaviourReference glortonFighter, float damageValue)
        {
            if (glortonFighter.TryGet(out GlortonFighter fighter))
            {
                EventManager.Instance.Combat.OnPlayerReceiveDamageClient?.Invoke(fighter,damageValue);
            }
        }

        private void Start()
        {
            throwArea.enabled = false;
            punch.enabled = false;
            kick.enabled = false;
            upPunch.enabled = false; 
            checkLayer = setting.attackMask & (~(1 << gameObject.layer));
            checkWithTirggerLayer = setting.attackMask & (~(1 << gameObject.layer|1<<LayerMask.NameToLayer("Mechanism")));
        }

        private void Update()
        {
            CheckHitBack();
        }
 
        public void CheckHitBack()
        {
            Vector2 center=  throwArea.offset+new Vector2(throwArea.transform.position.x,throwArea.transform.position.y);
            var size = throwArea.size;
            var halfExtend = new Vector2(size.x / 2,0)
                             *new Vector2(throwArea.transform.localScale.x,throwArea.transform.localScale.y)
                             *new Vector2(fighter.transform.localScale.x, fighter.transform.localScale.y);
            //代码与punch基本一致，但start与end互换
            Vector2 start = center + halfExtend;
            Vector2 end = center - halfExtend ;
            
            //拳击是单体攻击
            hitBack=Physics2D.Linecast(start, end,checkLayer);
        }
        
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color=Color.green;
                Vector2 pcenter=  punch.offset+new Vector2(punch.transform.position.x,punch.transform.position.y);
                var psize = punch.size;
                var halfExtend = new Vector2(psize.x / 2,0)*new Vector2(punch.transform.localScale.x,punch.transform.localScale.y);
                Vector2 start = pcenter - halfExtend;
                Vector2 end = pcenter +halfExtend ;
                Gizmos.DrawLine(start,end);
                Vector2 kcenter=  kick.offset+new Vector2(kick.transform.position.x,kick.transform.position.y);
                var ksize = kick.size * new Vector2(kick.transform.localScale.x, kick.transform.localScale.y);
                Gizmos.DrawWireCube(kcenter,ksize);
            }
        }

        public virtual void SpecialAttack(){}
        public virtual void RangedAttack(){}
        public virtual void UpPunch()
        {
            Vector2 center=  upPunch.offset+new Vector2(upPunch.transform.position.x,upPunch.transform.position.y);
            var size = upPunch.size * new Vector2(upPunch.transform.localScale.x, upPunch.transform.localScale.y);
            //拳击是单体攻击
            Collider2D[] hit=Physics2D.OverlapBoxAll(center,size,0,checkWithTirggerLayer);
            foreach (var targetCollider in hit)
            {
                if (targetCollider.TryGetComponent(out IDamagable target))
                { 
                    if(target.GetType()==Damagable.Fighter)
                        EventManager.Instance.Combat.OnPlayerUpPunch?.Invoke(fighter,target as GlortonFighter);
                    else
                        EventManager.Instance.Mechanism.OnPlayerTriggerMechanism?.Invoke(fighter,target);
                }
            }
        }
        public virtual void Throw()
        {
     
            if (hitBack.collider!=null&&hitBack.collider.TryGetComponent(out GlortonFighter target))
            {
                EventManager.Instance.Combat.OnPlayerThrow?.Invoke(fighter,target);
            }
        }

        public virtual void Punch()
        {
            Vector2 center=  punch.offset+new Vector2(punch.transform.position.x,punch.transform.position.y);
            var size = punch.size;
            var halfExtend = new Vector2(size.x / 2,0)
                             *new Vector2(punch.transform.localScale.x,punch.transform.localScale.y)
                             *new Vector2(fighter.transform.localScale.x, fighter.transform.localScale.y);
            Vector2 start = center - halfExtend;
            Vector2 end = center +halfExtend ;
            
            //拳击是单体攻击
            var hit=Physics2D.Linecast(start, end,checkWithTirggerLayer);
            if (hit.collider!=null&&hit.collider.TryGetComponent(out IDamagable target))
            {
                if (target.GetType()==Damagable.Fighter)
                {
                    EventManager.Instance.Combat.OnPlayerPunch?.Invoke(fighter,target as GlortonFighter);
                }
                else
                { 
                    EventManager.Instance.Mechanism.OnPlayerTriggerMechanism?.Invoke(fighter,target);
                }
            }
        }
        public virtual void Kick()
        {
            Vector2 center=  kick.offset+new Vector2(kick.transform.position.x,kick.transform.position.y);
            var size = kick.size * new Vector2(kick.transform.localScale.x, kick.transform.localScale.y);
            //拳击是单体攻击
            Collider2D[] hit=Physics2D.OverlapBoxAll(center,size,0,checkLayer);
            foreach (var targetCollider in hit)
            {
                if (targetCollider.TryGetComponent(out GlortonFighter target))
                {
                    
                    EventManager.Instance.Combat.OnPlayerKick?.Invoke(fighter,target);

                }
            }
        }
    }
}