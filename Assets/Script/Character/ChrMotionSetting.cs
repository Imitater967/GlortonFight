using UnityEngine;

namespace Script.Character
{
    
    [CreateAssetMenu( menuName = "GlortonFight/Character/MotionSetting", order = 0)]
    public class ChrMotionSetting : ScriptableObject
    {
        public float downOffset=0.1f;
        //public float sizeExtend=0.1f;
        public LayerMask downCheckMask;
        public float secondJumpHeight = 1;
        public float jumpHeight=1;
        public float upPunchHeight=0.08f;
        public float saHeight = 0f;
        public float gravity = -9.8f;
        public float accleratedDownSpeed = -3.5f;
        public float minYSpeed = -5f; 
        public float moveSpeed = 1;
        public bool freezeWhileSpecialAttack;
        public bool canMoveXWhileSpecialAttack;
    }
}