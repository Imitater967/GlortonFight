using Script.Game;
using Script.Game.Room;
using UnityEngine;

namespace Game
{
    public abstract class System: MonoBehaviour
    {
        protected EventManager _eventManager=>ApplicationManager.Instance.EventManager;

        protected GameManager _gameManager => ApplicationManager.Instance.GameManager;

        protected RoomManager _roomManager => ApplicationManager.Instance.RoomManager;
        //游戏刚刚开始
        public virtual void OnGameInit()
        { 
        }
        //玩家生成,开始倒计时
        public virtual void OnGamePrepared()
        { 
        }
        //倒计时结束,游戏开始
        public virtual void OnGameStarted()
        { 
        }
    }
}