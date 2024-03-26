using UnityEngine;

namespace Script.Character
{

    [CreateAssetMenu( menuName = "GlortonFight/Character/InputSetting", order = 0)]
    public class ChrInputSetting : ScriptableObject
    {
        [Header("键盘设置")] public KeyCode jumpCode = KeyCode.W;
        public KeyCode leftCode = KeyCode.A;
        public KeyCode rightCode = KeyCode.D;
        public KeyCode crouchCode = KeyCode.S;
        public KeyCode attack0Code = KeyCode.J;
        public KeyCode attack1Code = KeyCode.K;
        public KeyCode shieldCode = KeyCode.L;
        public float flyBlockDuration=1;
        public float saCheckInterval = 0.1f;
    }
}