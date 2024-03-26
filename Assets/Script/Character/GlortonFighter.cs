using System;
using System.Collections;
using Game;
using Script.Game;
using Script.Mechanism;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Script.Character
{
    public enum FighterState
    {
        //待机,
        Idle=0,
        Run=1,
        Fly=2,//飞行
        Punch=3,//拳击,
        Kick=4,//踢腿,
        Throw=5,//过肩摔,
        UpPunch=6,//升龙拳
        Crouch=7,//下蹲
        Jump=8,//跳跃
        UpPunchMiddle=9,
        SpecialAttack=10,
        Ranged=11,
        Shock0,
        Shock1,
    } 
    public abstract class GlortonFighter: NetworkBehaviour,IDamagable
    {
        [NonSerialized] public string name;
        [Header("骨骼设置")]
        public GameObject bodyObj;
        [NonSerialized]
        public SpriteRenderer bodyRenderer;
        public GameObject lockObj;
        public GameObject l_footObj;
        [NonSerialized]
        public SpriteRenderer l_footRenderer;
        public GameObject l_handObj;
        [NonSerialized]
        public SpriteRenderer l_handRenderer;
        public GameObject r_footObj;
        [NonSerialized]
        public SpriteRenderer r_footRenderer;
        public GameObject r_handObj;
        [NonSerialized]
        public SpriteRenderer r_handRenderer;
        [Header("贴图设置")] public FighterSkinSheet skinSheet;
        [Header("编辑器设置")] public bool refresh = false;
        [Header("组件-输入")] 
        public ChrInputSetting inputSetting;
        [Header("组件-战斗")]
        public ChrCombatSetting combatSetting;
        public BoxCollider2D kickAttackArea;
        public BoxCollider2D  punckAttackArea;
        public BoxCollider2D upPunchAttackArea;
        public BoxCollider2D throwAttackArea;
        [Header("组件-Motion")] 
        public ChrMotionSetting motionSetting;

        public new GlortonFighterAnimation animation { get; protected set; }

        [Header("战斗属性")] public float shockRemain;
         public bool shocking;
         [FormerlySerializedAs("projectilePrefab")] 
         public GameObject rangedProjectilePrefab;
         [FormerlySerializedAs("rangedMuzzlePosition")] [FormerlySerializedAs("muzzlePosition")] 
         public Transform rangedMuzzleTransform;        
         public GameObject specialAttackProjectilePrefab;
         public Transform specialAttackMuzzle;
         public GlortonFighter killer;
         public GlortonFighterSprite sprite
        {
            get;
            protected set;
        }
        public GlortonFighterInput input{
            get;
            protected set;
            
        }

        public GlortonFighterMotion motion
        {
            get;
            protected set;
        }

        public GlortonFighterCombat combat
        {
            get;
            protected set;
        }
        public Animator animator
        {
            get;
            protected set;
        }        
        public Rigidbody2D rb
        {
            get;
            protected set;
        }

        public BoxCollider2D collider { get; private set; }
        public bool init = false;
        public Action OnInit;

        public bool Dead = false;
        [NonSerialized]
        public GameObject components;

        protected virtual void Awake()
        {
            name = gameObject.name;
            ReloadSpriteRenderer();
            components=new GameObject("Components");
            components.transform.SetParent(transform);
            components.transform.localPosition = new Vector3(0, 0, 0);
            components.layer = gameObject.layer;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            //运动过快,直接穿墙
            rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
            collider = GetComponent<BoxCollider2D>();           
            animation=components.AddComponent<GlortonFighterAnimation>();
            animation.RegInit(this);
            combat=components.AddComponent<GlortonFighterCombat>();
            combat.setting = combatSetting;
            combat.RegInit(this);
            combat.kick = kickAttackArea;
            combat.punch = punckAttackArea;
            combat.upPunch = upPunchAttackArea;
            combat.throwArea = throwAttackArea;
            motion=components.AddComponent<GlortonFighterMotion>();
            motion._setting = motionSetting; 
            motion.RegInit(this);
            input=components.AddComponent<GlortonFighterInput>();
            input.setting = inputSetting;
            input.RegInit(this);
            sprite=components.AddComponent<GlortonFighterSprite>();
            sprite.RegInit(this);
        }
        private void OnEnable()
        {
            ResetState();
        }

        public void ResetState(){
            motion.StopSpecialAttack();
            input.UnblockInput();
            motion.UnFreeze();
            motion.ExitCrouch();
            animation.StopFly();
            collider.isTrigger = false;

        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if(!IsServer)
                return;
            if (!Dead&&other.tag.Equals("Bound"))
            {
                Dead = true;
                combat.Health -= 1;
                combat.Damage = 0;
               EventManager.Instance.Game.OnPlayerDeathServer?.Invoke(this); 
               SendDeathEventClientRpc(combat.Health); 
               gameObject.SetActive(false);
            }

            if (other.tag.Equals("Platform"))
            {
                collider.isTrigger = false;
            }
        } 

        [ClientRpc]
        private void SendDeathEventClientRpc(short health)
        { 
            gameObject.SetActive(false); 
            EventManager.Instance.Game.OnPlayerDeathClient?.Invoke(this,transform.position);  
            ApplicationManager.Instance.EventManager.Game.OnPlayerHealthChangeClient?.Invoke(this,health);
        }

        public void DisableMotion()
        {
            motion.enabled = false;
        }

        public void EnableMotion()
        {
            motion.enabled = true;
        }
        protected void Start()
        {
                init = true; 
                OnInit?.Invoke();
                Debug.Log("已经成功加载玩家"+name);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn(); 
            if (IsClient)
            {   
                Debug.Log("执行客户端OnPlayerSpawnClient应用");
                ApplicationManager.Instance.EventManager.Game.OnPlayerSpawnClient?.Invoke(this);
            }
        }

        protected virtual void Update()
        {
            if (!Application.isPlaying)
            {
                if (bodyRenderer==null||refresh)
                {
                    ReloadSpriteRenderer();
                    refresh = false;
                }
                return;
            }

            if (shocking)
            {

                if (shockRemain < 0)
                {
                    UnShock();
                    shocking = false;
                    return;
                }

                shockRemain -= Time.deltaTime;
            }

        }
        
        public void ReloadSpriteRenderer()
        {
            bodyRenderer = bodyObj.GetComponent<SpriteRenderer>();
            l_footRenderer = l_footObj.GetComponent<SpriteRenderer>();
            r_footRenderer = r_footObj.GetComponent<SpriteRenderer>();
            l_handRenderer = l_handObj.GetComponent<SpriteRenderer>();
            r_handRenderer = r_handObj.GetComponent<SpriteRenderer>();
        }

        public void RevDamage(float damage)
        {
            combat.Damage += damage;
            
        }

        public Damagable GetType()
        {
            return Damagable.Fighter;
        }

        public void Shock(float duration)
        {
            shocking = true;
            shockRemain = duration;
            input.BlockInput(duration+1);
            ShockClientRpc();
            animation.StartShock();
            motion.Freeze();
        }
        [ClientRpc]
        private void ShockClientRpc()
        {
            sprite.StartShocked();
        }
        [ClientRpc]
        private void UnShockClientRpc()
        {
            sprite.StopShocked();
        }
        public void UnShock()
        {
            UnShockClientRpc();
            input.UnblockInput();
            animation.StopShock();
            motion.UnFreeze();
        }
 
    }
}