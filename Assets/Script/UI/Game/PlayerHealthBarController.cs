using System;
using Game;
using Script.Character;
using Script.Game;
using Script.Game.Room;
using Script.MVC;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI.Game
{
    /*
     * View-> nothing->controller
     * Model->EventSystem->Controller->View
     */
    public class PlayerHealthBarController : MvcController
    {
        private PlayerHealthBarView _view;
        private EventManager _eventManager;
        private RoomManager _roomManager;
        private GameManager _gameManager;
        public Animator Animator;
        public byte Index;
        private bool _deathMatch=>_roomManager.RoomSetting.DeathMatch;
        private void Awake()
        {
            _view = GetComponent<PlayerHealthBarView>();
            _view.Above5Style.SetActive(false);
            _view.Under5Style.SetActive(false);
            _view.ScoreStyle.SetActive(false);

            _eventManager = ApplicationManager.Instance.EventManager;
            _gameManager = ApplicationManager.Instance.GameManager;
            _roomManager = ApplicationManager.Instance.RoomManager;
            _eventManager.Game.OnPlayerHealthChangeClient += CheckHealth;
            var roomPlayerState = _roomManager.GetPlayerStateByIndex(Index).Value;
            _eventManager.Game.OnPrepared += () =>
            { 
                bool connected = roomPlayerState.Connected;
                if (!connected)
                {
                    Hide();
                    return;
                }

                var fighterAsset = _roomManager.GetFighterAsset(roomPlayerState);
                foreach (var viewHealthSlot in _view.HealthSlots)
                {
                    viewHealthSlot.sprite = fighterAsset.HealthSlot;
                }
                _view.Background.sprite = fighterAsset.HealthBarBG;
            };
            _eventManager.Game.OnPrepared += () =>
            {
                if(!roomPlayerState.Connected)
                    return; 
                if (_deathMatch)
                {  
                    var fighter = ((RoomPlayer)roomPlayerState.Player).fighter; 
                }
                else
                { 
                    _view.ScoreStyle.SetActive(true);
                }
            };
            _eventManager.Combat.OnPlayerReceiveDamageClient += (player, amount) =>
            {
                if (Index!=_roomManager.GetPlayerStateById(player.OwnerClientId).Value.Index)
                {
                    return;
                }
                Animator.CrossFade("UIAnim",0);
                this._view.DamageAmount.text = (amount*100).ToString("F0") + "%";
            };
        }

        private void CheckHealth(GlortonFighter obj, short s)
        { 
            if (_roomManager.GetPlayerStateById(obj.OwnerClientId).Value.Index != Index)
            { 
                 return;
            }

            if (!_deathMatch)
            { 
                return;
            } 
            var health = s;
            bool above5 = health > 5;
            _view.Above5Style.SetActive(above5);
            _view.Under5Style.SetActive(!above5);
            if (above5)
            {
                _view.Above5Text.text = "x " + health;
            }
            else
            {
                for (var i = 0; i < _view.Under5Slots.Length; i++)
                { 
                    if (i >= health)
                    { 
                        _view.Under5Slots[i].SetActive(false);
                    }
                }
            }
        }
    }
}