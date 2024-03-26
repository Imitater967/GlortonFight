using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Script.Character
{
    public  class GlortonFighterMotion: GlortonFighterComponent
    {
        public Rigidbody2D _rb; 
        public ChrMotionSetting _setting;
        [Header("跳跃"),SerializeField] 
        protected bool isGrounded;
        public int jumpCount = 0; 
        [Header("移动")]
        public float xAxis;
        [SerializeField]
        protected Vector2 velocity; 
        [SerializeField]
        protected bool crouching=false;

        [SerializeField] protected bool _onPlatform;
        [SerializeField] protected bool freezing = false;
        [SerializeField] protected bool specialAttacking = false;
        public Vector3 Velocity => velocity;
        
        protected BoxCollider2D collider;
        protected Coroutine checkGroundTask;
        protected RigidbodyConstraints2D originConstraint;
        private bool _inited;
        private void Start()
        { 
            originConstraint = _rb.constraints;
        }

        private void OnEnable()
        {
            if(checkGroundTask==null)
                checkGroundTask=StartCoroutine(NeverStopTask());
        }

        private void OnDisable()
        {
            if(checkGroundTask!=null)
                StopCoroutine(checkGroundTask);
            checkGroundTask = null;
        }

        public IEnumerator NeverStopTask()
        {
            while (true)
            {
                if (!freezing&&_inited&&fighter.IsServer)
                {
                    SyncSpeed();
                    CheckGrounded(); 
                } 
                yield return new WaitForFixedUpdate();
            }
        }
        protected override void OnInit()
        {
            base.OnInit(); 
            _rb = fighter.rb;
            collider = fighter.collider;
            _inited = true;
        }

        protected virtual void FixedUpdate()
        {
            if (!fighter.IsServer)
            {
                return;
            }
            velocity.x = xAxis * _setting.moveSpeed;  
            if (crouching)
            {
                velocity.x = 0;  
            }

            Vector2 pos = transform.position; 
            var hit2D = Physics2D.Linecast(pos, pos + velocity * Time.fixedDeltaTime);
      
            if (velocity.y < _setting.minYSpeed)
            {
                velocity.y = _setting.minYSpeed;
            }

            if (specialAttacking&&!_setting.canMoveXWhileSpecialAttack)
            {
                velocity.x = 0;
            }
            _rb.velocity = velocity;
        }

        public void Freeze()
        {
            if(_rb!=null)
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            freezing = true;
        }

        public void UnFreeze()
        {
            freezing = false;
            if(_rb!=null)
            _rb.constraints = originConstraint;
        }
        private void OnDrawGizmos()
        { 
            Gizmos.color=Color.gray; 
            var size = collider.size;
            Gizmos.DrawWireCube(transform.position+new Vector3(collider.offset.x,collider.offset.y,0)-new Vector3(0,size.y/2),size*(1)/new Vector2(1,4));
        
            //    Gizmos.DrawWireCube(colliderBounds.center-new Vector3(0,colliderBounds.size.y/2),colliderBounds.size*(1)/new Vector2(1,4));
        }

        private void SyncSpeed()
        { 
            { 
                var rbSpeed = _rb.velocity; 
                velocity.x = rbSpeed.x;
                velocity.y = rbSpeed.y; 
            }
        }
        private void CheckGrounded()
        {  
            _onPlatform=false;
            var size = collider.size;
            var center = transform.position + new Vector3(collider.offset.x, collider.offset.y, 0);
            RaycastHit2D[] hit2D = Physics2D.BoxCastAll(new Vector2(center.x,center.y)-new Vector2(0,size.y/2),size*(1)/new Vector2(1,4),0,Vector2.down,_setting.downOffset,_setting.downCheckMask);
            isGrounded = hit2D.Any(a => a.collider != null && !a.collider.isTrigger);
            if (isGrounded)
            {
                fighter.input.specialAttackCount = 0;
                jumpCount = 0;
                _onPlatform=hit2D.Any(a=>a.collider!=null&&a.collider.tag.Equals("Platform"));
            }
        }
        public void Jump()
        { 
            if(crouching||specialAttacking)
                return;
            velocity.y = Mathf.Sqrt(_setting.jumpHeight * 2f * -_setting.gravity);
            _rb.velocity = velocity;
        }

        public void SecondJump()
        {
            if(crouching||specialAttacking)
                return;
            velocity.y = Mathf.Sqrt(_setting.secondJumpHeight * 2f * -_setting.gravity);
            _rb.velocity = velocity;
        }
        //Executed by animation event 
        //-chr_common_upjump
        public void UpPunchJump()
        {
            //采用+=而不是=更贴进原版
            velocity.y += Mathf.Sqrt(_setting.upPunchHeight * 2f * -_setting.gravity);
            _rb.velocity = velocity;
        }
        public bool IsGrounded()
        {
            return isGrounded;
        }

        public bool StandOnPlatform()
        {
            return _onPlatform;
        }
        public void EnterCrouch()
        {
            crouching = true;
        }
        public void ExitCrouch()
        {
            crouching = false;
        }

        public void AcclerateYDown()
        {
            if (velocity.y > _setting.accleratedDownSpeed)
            {
                velocity.y = _setting.accleratedDownSpeed;
            }
            _rb.velocity = velocity;
        }

        public void StartSpecialAttack()
        {
            specialAttacking = true;
        }       
        public void StopSpecialAttack()
        {
            specialAttacking = false;
        }

        public void SpecialAttackJump()
        {
            velocity.y = Mathf.Sqrt(_setting.saHeight * 2f * -_setting.gravity);
            _rb.velocity = velocity; 
        }
    }
}