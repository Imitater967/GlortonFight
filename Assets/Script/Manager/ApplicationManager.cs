using System;
using Game;
using Script.Game.Room;
using Script.Manager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Game
{
    public class ApplicationManager: MonoBehaviour
    {
        [NonSerialized]
        public InputReader InputReader;
        [NonSerialized]
        public PlayerInput PlayerInput;
        [NonSerialized]
        public SoundManager SoundManager;
        [NonSerialized]
        public FXManager FXManager;
        [NonSerialized] 
        public EventManager EventManager;
        [NonSerialized] 
        public InputManager InputManager;
        
        [NonSerialized] public GameManager GameManager;
        [NonSerialized] public RoomManager RoomManager;
        [Header("Manager设置")]
        public SoundSetting SoundSetting;
        public MusicSetting MusicSetting;
        public FXSetting FXSetting;
        [Header("输入设置")]
        [SerializeField]
        protected GameObject _inputPrefab;
        protected GameObject _inputObject;
        public static ApplicationManager Instance { get; set; }
        
        public const string INPUT_SAVE_KEY = "Input";
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Application.targetFrameRate = 30;
            Instance = this; 
            DontDestroyOnLoad(gameObject);
            
            _inputObject=Instantiate(_inputPrefab, transform);
            InputReader = _inputObject.GetComponent<InputReader>();
            PlayerInput = _inputObject.GetComponent<PlayerInput>();
            PlayerInput.actions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(INPUT_SAVE_KEY));

            InputManager = gameObject.AddComponent<InputManager>();
            InputManager.Init(PlayerInput);
            
            EventManager=gameObject.AddComponent<EventManager>();

            SoundManager = gameObject.AddComponent<SoundManager>();
            SoundManager.Init(SoundSetting);
            SoundManager.RegCombatEvents();
            FXManager = gameObject.AddComponent<FXManager>();
            FXManager.Init(FXSetting);
            FXManager.RegCombatEvent();

        }
        

        private void OnDestroy()
        {
            PlayerPrefs.Save();
        }
    }
}