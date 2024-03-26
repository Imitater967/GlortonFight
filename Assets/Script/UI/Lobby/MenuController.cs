using System;
using Script.MVC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI.Lobby
{
    public class MenuController : MvcController
    {
        private MenuView _view;
        public KeyAlterController KeyAlter;
        public AppSettingController AppSetting;
        private void Awake()
        {
            _view = GetComponent<MenuView>();
            InitView();
        }

        private void InitView()
        {
            _view.Quit.onClick.AddListener(() =>
            {
                Application.Quit();
            });
            _view.AppSetting.onClick.AddListener(() =>
            {
                AppSetting.Show();
            });
            _view.AlterKey.onClick.AddListener(() =>
            {
                KeyAlter.Show();
            });
            _view.MultiPlay.onClick.AddListener(LoadRoomScene);
        }

        private void LoadRoomScene()
        {
            SceneManager.LoadScene("Scenes/Room", LoadSceneMode.Single);
        }
    }
}