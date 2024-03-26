using System;
using Script.MVC;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.UI.Lobby
{
    public class KeyAlterController : MvcController
    {
        protected KeyAlterView _view; 

        private void Awake()
        {
            _view = GetComponent<KeyAlterView>();
            InitView();
        }

        private void InitView()
        {
            //按键区域
            var leftBinding=_view.leftKey.gameObject.AddComponent<KeyBindButton>();
            leftBinding.index = 1;
            leftBinding.actionRef = _view.moveAction;
            var rightBinding = _view.rightKey.gameObject.AddComponent<KeyBindButton>();
            rightBinding.index = 2;
            rightBinding.actionRef = _view.moveAction;
            var crouchBinding = _view.downKey.gameObject.AddComponent<KeyBindButton>();
            crouchBinding.actionRef = _view.crouchAction;
            var jumpBinding = _view.upKey.gameObject.AddComponent<KeyBindButton>();
            jumpBinding.actionRef = _view.jumpAction;
            var punchBinding = _view.punchKey.gameObject.AddComponent<KeyBindButton>();
            punchBinding.actionRef = _view.punchAction;
            var rangedBinding = _view.rangedKey.gameObject.AddComponent<KeyBindButton>();
            rangedBinding.actionRef = _view.rangedAction;
            //返回按钮
            _view.returnButton.onClick.AddListener(Hide);
        }
 
    }
}