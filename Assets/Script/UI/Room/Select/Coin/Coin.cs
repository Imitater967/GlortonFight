using System;
using Script.Game;
using Script.Game.Room;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI.Room
{
    public class Coin: MonoBehaviour
    {
        [NonSerialized]
        public Image RaycastTarget;
        [NonSerialized] public TMP_Text Text;  

        private CoinDrag _drag;
        public Transform DragParent;

        private void Awake()
        {
            _drag=gameObject.AddComponent<CoinDrag>();
            
            RaycastTarget = GetComponent<Image>();
            Text = GetComponentInChildren<TMP_Text>(); 
            Text.text = "You";
         }

        private void Start()
        {
            _drag.Init(this);
        }
    }
}