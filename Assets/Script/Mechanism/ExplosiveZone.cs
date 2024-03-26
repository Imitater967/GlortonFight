using System;
using Game;
using Script.Character;
using Script.Game;
using UnityEngine;

namespace Script.Mechanism
{


    public class ExplosiveZone : MonoBehaviour
    {
        public ExplosiveZoneProperties Properties;
        [SerializeField] private float _explodeRemain = 0.1f;
        private BoxCollider2D _boxCollider2D;

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (_explodeRemain <= 0)
            {
                _explodeRemain = Properties.ExplodeInterval;
                var param = _boxCollider2D.GetBoxCheckParam();
                Collider2D[] collider2D = Physics2D.OverlapBoxAll(param.center, param.size, 0, Utils.LAYER_PLAYERS);
                foreach (var collider2D1 in collider2D)
                {
                    if (collider2D1.TryGetComponent(out GlortonFighter fighter))
                    {
                        ApplicationManager.Instance.EventManager.Mechanism.OnPlayerTriggerExplosiveZone?.Invoke(this,
                            fighter);
                    }
                }
            }
            else
            {
                _explodeRemain -= Time.deltaTime;
            }
        }
        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     Debug.Log("Trigger enter "+other.transform.name);
        //     if (other.TryGetComponent(out GlortonFighter fighter))
        //     {
        //         ApplicationManager.Instance.EventManager.Mechanism.OnPlayerTriggerExplosiveZone?.Invoke(this,fighter);
        //     } 
        // }


    }
}