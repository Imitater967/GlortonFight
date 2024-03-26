using System;
using UnityEngine;

namespace Script.Projectile.Aubergine
{
    public class TransformRotate : MonoBehaviour
    {
        protected Vector3 rot;
        public float rotSpeed=1;

        private void FixedUpdate()
        {
            if (rot.z > 180)
            {
                rot.z = -180;
            }
            if (rot.z < -180)
            {
                rot.z = 180;
            }
            rot.z += rotSpeed * Time.fixedDeltaTime;
            transform.rotation=Quaternion.Euler(rot);
        }
    }
}