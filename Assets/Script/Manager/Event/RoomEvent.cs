using System;
using Script.Character;
using Script.Game.Room;
using Script.Game.Room.Map;

namespace Script.Game.Event
{
    public class RoomEvent
    { 
        public Action<RoomSetting,RoomSetting> OnSettingChanged;
        public Action<bool> OnRoomInit;
        public Action<RoomPlayerState, RoomPlayerState> OnPlayerStateChange;
        public Action<ulong,FighterType> OnClientRequestFighter;
        public Action OnStart;
        public Action<MapType> OnHostChangeMap;
    }
}