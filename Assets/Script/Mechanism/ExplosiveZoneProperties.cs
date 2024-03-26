using UnityEngine;

namespace Script.Mechanism
{
    [CreateAssetMenu( menuName = "GlortonFight/Mechanism/ExplosiveZone", order = 0)]
    public class ExplosiveZoneProperties: ScriptableObject
    {
        public float ForceMultiplier=2;
        public float VerticalForce=3;
        public float HorizontalForce=4;
        public float Damage = 0.5f;
        public float ExplodeInterval = 1f;
    }
}