using System;
using Script.Game;
using Script.MVC;
using UnityEngine;

namespace Script.UI.Lobby
{
    public class AppSettingController : MvcController
    { 
        private AppSettingView _view;
        private void Awake()
        {
            _view = GetComponent<AppSettingView>();
            InitView();
        }

        private void InitView()
        {
            _view.ReturnButton.onClick.AddListener(Hide); 
            
            _view.MusicVolume.value = ApplicationManager.Instance.MusicSetting.Volume;
            _view.SoundVolume.value = ApplicationManager.Instance.SoundSetting.Volume;
            
            _view.SoundVolume.onValueChanged.AddListener((volume) =>
            {
                ApplicationManager.Instance.SoundSetting.Volume = volume;
            });
            _view.MusicVolume.onValueChanged.AddListener((volume) =>
            {
                ApplicationManager.Instance.MusicSetting.Volume = volume;
            });
        }
    }
}