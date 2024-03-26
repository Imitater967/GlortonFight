using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Character
{
    public class StrawBerryFighter: GlortonFighter
    {
        [Header("草莓特殊设置")]
        [SerializeField]
        protected GameObject sicklePrefab;
        [SerializeField]
        protected GameObject lasterPrefab;
        [SerializeField]
        protected BoxCollider2D saArea;
        public GameObject lasterObj;
        protected override void Awake()
        {
            base.Awake();
            DestroyImmediate(combat);
            DestroyImmediate(sprite);
            sprite=components.AddComponent<StrawBerrySprite>();
            sprite.RegInit(this);
            combat = components.AddComponent<StrawBerryCombat>();
            combat.RegInit(this);
            combat.setting = combatSetting; 
            combat.kick = kickAttackArea;
            combat.punch = punckAttackArea;
            combat.upPunch = upPunchAttackArea;
            combat.throwArea = throwAttackArea;
            
            var sbCombat = combat as StrawBerryCombat;
            
            lasterObj=Instantiate(lasterPrefab,transform);
            lasterObj.name = lasterPrefab.name;
            
            GameObject sickle=Instantiate(sicklePrefab,transform);
            sickle.name = sicklePrefab.name;
            sickle.SetActive(false);

            sbCombat.rangedAttackArea = lasterObj.GetComponent<BoxCollider2D>();
            sbCombat.specialAttackArea = saArea;
            saArea.gameObject.SetActive(false);
            //重启动画机,让动画机找到骨骼 
            animator.Rebind();
        }


    }
}