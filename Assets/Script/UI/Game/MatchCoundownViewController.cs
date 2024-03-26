using System;
using Script.Game;
using Script.MVC;
using TMPro;
using UnityEngine;

namespace Script.UI.Game
{
    public class MatchCoundownViewController : MvcController
    {
        public TMP_Text Text;

        private void Awake()
        {
            ApplicationManager.Instance.EventManager.Game.OnMatchCountdown += ChangeText;
            ApplicationManager.Instance.EventManager.Game.OnPrepared += () =>
            {
                bool dontEnable=ApplicationManager.Instance.RoomManager.RoomSetting.DeathMatch;
                if(dontEnable)
                    gameObject.SetActive(false);
            };
        }

        private void ChangeText(int time)
        { 
            Text.text= $"{time / 60:D2}:{time % 60:D2}";
            if (time <= 5)
            {
                Text.color=Color.red;
            }
        }
    }
}