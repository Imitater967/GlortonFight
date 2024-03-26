using System;
using Game;
using Script.Character;
using Unity.Netcode;
using UnityEngine;

namespace Script.Game.Room
{
    public class RoomPlayer: NetworkBehaviour
    {
        private RoomManager _roomManager;
        public GlortonFighter fighter;
        public FighterAsset FighterAsset => _roomManager.GetFighterAsset(_roomManager.GetPlayerStateById(OwnerClientId).Value);
        public bool LoadReady = false;
        
        private void Awake()
        {
            _roomManager = ApplicationManager.Instance.RoomManager;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                _roomManager.LocalPlayer = this;
                Debug.Log("Successfully create messaging system with server");
            }
        }

        [ServerRpc]
        public void MarkPrepareServerRpc(bool prepared)
        {
            var stateVar = _roomManager.GetPlayerStateById(OwnerClientId);
            var state=stateVar.Value;
            state.Prepared = prepared;
            stateVar.Value= state;
        }
        [ServerRpc]
        public void SelectFighterServerRpc(FighterType componentType)
        {
            EventManager.Instance.Room.OnClientRequestFighter?.Invoke(OwnerClientId,componentType);
        }
        [ServerRpc]
        public void StartServerRpc()
        {
            if(!IsHost)
                Debug.LogError("Trying to start game but not host");
            bool canStart = _roomManager.CanStartGame();
            if (!canStart)
            {
                Debug.LogWarning("Fail To Start The Game");
                return;
            }
            Debug.Log("Game Started Selecting Map");
            EventManager.Instance.Room.OnStart?.Invoke();
        }
        [ServerRpc]
        public void LoadReadyServerRpc()
        {
            LoadReady = true;
        }
        [ClientRpc]
        public void SendFighterInitialDataClientRpc(NetworkBehaviourReference glortonFighter,short health)
        {
            glortonFighter.TryGet(out GlortonFighter gf);
            this.fighter = gf;
            var gfIndex = _roomManager.GetPlayerStateById(OwnerClientId).Value.Index;
            gf.gameObject.layer = Utils.GetLayerByIndex(gfIndex);
            // Debug.Log(gfIndex);
            // Debug.Log("P"+(gfIndex+1));
            // Debug.Log(LayerMask.NameToLayer("P"+(gfIndex+1)));
            // Debug.Log("Layer value"+ Utils.GetLayerByIndex(gfIndex));
            // Debug.Log("Editing "+gfIndex);
            // Debug.Log("new layer"+gf.gameObject.layer);
            foreach (var componentsInChild in gf.GetComponentsInChildren<SpriteRenderer>())
            {
                componentsInChild.sortingLayerID = Utils.GetSortingLayer(gfIndex);
            }
            ApplicationManager.Instance.EventManager.Game.OnPlayerHealthChangeClient?.Invoke(gf,health);
        }

        [ClientRpc]
        public void SendDeathEffectClientRpc(Vector3 pos, Quaternion rotation)
        {
            ApplicationManager.Instance.EventManager.Game.OnPlayerDeathEffectClient?.Invoke(pos, rotation);
        }
    }
}