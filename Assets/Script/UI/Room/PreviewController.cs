using System;
using Game;
using Script.Game;
using Script.Game.Room;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI.Room
{
    public class PreviewController : MonoBehaviour
    {
        private PreviewView _view;
        private RoomManager _manager;
        private void Awake()
        {
            _view = GetComponent<PreviewView>();
            //player A 固定是房主
            //_view.playerAStatus.gameObject.SetActive(false);
            _view.playerBStatus.gameObject.SetActive(false);
            _view.playerCStatus.gameObject.SetActive(false);
            _view.playerDStatus.gameObject.SetActive(false);
            //Model->Controller
            EventManager.Instance.Room.OnPlayerStateChange+=UpdatePreviewImage;
        }

        private void Start()
        {
            _manager = ApplicationManager.Instance.RoomManager;
        }

        #region Event->Controller->View
        //更新封面
        //需要注意的是selected
        private void UpdatePreviewImage(RoomPlayerState arg1, RoomPlayerState arg2)
        {
            //房主的文字不需要修改
            if (arg2.Index != 0)
            {
                GetPlayerTextByIndex(arg2.Index).gameObject.SetActive(arg2.Prepared);
            }
            var image=GetPlayerImageByIndex(arg2.Index);
            image.gameObject.SetActive(arg2.Selected&&arg2.Connected);
            //index由服务器决定,我们不需要干预
            //因为fighterAsset.selected只存在与服务器上
            var fighterAsset = _manager.GetFighterAsset(arg2);
            image.sprite = fighterAsset.Preview;
        }

        #endregion
        #region From Model

        #endregion
        #region helper

        public Image GetPlayerImageByIndex(byte index)
        {
            switch (index)
            {
                case 0:
                    return _view.playerAImage;
                case 1:
                    return _view.playerBImage;
                case 2:
                    return _view.playerCImage;
                case 3:
                    return _view.playerDImage;
            }
            return null;
        }
        public TMP_Text GetPlayerTextByIndex(byte index)
        {
            switch (index)
            {
                case 0:
                    return _view.playerAStatus;
                case 1:
                    return _view.playerBStatus;
                case 2:
                    return _view.playerCStatus;
                case 3:
                    return _view.playerDStatus;
            }
            return null;
        }


        #endregion
    }
}