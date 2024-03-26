using System;
using Unity.Netcode;
using UnityEngine;

namespace Script.Game.Room
{
    [Serializable]
    public struct RoomSetting : INetworkSerializable
    {
        //1~50
        public short Stock;
        //1~10
        public byte Time;
        //true->Stock,false->Time
        public bool DeathMatch;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Stock);
            serializer.SerializeValue(ref Time);
            serializer.SerializeValue(ref DeathMatch);
        }
    }
    public partial class RoomManager
    {
        //Run only on host
        public void UpdateRoomSetting(RoomSetting cur)
        {
            if (!IsServer)
            {
                Debug.LogError("Update Room Setting with not perm");
                return;
            }
            if (cur.Stock > MAX_STOCK)
            {
                cur.Stock = MAX_STOCK;
            }

            if (cur.Stock<MIN_STOCK)
            {
                cur.Stock = MIN_STOCK;
            }

            if (cur.Time < MIN_TIME)
            {
                cur.Stock = MIN_TIME;
            }

            if (cur.Time > MAX_TIME)
            {
                cur.Time = MAX_TIME;
            }

            _roomSetting.Value = cur;
        }
        
    }
}