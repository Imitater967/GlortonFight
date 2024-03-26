using System;
using Script.Character;
using Script.Game.Room;
using Unity.Netcode;
using UnityEngine;

namespace Script.Manager.Event
{
    public class GameEvent
    {
        public Action OnPrepared;
        public Action<byte> OnPrepareCountdown;
        public Action<int> OnMatchCountdown;
        public Action OnStart;
        public Action<RoomPlayerState, FighterAsset,GlortonFighter> OnPlayerSpawnServer;
        public Action<GlortonFighter> OnPlayerSpawnClient;
        public Action<GlortonFighter> OnPlayerDeathServer;
        public Action<GlortonFighter,Vector3> OnPlayerDeathClient;
        public Action<GlortonFighter> OnPlayerRespawnServer;
        public Action<GlortonFighter> OnPlayerRespawnClient;
        public Action<GlortonFighter,short> OnPlayerHealthChangeClient;
        public Action<Vector3, Quaternion> OnPlayerDeathEffectClient;
        
    }
}