using System;
using System.Collections.Generic;
using Game;
using Script.Character;
using Script.Game.Room.Map;
using Script.UI.Room;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Script.Game.Room
{
    //备注,我们不需要有房主这个概念哟,server就是房主
    public partial class RoomManager : NetworkBehaviour
    {
        [Header("房间设置")]
        [SerializeField]
        private NetworkVariable<RoomSetting> _roomSetting = new NetworkVariable<RoomSetting>(
            readPerm: NetworkVariableReadPermission.Everyone,
            writePerm: NetworkVariableWritePermission.Server);

        public bool CanJoin;
        [SerializeField]
        private string _fallbackScene="Scenes/Combat/Map_Demo";

        [SerializeField] private RoomSetting DefaultRoomSetting;
        public RoomSetting RoomSetting => _roomSetting.Value;
        public const short MAX_STOCK=50;
        public const short MIN_STOCK=1;
        public const byte MAX_TIME=10;
        public const short MIN_TIME=1;
        [Header("地图")]
        [SerializeField]
        private NetworkVariable<MapType> _mapType = new NetworkVariable<MapType>();

        [Header("房间内玩家设")] public int MinPlayerToStart = 1;
        [SerializeField]
        private NetworkVariable<RoomPlayerState> _playerAState  = new NetworkVariable<RoomPlayerState>(); 
        [SerializeField]
        private NetworkVariable<RoomPlayerState> _playerBState  = new NetworkVariable<RoomPlayerState>(); 
        [SerializeField]
        private NetworkVariable<RoomPlayerState> _playerCState  = new NetworkVariable<RoomPlayerState>(); 
        [SerializeField]
        private NetworkVariable<RoomPlayerState> _playerDState  = new NetworkVariable<RoomPlayerState>();
        [Header("角色列表")]
        public List<FighterAsset> StrawBerries;
        public List<FighterAsset> Peaches;
        [FormerlySerializedAs("Trash")] public List<FighterAsset> Trashs;
        [FormerlySerializedAs("Coffee")] public List<FighterAsset> Coffees;
        [FormerlySerializedAs("Ball")] public List<FighterAsset> Balls;
        [FormerlySerializedAs("Aubergine")] public List<FighterAsset> Aubergines;
        [Header("通信")]
        [SerializeField] private RoomPlayer _roomPlayerPrefab;
        public RoomPlayer LocalPlayer;
        [Header("UI")] public RoomController RoomController;
        [Header("其他")]
        [NonSerialized]
        public List<FighterAsset> AllFighter; 
        [NonSerialized]
        public List<NetworkVariable<RoomPlayerState>> AllState; 
        private void Awake()
        {
            //Singleton
            if (ApplicationManager.Instance.RoomManager != null)
            {
                Debug.LogError("Multi Room Manager Detected");
                Destroy(gameObject);
                return;
            }
            ApplicationManager.Instance.RoomManager = this; 
            //INIT VAR PlayerSelect
            AllFighter = new List<FighterAsset>();
            AllFighter.AddRange(StrawBerries);
            InitIndexAndType(StrawBerries, FighterType.StrawBerry);
            AllFighter.AddRange(Peaches);
            InitIndexAndType(Peaches, FighterType.Peach);
            AllFighter.AddRange(Trashs);
            InitIndexAndType(Trashs, FighterType.Trash);
            AllFighter.AddRange(Coffees);
            InitIndexAndType(Coffees, FighterType.Coffee);
            AllFighter.AddRange(Balls);
            InitIndexAndType(Balls, FighterType.Ball);
            AllFighter.AddRange(Aubergines);
            InitIndexAndType(Aubergines, FighterType.Aubergine);
            AllState = new List<NetworkVariable<RoomPlayerState>>()
            {
                _playerAState,
                _playerBState,
                _playerCState,
                _playerDState,
            }; 
            //RESET
            ResetSelectionStage();
            //EVENTS
            foreach (var networkVariable in AllState)
            { 
                networkVariable.OnValueChanged += (a, b) =>
                {
                    EventManager.Instance.Room.OnPlayerStateChange?.Invoke(a,b);
                };
            }

            _mapType.OnValueChanged += (a, b) =>
            {
                EventManager.Instance.Room.OnHostChangeMap?.Invoke(b);
            };
            _roomSetting.OnValueChanged += (a, b) =>
            {
                EventManager.Instance.Room.OnSettingChanged?.Invoke(a,b);
            };

            NetworkManager.Singleton.OnClientConnectedCallback += AssignState; 
            NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerQuit;
           
            EventManager.Instance.Room.OnClientRequestFighter += SelectFighter;
            // EventManager.Instance.Room.OnStart += HideAllUI;
            EventManager.Instance.Room.OnStart += LoadGameScene;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            //数值初始化
            if (IsServer)
            { 
                //房间设置 
                _roomSetting.Value = DefaultRoomSetting; 
                for (byte i = 0; i < AllState.Count; i++)
                {
                    AllState[i].Value = new RoomPlayerState()
                    {
                        Index = i,
                        Selected = false
                    };
                }
            }
            //事件激活
            EventManager.Instance.Room.OnRoomInit?.Invoke(IsServer);
            for (byte i = 0; i < AllState.Count; i++)
            {
                var playerState = AllState[i];
                EventManager.Instance.Room.OnPlayerStateChange?.Invoke(playerState.Value,playerState.Value);
            }
            EventManager.Instance.Room.OnSettingChanged?.Invoke(RoomSetting,RoomSetting);
            EventManager.Instance.Room.OnHostChangeMap?.Invoke(_mapType.Value);
            foreach (var networkVariable in AllState)
            {
                EventManager.Instance.Room.OnPlayerStateChange?.Invoke(networkVariable.Value,networkVariable.Value);
            }
           
        }

 
        #region Events

        private RoomPlayerState CreateMessageSystem(RoomPlayerState id)
        { 
            var obj=Instantiate(_roomPlayerPrefab, transform).GetComponent<NetworkObject>();
            obj.SpawnWithOwnership(id.ClientId);
            obj.TrySetParent(gameObject);
            obj.name = "player-" + id.ClientId;
            id.Player=obj.GetComponent<RoomPlayer>();
            return id;
        }
        private void AssignState(ulong id)
        { 
            if(!IsServer)
                return;
            for (var i = 0; i < AllState.Count; i++)
            {
                var playerState = AllState[i].Value;
                if (playerState.Connected == false)
                {
                    playerState=ResetPlayerState(ref playerState);
                    playerState.Connected = true;
                    playerState.ClientId = id;
                    playerState = CreateMessageSystem(playerState);
                    AllState[i].Value = playerState;
                    Debug.Log("Player "+id+" Joined With Index "+playerState.Index);
                    return;
                }
            }
        }

        private void OnPlayerQuit(ulong id)
        {
            if(!IsServer)
                return;
            for (var i = 0; i < AllState.Count; i++)
            {
                var state = AllState[i].Value;
                if (state.Connected&&state.ClientId==id)
                {
                    state=ResetPlayerState(ref state); 
                    state.Connected = false;  
                    AllState[i].Value = state;
                }
            }
        }

        private RoomPlayerState ResetPlayerState(ref RoomPlayerState playerState)
        {
            playerState.Prepared = false;
            playerState.Selected = false; 
            return playerState;
        }
        #endregion

        #region Helpers

 

        public NetworkVariable<RoomPlayerState> GetPlayerStateByIndex(byte index)
        {
            return AllState[index];
        }
        public NetworkVariable<RoomPlayerState> GetPlayerStateById(ulong id)
        {
            foreach (var networkVariable in AllState)
            {
                if (networkVariable.Value.ClientId == id)
                    return networkVariable;
            }

            return null;
        }
 

        private void InitIndexAndType(List<FighterAsset> fighters, FighterType type)
        {
            for (byte i = 0; i < fighters.Count; i++)
            {
                fighters[i].Index = i;
                fighters[i].Type = type;
            }
        }

        public FighterAsset GetFirstAvailableFighter(FighterType type)
        {
            return GetFirstAvailableFighter(
                GetFighters(type));
        }
        public List<FighterAsset> GetFighters(FighterType type)
        {
            switch (type)
            {
                case FighterType.StrawBerry:
                    return StrawBerries;
                case FighterType.Peach:
                    return Peaches;
                case FighterType.Trash:
                    return Trashs;
                case FighterType.Coffee:
                    return Coffees;
                case FighterType.Ball:
                    return Balls;
                case FighterType.Aubergine:
                    return Aubergines;
            }

            return null;
        }
        private void ResetSelectionStage()
        { 
            foreach (var fighterAsset in AllFighter)
            {
                fighterAsset.Selected = false;
            }
        }
        
        public bool CanStartGame()
        {
            int player = 0;
            foreach (var networkVariable in AllState)
            {
                var state = networkVariable.Value;
                if (state.Connected)
                {
                    player += 1;
                }

                if (state.Connected)
                {
                    //忽略房主,并且是连接了的而且已准备了的
                    if (state.Index!=0&&state.Prepared == false)
                    {
                        return false;
                    }
                    //有一个没有选角色就不可以开始
                    if ( !state.Selected)
                        return false;
                }
            }
            if (player<MinPlayerToStart)
            {
                return false;
            }

            return true;
        }
        
        public void SelectMap(MapType type)
        {
            if (!IsServer)
            {
                Debug.LogWarning("client trying to select map");
            }else
                _mapType.Value = type;
        }
        #endregion


    }
}