using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.UI.Room
{
    public class RoomView : MonoBehaviour
    {
        //设置
        public Button settingSwapButton;
        public TMP_Text settingModeText;
        public Button settingAddButton;
        public Button settingReduceButton;
        public TMP_Text settingValueText;
        //准备
        public Button readyOrStartButton;

        [FormerlySerializedAs("readyOrPrepareText")] public TMP_Text readyText;
        //地图
        public Button mapSelectBnt;

        public Image mapPreview;
        //返回
        public Button returnBnt;
        
        
        
        
    }
}