using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Game.Room
{
    public partial class RoomManager
    {
        void HideAllUI()
        {
            RoomController.Hide();
        }
        void LoadGameScene()
        {
            var s = _mapType.Value.ToString();
            string map = "Scenes/Combat/Map_" + s;
            CanJoin = false;
            if (IsValidCanLoadScene(map))
            {
                NetworkManager.SceneManager.LoadScene(map, LoadSceneMode.Additive);
                Debug.Log("Loading scene "+map);
            }
            else
            {
                NetworkManager.SceneManager.LoadScene(_fallbackScene, LoadSceneMode.Additive);
                Debug.LogError("Scene "+map+" not found, loading fallback");
            }
        }
        bool IsValidCanLoadScene(string scenename)
        {
            if (Application.CanStreamedLevelBeLoaded(scenename))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}