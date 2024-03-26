using System;
using System.Collections;
using Script.Game;
using Unity.Netcode;
using UnityEngine;

namespace Script.Character
{
    public class GlortonFighterInput: NetworkGlortonFighterComponent
    {
        public ChrInputSetting setting;
        [Header("属性设置"),SerializeField]
        protected float inputBlockRemain;
       [SerializeField]
        protected float saCheckRemain;
        protected GlortonFighterAnimation _animation;
        protected GlortonFighterMotion _motion;
        protected Coroutine task = null;
        [Header("Debugging")]
        public bool jump ;
        public bool crouch;
        public bool attack0;
        public bool attack1; 
        public float xAxis = 0; 
        public int specialAttackCount=0;
        private void Start()
        {
            task=    StartCoroutine(NeverStopTask());
            var input = ApplicationManager.Instance.InputReader;
            if(!IsOwner)
                return;
            input.MoveEvent += (axis) =>
            {
                MoveServerRpc(axis);
            };
            input.JumpEvent += () =>
            {
                JumpServerRpc();
            };
            input.CrouchEvent += () =>
            {
                CrouchServerRpc();
            };
            input.CrouchCancelEvent += () =>
            {
                CancelCrouchServerRpc();
            };
            
            input.PunchEvent += () =>
            {
                PunchServerRpc();
            };
            input.RangedEvent += () =>
            {
                RangedServerRpc();
            };

        }

        #region Server Rpcs

        [ServerRpc]
        private void RangedServerRpc()
        {
            attack1=true;
        }

        [ServerRpc]
        private void PunchServerRpc()
        {
            attack0 = true;
        }
        [ServerRpc]
        private void CancelCrouchServerRpc()
        {
            crouch = false;
        }
        [ServerRpc]
        private void CrouchServerRpc()
        {
            crouch = true;
            if (downTask == null&&fighter.motion.StandOnPlatform())
            {
                TryGetDownOfPlatform();
                // downTask = StartCoroutine(RestoreCollider());
            }
        }

        private Coroutine downTask;
        private void TryGetDownOfPlatform()
        {
            if (fighter.motion.StandOnPlatform())
            {
                fighter.collider.isTrigger = true;
                
            }
        }

        // private IEnumerator RestoreCollider()
        // {
        //     // yield return new WaitForSeconds(triggerTime);
        //     // fighter.collider.isTrigger = false;
        //     // downTask = null;
        // }
        [ServerRpc]
        private void JumpServerRpc(){jump = true;}
        [ServerRpc]
        private void MoveServerRpc(float axis)
        {
            xAxis = axis;
        }
        

        #endregion
        protected void OnDestroy()

        {
         StopCoroutine(task);   
        }

        protected override void OnInit()
        {
            base.OnInit(); 
                _animation = fighter.animation;
                _motion = fighter.motion; 
        }
        public void UnblockInput()
        {
            inputBlockRemain = -1;
            fighter.EnableMotion();
            enabled = true;   
        }
        public void BlockInput(float time)
        {
            inputBlockRemain = time;
            fighter.DisableMotion(); 
            enabled = false; 
        }
        public IEnumerator NeverStopTask()
        {
            while (true)
            {
                if (inputBlockRemain > 0)
                {
                    inputBlockRemain -= Time.deltaTime;
                    enabled = false;
                }
                //改为着地之前无法操控
                // if (inputBlockRemain < 0)
                // {
                //     enabled = true;
                //     fighter.EnableMotion();
                // }
                
                yield return new WaitForFixedUpdate();
            }

            yield return null;
        }
        protected void Update()
        {
            if(!fighter.init)
                return;
            if(!IsServer)
                return;
            if (saCheckRemain > 0)
            {
                saCheckRemain -= Time.deltaTime;
            }
              
            _animation.Idle();
            _motion.xAxis = 0; 
            //攻击
            if (saCheckRemain > 0)
            {
                if (attack1&&specialAttackCount<2)
                {
                    _animation.ResetJumpTrigger();
                    _animation.SpecialAttack();
                    enabled = false;
                    specialAttackCount += 1; 
                }
            }
            if (attack0)
            {
                _animation.Punch(); 
            }
            if (jump)
            {
                saCheckRemain = setting.saCheckInterval;
                if (_motion.jumpCount == 0)
                {
                    _motion.Jump(); 
                    //动画与计数修改为isGround=false并且velocity>0才进行跳跃
                    //相当于延迟执行,修复上面的bug
                }else if (_motion.jumpCount == 1)
                { 
                    _motion.SecondJump();
                    _animation.SecondJump();
                    _motion.jumpCount = 2;  
                }

            } else if (crouch)
            {
                if (_motion.IsGrounded())
                {
                    _animation.Crouch();  
                }
                else
                {
                    _motion.AcclerateYDown(); 
                }
            }
            else
            { 
                //行走
                if (xAxis > 0)
                {
                    _animation.Run(); 
                    _motion.xAxis = 1;
                    _animation.AlignFace(true);
                }else if (xAxis < 0)
                {
                    _animation.Run();
                    _motion.xAxis = -1;
                    _animation.AlignFace(false);
                }
                if (attack1)
                {
                    _animation.Ranged(); 
                } 
            }

            attack0 = false;
            attack1 = false;
            jump = false;
            UpdateJumpCount();
        }

        private void UpdateJumpCount()
        {
            if (_motion.Velocity.y>0&&!_motion.IsGrounded())
            {
                if (_motion.jumpCount == 0)
                {   
                    _motion.jumpCount = 1; 
                } 
            } 
        } 
    }
}