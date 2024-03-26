using System;
using UnityEngine;

namespace Script.Character
{
     [ExecuteInEditMode]
    public class GFAnimAdaptor: MonoBehaviour
    {
        private GlortonFighterSprite _sprite; 
        public FighterState state;
        private FighterState lastState;
        protected GlortonFighter _fighter;
        private void Awake()
        {
            _fighter = GetComponent<GlortonFighter>(); 
            _fighter.OnInit += () =>
            {
                _sprite = _fighter.sprite; 
            };
        }
        private void Update()
        {
   

            if (!Application.isPlaying)
            {
                _fighter = GetComponent<GlortonFighter>(); 
                if (state != lastState) {
                    if (TryGetComponent(out AnimatonHelper helper))
                    {
                        helper.SwitchState(state);  
                    }
                    lastState=state;
                }  
            }
            
            if (_fighter.init)
            {
                if (state != lastState) { 
                    _fighter.sprite.SwitchState(state); 
                    // _input.SwitchState(state);
                    lastState=state;
                }  
            }
        }

        public void UpPunchJump()
        {
            _fighter.motion.UpPunchJump();
        }
        public void TriggerPunchDamage()
        {
            _fighter.combat.Punch();
        }

        public void TriggerStandUp()
        {
            _fighter.animation.StopFly();
        }
        public void TriggerUpPunchDamage()
        { 
            _fighter.combat.UpPunch();
        }
        public void TriggerThrowDamage()
        { 
            _fighter.combat.Throw();
        }
        public void TriggerKickDamage()
        { 
            _fighter.combat.Kick();
        }

        public void TriggerRangedAttack()
        {
            _fighter.combat.RangedAttack();

        }
        public void SpecialAttackJump()
        {
            _fighter.motion.SpecialAttackJump();
        }
        public void TriggerSpecialAttackDamage()
        { 
            _fighter.combat.SpecialAttack();
        }
    }
}