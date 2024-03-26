using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.UI.Game
{
    public class PlayerHealthBarView : MonoBehaviour
    {
        [FormerlySerializedAs("under5Style")] public GameObject Under5Style;
        [FormerlySerializedAs("above5Style")] public GameObject Above5Style;
        [FormerlySerializedAs("scoreStyle")] public GameObject ScoreStyle;
        [FormerlySerializedAs("under5Slots")] public GameObject[] Under5Slots;
        [FormerlySerializedAs("above5Text")] public TMP_Text Above5Text;
        public Image Background;
        public Image[] HealthSlots;
        public TMP_Text DamageAmount; 

    }
}