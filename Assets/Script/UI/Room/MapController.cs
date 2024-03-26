using System;
using Script.Game;
using Script.Game.Room.Map;
using Script.MVC;
using UnityEngine;

namespace Script.UI.Room
{
    public class MapController : MvcController
    {
        private MapView _view;

        private void Awake()
        {
            _view = GetComponent<MapView>();
            _view.Mogadish.onClick.AddListener(() =>
            {
                SelectMap(MapType.Mogadishu);
                Hide();
            });
            _view.Space.onClick.AddListener(() =>
            {
                SelectMap(MapType.Space);
                Hide();
            });
        }

        public void SelectMap(MapType type)
        {
            ApplicationManager.Instance.RoomManager.SelectMap(type);
        }
    }
}