// using System;
// using Script.Game;
// using Script.Game.Room;
// using Unity.Netcode;
// using UnityEngine;
//
// namespace Script.UI.Room
// {
//     public class CoinSlot : NetworkBehaviour
//     {
//         public Coin Coin;
//         public Coin CoinPrefab;
//         public RoomPlayer Belong;
//         public Transform DragParent; 
//         public override void OnNetworkSpawn()
//         {
//             base.OnNetworkSpawn(); 
//         }
//         /**
//          * 生成并配置后发送InitRpc,初始化硬币
//          * 现在的问题是,后面加入的玩家,无法收到这个Rpc
//          * 那么我们可以采用NetworkVar,生成后
//          */
//         private void Update()
//         {
//             if(Belong==null||!IsServer)
//                 return;
//             if (Coin==null&&Belong.Connected)
//             {
//                 Coin = Instantiate(CoinPrefab).GetComponent<Coin>();
//                 Coin.NetworkObject.SpawnWithOwnership(Belong.Id);
//                 SendLocalCoinClientRpc(Coin);
//                 Coin.NetworkObject.TrySetParent(DragParent);
//                 Coin.transform.position=transform.position; 
//                 Coin.SlotRef.Value = this;
//             } else if (Coin!=null&&!Belong.Connected)
//             {
//                 Coin.NetworkObject.Despawn();
//             }
//         }
//
//         [ClientRpc]
//         private void SendLocalCoinClientRpc(NetworkBehaviourReference coinRef)
//         {
//  
//             Coin coin = (Coin)coinRef;
//             if (NetworkManager.Singleton.LocalClientId == coin.OwnerClientId)
//             {
//                 ApplicationManager.Instance.RoomManager.LocalCoin = coin;
//             }
//         }
//     }
// }