using UnityEngine;

namespace Script.Character.Coffee
{
    public class CoffeeFighter : GlortonFighter
    {
        public BoxCollider2D specialAttackCheckArea;
        protected override void Awake()
        {
            base.Awake();
            DestroyImmediate(combat);
            combat = gameObject.AddComponent<CoffeeCombat>();
            combat.RegInit(this);
            combat.setting = combatSetting; 
            combat.kick = kickAttackArea;
            combat.punch = punckAttackArea;
            combat.upPunch = upPunchAttackArea;
            combat.throwArea = throwAttackArea;
        }
    }
}