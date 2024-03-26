using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI.Room
{
    public class NetworkViewController : MonoBehaviour
    {
        public Button client;
        public Button host;

        private void Awake()
        {
            client.onClick.AddListener(() => { NetworkManager.Singleton.StartClient(); });
            host.onClick.AddListener(() => { NetworkManager.Singleton.StartHost();});
        }
    }
}