using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Script.Game;
using Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Script.UI.Lobby
{
    public  class KeyBindButton : MonoBehaviour
    {
        public InputActionReference actionRef;
        public int index = 0;
        protected TMP_Text _text;
        protected Button _button;
        protected InputAction _action;
        protected InputManager _manager;
        private void Awake()
        {
            _text = gameObject.GetComponentInChildren<TMP_Text>();
        }

        void Start()
        {  
            _action = actionRef.action;
            _button = gameObject.GetComponent<Button>();
            _text.text = Utils.ToHumanReadableName(actionRef.action, index);
            _button.onClick.AddListener(StartRebind);
            _manager = ApplicationManager.Instance.InputManager;
        }

        public void StartRebind()
        {
            var origin = _text.text;
            var playerInputActions = ApplicationManager.Instance.PlayerInput.actions;
            _text.text = "-";
            playerInputActions.Disable();
            _manager.SwitchToUI();
            // .WithControlsExcluding("Mouse/delta")
            // .WithControlsExcluding("Mouse/position")
            _action.PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")
                .WithCancelingThrough("<Keyboard>/escape")
                .WithTargetBinding(index)
                .OnComplete(operation =>
                {
                    _text.text = Utils.ToHumanReadableName(actionRef, index);
                    operation.Dispose();
                    string json = playerInputActions.SaveBindingOverridesAsJson();
                    PlayerPrefs.SetString(ApplicationManager.INPUT_SAVE_KEY, json);
                    playerInputActions.Enable();
                }).OnCancel(operation =>
                {
                    _text.text = origin;
                    playerInputActions.Enable();
                    operation.Dispose();
                }).Start();
        }
    }
}