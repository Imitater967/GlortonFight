using UnityEngine;

namespace Script.Character
{
 
    [CreateAssetMenu( menuName = "GlortonFight/Character/CombatSetting", order = 0)]
    public class ChrCombatSetting : ScriptableObject
    {
        public LayerMask attackMask;
        public float punchDamage=0.05f;
        public float punchForce=1f;
        public float upPunchDamage = 0.1f;
        public float upPunchForce = 0.5f;
        public float kickDamage = 0.1f;
        public float kickForce = 1f;
        public float throwDamage = 0.2f;
        public float throwForce = 1f;
        public float saDamage = 0.1f;
        public float saForce = 1f;
        public float rangedDamage=0.05f;
        public float rangedForce=1;
        public float shockTime=1;
        public float shockFactor=0.5f;
        public float shockMaxTime=2;
        public Vector2 punchForceOffset = new Vector2(0, 0.3f);
        public Vector2 throwForceOffset = new Vector2(0, 0.45f);
        public Vector2 kickForceOffset = new Vector2(0, 0.3f);
        public Vector2 upPunchForceOffset= new Vector2(0, 1f);
        public Vector2 rangedForceOffset = new Vector2(0, 0);
        public Vector2 saForceOffset = new Vector2(0, 0.3f);
        public float inputBlockDuration = 0.5f;
        public float inputBlockFactor = 0.5f;
        public float inputBlockMax = 1.5f;
    }
}