using UnityEngine;

namespace Script.Character.Peach
{
    public class PeachFighter : GlortonFighter
    { 

        protected override void Awake()
        {
            base.Awake();
            DestroyImmediate(sprite);
            sprite=components.AddComponent<PeachSprite>();
            sprite.RegInit(this);            
            DestroyImmediate(combat);
            combat = gameObject.AddComponent<PeachCombat>();
            combat.RegInit(this);
            combat.setting = combatSetting; 
            combat.kick = kickAttackArea;
            combat.punch = punckAttackArea;
            combat.upPunch = upPunchAttackArea;
            combat.throwArea = throwAttackArea;
        }
    }
}