using System;
using Game;
using Script.Character;
using Script.Game;
using Script.Game.Room;
using Unity.Netcode;
using UnityEngine;

namespace Script.UI.Room
{
    public class SelectViewController : MonoBehaviour
    {
        public Coin Coin;
        public Transform DragParent;
        private RoomManager roomManager;

        private void Awake()
        {
            Coin.gameObject.SetActive(false);
            Coin.DragParent = DragParent;
        }

        private void Start()
        {
            roomManager = ApplicationManager.Instance.RoomManager;
            EventManager.Instance.Room.OnRoomInit += (b) =>
            {
                Coin.gameObject.SetActive(true);
            };
        }

        //View->Controller
        public void Select(FighterType componentType)
        {
            roomManager.LocalPlayer.SelectFighterServerRpc(componentType);
        }
    }
}