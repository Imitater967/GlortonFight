using System;
using UnityEngine;

namespace Script.FX
{
    public class AutoDestroy: MonoBehaviour
    {
        public float lifeTime;
        private float timeRemain;

        private void Awake()
        {
            timeRemain = lifeTime;
        }

        private void Update()
        {
            timeRemain -= Time.deltaTime;
            if (timeRemain <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}