using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Game
{ 

    public class InputReader: MonoBehaviour,GameInput.IIngameActions
    {
        public event Action<float> MoveEvent;
        public event Action CrouchEvent;
        public event Action CrouchCancelEvent;
        public event Action JumpEvent;
        public event Action PunchEvent;
        public event Action RangedEvent;

        public void SwitchToUI()
        {
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<float>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                JumpEvent?.Invoke();
            }
 
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                CrouchEvent?.Invoke();
            }

            if (context.phase==InputActionPhase.Canceled)
            {
                CrouchCancelEvent?.Invoke();
            }
        }

        public void OnPunch(InputAction.CallbackContext context)
        {
            
            if (context.phase == InputActionPhase.Performed)
            {
                PunchEvent?.Invoke();
            }
        }

        public void OnRanged(InputAction.CallbackContext context)
        {
            
            if (context.phase == InputActionPhase.Performed)
            {
                RangedEvent?.Invoke();
            }
        }
    }
}