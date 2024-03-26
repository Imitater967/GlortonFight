using UnityEngine;

namespace Script
{
    [CreateAssetMenu( menuName = "GlortonFight/AudioAsset", order = 0)]
    public class AudioAsset: ScriptableObject
    {
        public AudioClip audioClip;
        public float volume=1;
    }
}