using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Projectile
{
    [CreateAssetMenu( menuName = "GlortonFight/Projectile/Setting", order = 0)]
    public class ProjectileSetting : ScriptableObject
    {
        public LayerMask checkLayers;
        public float lifeTime; 
        [FormerlySerializedAs("triggerTrigger")] public bool triggerMechanism;
    }
}