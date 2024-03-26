using System;
using Unity.Netcode.Components;
using UnityEngine;

namespace Script.Character
{
    public class GlortonFighterAnimation: NetworkGlortonFighterComponent
    {
        protected NetworkAnimator _networkAnimator;
        protected Animator _animator; 
        protected GlortonFighterMotion _motion;
        private static readonly int PARAM_ATTTACK_0 = Animator.StringToHash("Attack0");
        private static readonly int PARAM_ATTTACK_1 = Animator.StringToHash("Attack1");
        private static readonly int PARAM_CROUCH = Animator.StringToHash("Crouch");
        private static readonly int PARAM_RUN = Animator.StringToHash("Run");
        private static readonly int PARAM_GROUNDED = Animator.StringToHash("Grounded");
        private static readonly int PARAM_JUMP = Animator.StringToHash("Jump");
        private static readonly int PARAM_DOUBLEJUMP = Animator.StringToHash("DoubleJump");
        private static readonly int PARAM_FLY = Animator.StringToHash("Fly");
        private static readonly int PARAM_SpeedX = Animator.StringToHash("VelocityX");
        private static readonly int PARAM_SpeedY = Animator.StringToHash("VelocityY");
        private static readonly int ParamBehind = Animator.StringToHash("Behind");
        private static readonly int PARAM_SA = Animator.StringToHash("SA");
        private static readonly int PARAM_SAing = Animator.StringToHash("SAing");
        private static readonly int PARAM_SHOCK = Animator.StringToHash("Shock");

        private void Awake()
        {
            _networkAnimator = GetComponentInParent<NetworkAnimator>();
        }

        protected override void OnInit()
        {
            base.OnInit();
            _motion = fighter.motion;
            _animator = fighter.animator;
        }

        private void Update()
        {
            if(!fighter.init||!IsServer) 
                return;
            
            _animator.SetFloat(ParamBehind,fighter.combat.playerBehind?0:1);
        }

        private void FixedUpdate()
        {
            if(!fighter.init||!IsServer) 
                return;
            _animator.SetBool(PARAM_GROUNDED,_motion.IsGrounded());
            _animator.SetInteger(PARAM_SpeedX,Mathf.RoundToInt(fighter.rb.velocity.x)); 
            _animator.SetInteger(PARAM_SpeedY,Mathf.RoundToInt(fighter.rb.velocity.y)); 
        }

        public void Idle()
        {
            _animator.SetBool(PARAM_RUN,false);
            _animator.SetBool(PARAM_CROUCH,false);
        }
        public void Run()
        {
            _animator.SetBool(PARAM_RUN,true);
        }

        public void AlignFace(bool right)
        {
            if (right)
            {
                fighter.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                fighter.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        public void Fly()
        {
            _animator.SetBool(PARAM_FLY,true);
            _animator.CrossFade("飞行开始",0);
            //todo: 不知道cross fade是否会同步
            _networkAnimator.ResetTrigger(PARAM_ATTTACK_0);
            _networkAnimator.ResetTrigger(PARAM_ATTTACK_1);
            // _animator.ResetTrigger(PARAM_ATTTACK_0);
            // _animator.ResetTrigger(PARAM_ATTTACK_1);
        }

        public void StopFly()
        {
            _animator.SetBool(PARAM_FLY,false);
        }
        public void SpecialAttack()
        {
            _networkAnimator.SetTrigger(PARAM_SA);
            _networkAnimator.ResetTrigger(PARAM_JUMP);
            // _animator.SetTrigger(PARAM_SA);
            // _animator.ResetTrigger(PARAM_JUMP);
        }
        public void Punch()
        {
            _networkAnimator.SetTrigger(PARAM_ATTTACK_0);
            // _animator.SetTrigger(PARAM_ATTTACK_0); 
        }

        public void Crouch()
        {
            _animator.SetBool(PARAM_CROUCH,true);
        }

        public void Jump()
        {
            _networkAnimator.SetTrigger(PARAM_JUMP);
            // _animator.SetTrigger(PARAM_JUMP); 
        }

        public void SecondJump()
        { 
            _networkAnimator.SetTrigger(PARAM_DOUBLEJUMP);
            // _animator.SetTrigger(PARAM_DOUBLEJUMP);
        }

        public void ResetAttackTriggers()
        {
            _networkAnimator.ResetTrigger(PARAM_ATTTACK_0);
            _networkAnimator.ResetTrigger(PARAM_ATTTACK_1);
            // _animator.ResetTrigger(PARAM_ATTTACK_0);
            // _animator.ResetTrigger(PARAM_ATTTACK_1);
        }

        public void Ranged()
        { 
            // _animator.SetTrigger(PARAM_ATTTACK_1);
            _networkAnimator.SetTrigger(PARAM_ATTTACK_1);
        }

        public void ResetJumpTrigger()
        {
         
            _networkAnimator.SetTrigger(PARAM_JUMP);
            // _animator.ResetTrigger(PARAM_JUMP);
        }

        public void StartSpecialAttack()
        {
            _animator.SetBool(PARAM_SAing,true);
        }
        public void StopSpecialAttack()
        {
            _animator.SetBool(PARAM_SAing,false);
        }

        public void StartShock()
        {
            _animator.SetBool(PARAM_SHOCK,true);
        }
        public void StopShock()
        {
            _animator.SetBool(PARAM_SHOCK,false);
        }
    }
}