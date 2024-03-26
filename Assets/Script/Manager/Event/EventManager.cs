using System;
using Script;
using Script.Character; 
using Script.Game.Event;
using Script.Manager.Event;
using Script.Mechanism;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneEvent = Script.Game.Event.SceneEvent;

namespace Game
{

    public class MechanismEvent
    { 
        //Trigger,Triggered
        public Action<GlortonFighter,IDamagable> OnPlayerTriggerMechanism;
        public Action<ExplosiveZone, GlortonFighter> OnPlayerTriggerExplosiveZone;
    }
    public class EventManager : MonoBehaviour
    {
        #region static
        protected static EventManager instance;
        public static EventManager Instance
        {
            get => instance;
        }
        #endregion

        #region Getter
        //combat应该归类为game,以后再改
        public CombatEvent Combat => _combat;
        public MechanismEvent Mechanism => _mechanism;
        public RoomEvent Room => _room;
        public SceneEvent Scene => _scene;
        public GameEvent Game => _game;

        #endregion
        
        public bool debugging;
        
        protected CombatEvent _combat;
        protected MechanismEvent _mechanism;
        protected RoomEvent _room;
        protected SceneEvent _scene;
        protected GameEvent _game;
        private void Awake()
        {
            instance = this;
            _combat = new CombatEvent();
            _room = new RoomEvent();
            _scene = new SceneEvent();
            _game = new GameEvent();
            _mechanism = new MechanismEvent();
        }

        private void Start()
        {
            // var sceneManager = NetworkManager.Singleton.SceneManager;
            // Debug.Log(NetworkManager.Singleton==null);
            // sceneManager.OnLoad += OnSceneLoad;
            // sceneManager.OnUnload += OnSceneUnload;
            //在场景加载的时候是无法访问gameobject的,需要用引子
            // SceneManager.sceneLoaded += OnSceneLoad;
            // SceneManager.sceneUnloaded += OnSceneUnload;
        }

        private void OnSceneUnload(Scene scene)
        {
       
                if (scene.name.StartsWith("Map_"))
                {
                    _scene.OnCombatUnloaded?.Invoke();
                } 
        }

        
    }
}