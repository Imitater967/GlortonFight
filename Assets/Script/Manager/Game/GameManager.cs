using System;
using System.Collections;
using Cinemachine;
using Game;
using Script.Character;
using Script.Game.Room;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Script.Game
{
    public class GameManager: NetworkBehaviour
    {
        private RoomManager _roomManager;
        private EventManager _eventManager;
        public CinemachineTargetGroup TargetGroup; 
        public PolygonCollider2D MapBounds;
        public byte Countdown = 5;
        public Transform[] SpawnPoints;
        protected CombatSystem _combatSystem;
        protected MechanismSystem _mechanismSystem;
        protected CameraSystem _cameraSystem;
        protected ExplodeSystem _explodeSystem;
        protected global::Game.System[] systems;
        
        public bool Prepared = false;
        [SerializeField] private NetworkVariable<int> _time = new NetworkVariable<int>();
        

        private Coroutine _matchCountdownTask;
        private void Awake()
        {
            ApplicationManager.Instance.GameManager = this;
            _roomManager = ApplicationManager.Instance.RoomManager;
            _eventManager = ApplicationManager.Instance.EventManager;
            _combatSystem = gameObject.AddComponent<CombatSystem>();
            _explodeSystem = gameObject.AddComponent<ExplodeSystem>();
            _mechanismSystem = gameObject.AddComponent<MechanismSystem>();
            _cameraSystem = gameObject.AddComponent<CameraSystem>();
            _cameraSystem.TargetGroup = TargetGroup;
            systems = new global::Game.System[] { _combatSystem ,_mechanismSystem,_explodeSystem,_cameraSystem};
            for (var i = 0; i < systems.Length; i++)
            {
                //服务器生成玩家后,我们才调用客户端的OnGameInit 
                systems[i].OnGameInit();
            }
            _roomManager.LocalPlayer.LoadReadyServerRpc();
            _eventManager.Game.OnPlayerDeathServer += OnPlayerDeath;
            _eventManager.Game.OnPrepareCountdown += (a) =>
            {
                if (a == 0)
                {
                    ApplicationManager.Instance.InputManager.SwitchToIngame();
                }
            };
        }

        #region Events

        
        private void OnPlayerDeath(GlortonFighter obj)
        { 
            
            Debug.Log(obj.name+"已经死亡-Server");
            if(obj.combat.Health>0)
                StartCoroutine(RespawnTask(obj));
            
            _roomManager.GetPlayerStateById(obj.OwnerClientId).Value.Player.TryGet(out RoomPlayer roomPlayer);
             
            Vector3 pos = obj.transform.position;
            PolygonCollider2D mapBound=ApplicationManager.Instance.GameManager.MapBounds;
            Vector2 normal=mapBound.bounds.ClosestPoint(pos) - pos; 
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
            // SpawnDeathEffect(pos, rotation);
            roomPlayer.SendDeathEffectClientRpc(pos,rotation); 

        }
        protected IEnumerator RespawnTask(GlortonFighter obj)
        { 
            yield return new WaitForSeconds(1); 
            var spawnTransform = SpawnPoints[Random.Range(0, 3)]; 
            obj.gameObject.transform.position = spawnTransform.position;
            obj.gameObject.SetActive(true);
            EventManager.Instance.Game.OnPlayerRespawnServer?.Invoke(obj);
            SendRespawnEventClientRpc(obj);
            obj.Dead = false;
            Debug.Log(obj.name+"已重生-Server");
        }


        #endregion

        [ClientRpc]
        private void SendRespawnEventClientRpc(NetworkBehaviourReference reference)
        {
            if (reference.TryGet(out GlortonFighter fighter))
            {
                fighter.gameObject.SetActive(true); 
                EventManager.Instance.Game.OnPlayerRespawnClient?.Invoke(fighter);
            }
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _time.Value = _roomManager.RoomSetting.Time * 60;
            _time.OnValueChanged += (a, b) =>
            {
                _eventManager.Game.OnMatchCountdown?.Invoke(b); 
            };
            SendMatchTimeClientRpc(_time.Value);
            
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            ApplicationManager.Instance.InputManager.SwitchToUI();
        }

        [ClientRpc]
        private void SendMatchTimeClientRpc(int time)
        {
            _eventManager.Game.OnMatchCountdown?.Invoke(_time.Value);
        }
        private void Update()
        {
            TryPrepareGame();
            if (IsServer&&Prepared&&_matchCountdownTask==null&& !_roomManager.RoomSetting.DeathMatch)
            {
               _matchCountdownTask= StartCoroutine(MatchCountdownTask());
            }
        }

        #region 游戏准备
        private void TryPrepareGame()
        {
            if (!Prepared&&IsServer)
            {
                for (var i = 0; i < _roomManager.AllState.Count; i++)
                {
                    //LoadReady只存在与服务器中
                    if (_roomManager.AllState[i].Value.Connected&&!((RoomPlayer)_roomManager.AllState[i].Value.Player).LoadReady)
                    {
                        return;
                    }
                }
                SpawnFighters();
                Prepared = true;
                for (var i = 0; i < systems.Length; i++)
                {
                    //服务器生成玩家后,我们才调用客户端的OnGameInit 
                    systems[i].OnGamePrepared();
                }
                PrepareGameClientRpc();
            }
        }
        [ClientRpc]
        private void PrepareGameClientRpc()
        {    
            
            Debug.Log("所有玩家均加载完毕,开始游戏倒计时");
            var playerState=(RoomPlayerState)_roomManager.GetPlayerStateByIndex(0).Value;
            var tryGet = (playerState.Player.TryGet(out RoomPlayer fighter)); 
            _eventManager.Game.OnPrepared?.Invoke(); 
            StartCoroutine(PrepareCountdownTask());
        }
        private IEnumerator PrepareCountdownTask()
        {
            byte currentTime = Countdown;
            while (currentTime>0)
            {
                currentTime -= 1;
                //Model->Event->Controller->View
                _eventManager.Game.OnPrepareCountdown?.Invoke(currentTime);
                yield return new WaitForSeconds(1);
            }
            _eventManager.Game.OnStart?.Invoke();
        }
             
        private void SpawnFighters()
        {
            for (var i = 0; i < _roomManager.AllState.Count; i++)
            {
                var state = _roomManager.AllState[i].Value;
                if (state.Connected)
                {
                    var asset=_roomManager.GetFighterAsset(state);
                    var fighterObj=Instantiate(asset.Fighter).GetComponent<NetworkObject>();
                    //这个函数只对服务器有用,以后需要注意
                    SceneManager.MoveGameObjectToScene(fighterObj.gameObject,gameObject.scene);
                    fighterObj.transform.position = SpawnPoints[i].position;
                    fighterObj.SpawnWithOwnership(state.ClientId);
                    //0~3,不需要做额外检查,因为是服务器的设置
                    var glortonFighter = fighterObj.GetComponent<GlortonFighter>();
                    if (_roomManager.RoomSetting.DeathMatch)
                    {
                        glortonFighter.combat.Health = _roomManager.RoomSetting.Stock;
                    }
                    ((RoomPlayer)state.Player).SendFighterInitialDataClientRpc(glortonFighter,glortonFighter.combat.Health);
                    _eventManager.Game.OnPlayerSpawnServer?.Invoke(state,asset,glortonFighter);
                    continue;
                }
                Debug.Log("Player index "+i+" not connected skipping");
            }

        }

        private IEnumerator MatchCountdownTask()
        {
            while (_time.Value>0)
            {
                yield return new WaitForSeconds(1);
                _time.Value -= 1;
            }
        }

        #endregion


        
    }
}