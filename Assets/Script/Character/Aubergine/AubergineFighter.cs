using UnityEngine;

namespace Script.Character.Aubergine
{
    public class AubergineFighter : GlortonFighter
    {
        [Header("茄子特殊设置")] public BoxCollider2D specialAttackArea1;
        [Header("茄子特殊设置")] public BoxCollider2D specialAttackArea2;
        protected override void Awake()
        {
            base.Awake();
            DestroyImmediate(sprite);
            sprite=components.AddComponent<AubergineSprite>();
            sprite.RegInit(this);
            DestroyImmediate(combat);
            combat = gameObject.AddComponent<AubergineCombat>();
            combat.RegInit(this);
            combat.setting = combatSetting; 
            combat.kick = kickAttackArea;
            combat.punch = punckAttackArea;
            combat.upPunch = upPunchAttackArea;
            combat.throwArea = throwAttackArea;
        }
    }
}