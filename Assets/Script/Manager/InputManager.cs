using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Manager
{
    public class InputManager: MonoBehaviour
    {
        protected PlayerInput _playerInput;

        public void Init(PlayerInput playerInput)
        {
            _playerInput = playerInput;
            SwitchToUI();
        }
        

        public void SwitchToIngame()
        {
            _playerInput.SwitchCurrentActionMap("Ingame");
            Debug.Log(GetType().Name+" now switch to ingame");
        }

        public void SwitchToUI()
        {
            _playerInput.SwitchCurrentActionMap("UI");
            Debug.Log(GetType().Name+" now switch to ui");
        }
    }
}