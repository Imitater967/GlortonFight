using System;
using Game;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Script.UI.Room
{
    public class CoinDrag : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private SelectViewController _controller;
        private Vector2 _originPos;
        private Transform _originParent;
        private Coin _coin;

        public void Init(Coin coin)
        {
            _controller = GetComponentInParent<SelectViewController>();
            this._coin = coin;
            _originParent = coin.DragParent;
            _originPos = transform.position;
            transform.SetParent(_originParent);
        }
 

        public void OnBeginDrag(PointerEventData eventData)
        {
            _coin.RaycastTarget.raycastTarget = false;
            transform.SetParent(_originParent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            var target = eventData.pointerEnter; 
            if (eventData.pointerEnter!=null && target.tag.Equals("FighterBox"))
            {     
                transform.SetParent(target.transform);
                transform.SetAsLastSibling();
               if( target.TryGetComponent(out FighterBox box))
               {
                   _controller.Select(box.type);
               }
            }
            else
            {
                transform.SetParent(_originParent);
                transform.position = _originPos;
            }
            
            _coin.RaycastTarget.raycastTarget = true;
        }
        
    }
}