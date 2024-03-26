using System;

namespace Script.Game.Event
{
    public class SceneEvent
    {
        public Action OnLobbyLoaded;
        public Action OnRoomLoaded;
        //addective
        public Action OnCombatLoaded;
        public Action OnCombatUnloaded;
        public Action<string> OnSceneLoaded;
        public Action<string> OnSceneUnloaded;
        public SceneEvent()
        {
            OnSceneLoaded += (scenename) =>
            {
                if (scenename.Equals("Lobby"))
                {
                    OnLobbyLoaded?.Invoke();
                }

                if (scenename.Equals("Room"))
                {
                    OnRoomLoaded?.Invoke();
                }

                if (scenename.StartsWith("Map_"))
                {
                    OnCombatLoaded?.Invoke();
                }
            };
        }
        
    }
}