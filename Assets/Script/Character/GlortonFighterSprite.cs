 
using System.Collections;
using UnityEngine;

namespace Script.Character
{ 
    public class GlortonFighterSprite: GlortonFighterComponent
    {
        public float shockSwitchInterval = 0.1f;
        public Coroutine shockSwitchTask;
        private void Start()
        {
            ResetSprite();
        }
        protected void OnDestroy()
        {
            if(shockSwitchTask!=null)
            StopCoroutine(shockSwitchTask); 
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
                    SpecialAttack();
                    break;
                case FighterState.Fly:
                    Fly();
                    break;
                case FighterState.Ranged:
                    Ranged();
                    break;
                case FighterState.Shock0:
                    Shock0();
                    break;
                case FighterState.Shock1:
                    Shock1();
                    break;
            }
        }

        protected virtual void Shock0()
        { 
            fighter.bodyRenderer.sprite = fighter.skinSheet.shockedSheet.tick0.body;
            fighter.l_footRenderer.sprite = fighter.skinSheet.shockedSheet.tick0.l_foot;
            fighter.r_footRenderer.sprite = fighter.skinSheet.shockedSheet.tick0.r_foot;            
            fighter.l_handRenderer.sprite = fighter.skinSheet.shockedSheet.tick0.l_hand;
            fighter.r_handRenderer.sprite = fighter.skinSheet.shockedSheet.tick0.r_hand;
        }

        protected virtual void Shock1()
        {
            fighter.bodyRenderer.sprite = fighter.skinSheet.shockedSheet.tick1.body;
            fighter.l_footRenderer.sprite = fighter.skinSheet.shockedSheet.tick1.l_foot;
            fighter.r_footRenderer.sprite = fighter.skinSheet.shockedSheet.tick1.r_foot;            
            fighter.l_handRenderer.sprite = fighter.skinSheet.shockedSheet.tick1.l_hand;
            fighter.r_handRenderer.sprite = fighter.skinSheet.shockedSheet.tick1.r_hand; 
        }

        protected virtual void Ranged()
        {
            ResetSprite();
        }
        protected virtual void SpecialAttack()
        {
            ResetSprite(); 
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

        public void StopShocked()
        {
            if (shockSwitchTask != null)
            {
                StopCoroutine(shockSwitchTask);
                shockSwitchTask = null;
            }
            ResetSprite();
        }

        public void StartShocked()
        {
            StopShocked();
            shockSwitchTask = StartCoroutine(ShockTask()); 
        }

        protected IEnumerator ShockTask()
        {
            bool b = false;
            while (true)
            {
                if (b)
                {
                    SwitchState(FighterState.Shock0); 
                    b = false;
                }
                else
                {
                    SwitchState(FighterState.Shock1);
                    b = true;
                }
                yield return new WaitForSeconds(shockSwitchInterval);
            }
 
        }
    }
}