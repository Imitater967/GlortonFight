using System;
using Game;
using UnityEngine;

namespace Script.Manager.Event
{
    public class SceneEventHelper: MonoBehaviour
    {
        private void Awake()
        {
            EventManager.Instance.Scene.OnSceneLoaded?.Invoke(gameObject.scene.name);
        }

        private void OnDestroy()
        {
            EventManager.Instance.Scene.OnSceneUnloaded?.Invoke(gameObject.scene.name);
        }
    }
}