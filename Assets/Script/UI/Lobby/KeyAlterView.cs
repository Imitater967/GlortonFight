using System;
using Script.MVC;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Script.UI.Lobby
{
    public class KeyAlterView : MonoBehaviour
    {
        [Header("返回按钮")] public Button returnButton;
        [Header("改键区域")]
        public Button leftKey;
        public Button rightKey;
        public Button upKey;
        public Button downKey;
        public Button punchKey;
        public Button rangedKey;
        public InputActionReference moveAction;
        public InputActionReference jumpAction;
        public InputActionReference crouchAction;
        public InputActionReference punchAction;
        public InputActionReference rangedAction;

    }
}