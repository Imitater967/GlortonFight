using System;
using Game;
using Script.Game;
using Script.MVC;
using UnityEngine;

namespace Script.UI.Game
{
    public class GameStartCountdownController : MvcController
    {
        private GameStartCountdownView _view;
        private EventManager _eventManager;
        private void Awake()
        {
            _view = GetComponent<GameStartCountdownView>();
            _eventManager = ApplicationManager.Instance.EventManager;
            _eventManager.Game.OnPrepareCountdown += AnimatedCountdown;
            _eventManager.Game.OnStart += Hide;
        } 
        private void AnimatedCountdown(byte obj)
        {
            if(obj!=0)
                _view.countdown.text = ""+obj;
            else
                _view.countdown.text = "GO !";
        }
    }
}