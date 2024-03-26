using System;
using System.Collections;
using Game;
using Script.Game.Room;
using Script.Game.Room.Map;
using Script.MVC;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Script.UI.Room
{
    public class RoomController : MvcController
    {
        public RoomManager RoomManager;
        protected RoomView _view;
        private UnityAction MarkReadyAction;
        private UnityAction CancelReadyAction;
        public MapController MapController;
        [Header("Map Preview")] 
        public Sprite Mogadishu;

        public Sprite Space;
        
        private void Awake()
        {
            CancelReadyAction += CancelReady;
            MarkReadyAction += MarkReady;
            _view = GetComponent<RoomView>();
            _view.returnBnt.onClick.AddListener(OnReturnBntPressed);
            _view.settingSwapButton.onClick.AddListener(SwitchMode);
            _view.settingAddButton.onClick.AddListener(AddValue);
            _view.settingReduceButton.onClick.AddListener(ReduceValue); 
            _view.readyOrStartButton.interactable = false;
            _view.readyText.color = new Color(0.5f, 0.5f, 0.5f, 1);
            
            EventManager.Instance.Scene.OnCombatLoaded += Hide;
            EventManager.Instance.Scene.OnCombatUnloaded += Show;
            EventManager.Instance.Room.OnRoomInit += UpdateSettingButton;
            EventManager.Instance.Room.OnRoomInit += InitPrepareButton;
            EventManager.Instance.Room.OnRoomInit += InitMapSelectButton;
            EventManager.Instance.Room.OnSettingChanged += (old,a) =>
            {
                UpdateMode(a.DeathMatch);
                UpdateBntStatus(a);
                UpdateModeValue(a);
            };
            EventManager.Instance.Room.OnPlayerStateChange += TogglePrepareStatus;
            EventManager.Instance.Room.OnPlayerStateChange += CheckCanStart;
            EventManager.Instance.Room.OnHostChangeMap += ChangeMapPreview;

        }
        
        private void ChangeMapPreview(MapType obj)
        {
            Sprite sprite = null;
            switch (obj)
            {
                case MapType.Mogadishu:
                    sprite = Mogadishu;
                    break;
                case MapType.Space:
                    sprite = Space;
                    break;
            }

            _view.mapPreview.sprite = sprite;
        }


        private void CheckCanStart(RoomPlayerState a, RoomPlayerState state)
        {
            if(!RoomManager.IsServer)
                return;
            _view.readyText.text = "Start";
            if (RoomManager.CanStartGame())
            {
                _view.readyOrStartButton.interactable = true;
                _view.readyText.color = new Color(1f, 1f, 1f, 1);
            }
            else
            {
                _view.readyOrStartButton.interactable = false;
                _view.readyText.color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
        }
        
        private void InitMapSelectButton(bool obj)
        {
            if (RoomManager.IsServer)
            {
                _view.mapSelectBnt.interactable = true;
                _view.mapSelectBnt.onClick.AddListener(MapController.Show);
            }
            else
            {
                _view.mapSelectBnt.interactable = false;
            }
        }

        private void TogglePrepareStatus(RoomPlayerState arg1, RoomPlayerState arg2)
        { 
            if(arg2.ClientId!=NetworkManager.Singleton.LocalClientId)
                return;
            //服务器端是开始
            if(RoomManager.IsServer)
                return;
            if (arg2.Selected)
            {
                _view.readyOrStartButton.interactable = true;
                
                _view.readyText.color = new Color(1f, 1f, 1f, 1);
            }
            else
            {
                return;
            }
            if (arg2.Prepared)
            {
                _view.readyText.text = "取消准备"; 
                _view.readyOrStartButton.onClick.RemoveListener(MarkReadyAction);
                _view.readyOrStartButton.onClick.AddListener(CancelReadyAction);
            }            
            if (!arg2.Prepared)
            {
                _view.readyText.text = "准备"; 
                _view.readyOrStartButton.onClick.AddListener(MarkReadyAction);
                _view.readyOrStartButton.onClick.RemoveListener(CancelReadyAction);
            }
        }

        private void OnReturnBntPressed()
        { 
            NetworkManager.Singleton.Shutdown(true);
            SceneManager.LoadScene("Scenes/Lobby");
        }

        #region From Model
        
        #endregion
        #region To Model
        //mark ready和cancel ready应该是两个RPC
        private void StartGame()
        {
            RoomManager.LocalPlayer.StartServerRpc();
        }
        private void MarkReady()
        {
            RoomManager.LocalPlayer.MarkPrepareServerRpc(true);
        }

        private void CancelReady()
        {
            RoomManager.LocalPlayer.MarkPrepareServerRpc(false);
        }
        //修改RoomManger中的设置
        private void SwitchMode()
        {
            var setting = RoomManager.RoomSetting;
            setting.DeathMatch = !setting.DeathMatch;
            RoomManager.UpdateRoomSetting(setting);
        }
        //添加设置数值, 如果时间就增加一分钟,如果生命就加1
        private void AddValue()
        {
            var setting = RoomManager.RoomSetting;
            if (setting.DeathMatch)
            {
                setting.Stock += 1;
            }
            else
            {
                setting.Time += 1;
            }
            RoomManager.UpdateRoomSetting(setting);
        }
        private void ReduceValue()
        {
            var setting = RoomManager.RoomSetting;
            if (setting.DeathMatch)
            {
                setting.Stock -= 1;
            }
            else
            {
                setting.Time -= 1;
            }
            RoomManager.UpdateRoomSetting(setting);
        }
        #endregion

        #region Methods
        public override void Hide()
        {
            if(gameObject!=null)
                gameObject.SetActive(false);
        } 
        public override void Show()
        {
            if(gameObject!=null)
                gameObject.SetActive(true);
        }
        
        private void InitPrepareButton(bool isServer)
        {
            if (!isServer)
            {
                _view.readyOrStartButton.onClick.AddListener(MarkReadyAction);

            }
            else
            {
                _view.readyOrStartButton.onClick.AddListener(StartGame);
            }
        }
        private void UpdateSettingButton(bool enable)
        {
            var bnts = new[] {_view.settingAddButton,_view.settingReduceButton,_view.settingSwapButton };
            if (enable)
            {
                foreach (var button in bnts)
                {
                    button.interactable = true;
                }
            }
            else
            {
                foreach (var button in bnts)
                {
                    button.interactable = false;
                }
            }
        }

        private void UpdateMode(bool deathMatch)
        {
            if (deathMatch)
            {
                _view.settingModeText.text = "决一死战"; 
            }
            else
            {
                _view.settingModeText.text = "计时赛";
            }
        }
        
        private void UpdateBntStatus(RoomSetting setting)
        {
            if (!RoomManager.IsServer)
            {
                return;
            }
            _view.settingAddButton.interactable = true;
            _view.settingReduceButton.interactable = true;
            
            if (setting.DeathMatch)
            {
                if (setting.Stock >= RoomManager.MAX_STOCK)
                {
                    _view.settingAddButton.interactable = false;
                }

                if (setting.Stock <= RoomManager.MIN_STOCK)
                {
                    _view.settingReduceButton.interactable = false;
                }
            }
            else
            {
                if (setting.Time >= RoomManager.MAX_TIME)
                {
                    _view.settingAddButton.interactable = false;
                }

                if (setting.Time <= RoomManager.MIN_TIME)
                {
                    _view.settingReduceButton.interactable = false;
                }
            }
        }
        private void UpdateModeValue(RoomSetting setting)
        {
            if (setting.DeathMatch)
            {
                _view.settingValueText.text = setting.Stock.ToString();
            }
            else
            {
                _view.settingValueText.text = setting.Time.ToString()+":00";
            }
        }

        #endregion
    }
}