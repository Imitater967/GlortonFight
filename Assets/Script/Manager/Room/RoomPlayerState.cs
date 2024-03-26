using System;
using Script.Character;
using Unity.Netcode;

namespace Script.Game.Room
{
    [Serializable]
    public struct RoomPlayerState: INetworkSerializable
    {
        public ulong ClientId; 
        public byte Index;
        public FighterType FighterClass;
        public byte FighterIndex;
        public bool Selected;
        public bool Prepared;       
        public bool Connected;
        public NetworkBehaviourReference Player;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {  
            serializer.SerializeValue(ref Player);
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref Index);
            serializer.SerializeValue(ref FighterClass);
            serializer.SerializeValue(ref FighterIndex);
            serializer.SerializeValue(ref Selected);
            serializer.SerializeValue(ref Prepared);
            serializer.SerializeValue(ref Connected);
        }
    }
}