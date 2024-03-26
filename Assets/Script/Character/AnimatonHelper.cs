using System;
using Script.Character.Peach;
using UnityEngine;

namespace Script.Character
{
    //一次性脚本，随用随注销
    [ExecuteInEditMode]
    public class AnimatonHelper: MonoBehaviour
    {
        protected GlortonFighter fighter;

        private void OnEnable()
        {
            if (Application.isPlaying)
                Destroy(this);
        }

        private void Update()
        {
            if(!Application.isPlaying){
                fighter = GetComponent<GlortonFighter>();
                fighter.ReloadSpriteRenderer();
            }
        }
       public void SwitchState(FighterState fighterState)
        {
            switch (fighterState)
            {
                case FighterState.Idle:
                    ResetSprite();
                    break;
                case FighterState.Run:
                    Run();
                    break;
                case FighterState.Punch:
                    FistOn();
                    break;
                case FighterState.Kick:
                    Kick();
                    break;
                case FighterState.Crouch:
                    Crouch();
                    break;
                case FighterState.Throw:
                    ThrowAction();
                    break;
                case FighterState.Jump:
                    Jump();
                    break;
                case FighterState.UpPunch:
                    UpPunch();
                    break;
                case FighterState.UpPunchMiddle:
                    UpPunchMiddle();
                    break;
                case FighterState.SpecialAttack:
                    ASpecialAttack();
                    // BSpecialAttack();
                    break;
                case FighterState.Fly:
                    Fly();
                    break;
                case FighterState.Ranged:
                    ResetSprite();
                    break;
            }
        }

       private void ASpecialAttack()
       {
           FistOn();
           fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;
       }

       protected void PeachRanged()
       {
           ResetSprite(); 
            fighter.r_handRenderer.sprite=(fighter.skinSheet as PeachSkinSheet).r_shotHand;
       }
       protected void BSpecialAttack()
       {
           ResetSprite();
           FistOn();
           fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;
       }
       private void SBSpecialAttack()
       { 
           ResetSprite();
           FistOn();
           fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;
       }

       protected virtual void Fly()
       {
           ResetSprite();      
           fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;

       }
        private void UpPunch()
        {
            ResetSprite();
            FistOn();
            fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;
        }
        private void UpPunchMiddle()
        {
            fighter.r_handRenderer.sprite = fighter.skinSheet.l_fist;
            fighter.l_handRenderer.sprite = fighter.skinSheet.r_fist;
        }
        protected void Jump()
        {
            ResetSprite();
            fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;
        }

        protected void ThrowAction()
        {
            ResetSprite();
            FistOn();
            fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;
        }

        protected void Crouch()
        {  
            ResetSprite();
            fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;
        }

        protected void Kick()
        { 
            ResetSprite();
            FistOn();
            fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;
        }
        protected void Run()
        {
            ResetSprite();
            FistOn();
            fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;
        }
        protected void FistOn()
        { 
            fighter.l_handRenderer.sprite = fighter.skinSheet.l_fist;
            fighter.r_handRenderer.sprite = fighter.skinSheet.r_fist;
        }       
        protected void FistOff()
        {  
            fighter.l_handRenderer.sprite = fighter.skinSheet.defaultSheet.l_hand;
            fighter.r_handRenderer.sprite = fighter.skinSheet.defaultSheet.r_hand;
        }
        public void ResetSprite()
        {
            if (fighter.bodyRenderer == null)
            {
                Debug.Log("reset sprite while render is null");
                return;
            }
            fighter.bodyRenderer.sprite = fighter.skinSheet.defaultSheet.body;
            fighter.l_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;
            fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.r_foot;
            FistOff();
        }
    }
}